var builder = WebApplication.CreateBuilder(args);

// Add additional JSON configuration file
Config.JsonFiles.ForEach(file => JsonConfigurationExtensions.AddJsonFile(builder.Configuration, file.Path, file.Optional, file.ReloadOnChange));

// Configuration
Configuration = builder.Configuration;

// See https://github.com/serilog/serilog-extensions-logging-file#appsettingsjson-configuration
builder.Logging.AddFile(Configuration.GetSection("Logging"));

// Web host
builder.WebHost
    .UseUrls("http://localhost:5000");

// HTTP cache headers
bool noCache = !Config.Cache;
builder.Services.AddHttpCacheHeaders(
    (expirationModelOptions) => {
        expirationModelOptions.CacheLocation = noCache ? CacheLocation.Private : CacheLocation.Public;
        expirationModelOptions.NoStore = noCache; // Note: "no-store, max-age=0" disable caching
        expirationModelOptions.MaxAge = noCache ? 0 : 60;
    },
    (validationModelOptions) => {
        validationModelOptions.MustRevalidate = noCache; // Note: "no-cache" and "max-age=0, must-revalidate" have the same meaning
        validationModelOptions.NoCache = noCache;
    });

// Lower case URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Controller
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddSessionStateTempDataProvider();

// Authorization
builder.Services.AddAuthorization(options => {
    options.AddPolicy("UserLevel", policy => {
        policy.AuthenticationSchemes.Add(CookieAuthenticationDefaults.AuthenticationScheme); // Cookie
        policy.Requirements.Add(new PermissionRequirement()); // User Level security
    });
    options.AddPolicy("ApiUserLevel", policy => { // API
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme); // JWT
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new PermissionRequirement()); // User Level security
    });
    options.AddPolicy("ApiUserLevel2", policy => { // API (Skip RequireAuthenticatedUser)
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme); // JWT
        policy.Requirements.Add(new PermissionRequirement()); // User Level security
    });
});

// Permission handler
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

// Cookie policy
builder.Services.Configure<CookiePolicyOptions>(options => options.CheckConsentNeeded = context => true);

// Memory cache
builder.Services.AddMemoryCache();

// Http client
builder.Services.AddHttpClient();

// Add identity types
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddDefaultTokenProviders();

// Identity Services
builder.Services.AddTransient<IUserStore<ApplicationUser>, CustomUserStore>();
builder.Services.AddTransient<IRoleStore<ApplicationRole>, CustomRoleStore>();

// Add framework services
builder.Services.AddMvc()
    .AddNewtonsoftJson(options => {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    });

// Add HttpContext accessor
builder.Services.AddHttpContextAccessor();

// Adds a default in-memory implementation of IDistributedCache.
builder.Services.AddDistributedMemoryCache();

// Session
builder.Services.AddSession(options => {
    options.Cookie.Name = ".WebApp.Session";
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(Config.SessionTimeout);
});

// Authentication
builder.Services.AddAuthentication(options => {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options => {
        options.Cookie.HttpOnly = Config.CookieHttpOnly;
        options.ExpireTimeSpan = TimeSpan.FromDays(ConvertToDouble(Config.CookieExpires));
        options.SlidingExpiration = true;
        options.LoginPath = new PathString("/login");
        options.AccessDeniedPath = new PathString("/error");
    })
    .AddGoogle(options => { // Google
        options.CorrelationCookie.SameSite = Enum.Parse<Microsoft.AspNetCore.Http.SameSiteMode>(Config.CookieSameSite);
        options.ClientId = Config.Authentications["Google"].Id;
        options.ClientSecret = Config.Authentications["Google"].Secret;
    })
    .AddJwtBearer(options => { // JWT
        options.TokenValidationParameters = new () {
            // Token signature will be verified using a private key
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"] ?? "")),

            // Token will only be valid if contains below domain (e.g http://localhost) for "iss" claim
            ValidateIssuer = true,
            ValidIssuer = Configuration["Jwt:Issuer"] ?? "",

            // Token will only be valid if contains below domain (e.g http://localhost) for "aud" claim
            ValidateAudience = true,
            ValidAudience = Configuration["Jwt:Audience"] ?? "",

            // Token will only be valid if not expired yet, with 5 minutes clock skew
            ValidateLifetime = true
        };
        options.Events = new () {
            OnMessageReceived = context => {
                string authorization = context.Request.Headers["X-Authorization"].ToString();
                if (authorization?.StartsWith("Bearer ") ?? false) { // Authorization header found
                    context.Token = authorization.Substring("Bearer ".Length).Trim();
                } else {
                    context.NoResult();
                }
                return Task.CompletedTask;
            }
        };
    });

// HTTP context accessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// CORS
builder.Services.AddCors(options => {
    // CORS policy
    options.AddPolicy("CorsPolicy", builder => {
        string allowOrigin = Configuration["Cors:AccessControlAllowOrigin"] ?? "";
        string allowHeaders = Configuration["Cors:AccessControlAllowHeaders"] ?? "";
        if (Empty(allowOrigin) || SameString(allowOrigin, "*")) {
            builder.AllowAnyOrigin();
        } else {
            builder.WithOrigins(allowOrigin.Split(',').Select(x => x.Trim()).Where(x => x != "").ToArray()).AllowCredentials();
        }
    });

    // API CORS policy
    options.AddPolicy("ApiCorsPolicy", builder => {
        string allowOrigin = Configuration["Cors:ApiAccessControlAllowOrigin"] ?? "";
        string allowHeaders = Configuration["Cors:ApiAccessControlAllowHeaders"] ?? "";
        if (Empty(allowOrigin) || SameString(allowOrigin, "*")) {
            builder.AllowAnyOrigin();
        } else {
            builder.WithOrigins(allowOrigin.Split(',').Select(x => x.Trim()).Where(x => x != "").ToArray()).AllowCredentials();
        }
    });
});

// Anti-Forgery
builder.Services.AddAntiforgery(options => {
    options.FormFieldName = Config.TokenName;
    options.HeaderName = Config.TokenName.HeaderCase();
});

// Services Add event
ServiceAdd(builder.Services);

// Register services with Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModule()));

// App Build event
AppBuild(builder);

// Build app
var app = builder.Build();
Application = app;

// Cultures
var supportedCultures = new[] { "es-ES" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture("es-ES")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    // app.UseMigrationsEndPoint(); // Not required
} else {
    app.UseExceptionHandler("/home/error");
}

// Run app
app.UseDefaultFiles();
app.UseStaticFiles(StaticFileSettings);
app.UseRouting();
app.UseHttpCacheHeaders();
app.UseCookiePolicy();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapDefaultControllerRoute();
RouteAction(app); // Routes Add event
app.Run();
