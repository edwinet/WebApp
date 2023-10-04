namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// logout
    /// </summary>
    public static Logout logout
    {
        get => HttpData.Get<Logout>("logout")!;
        set => HttpData["logout"] = value;
    }

    /// <summary>
    /// Page class (logout)
    /// </summary>
    public class Logout : LogoutBase
    {
        // Constructor
        public Logout(Controller controller) : base(controller)
        {
        }

        // Constructor
        public Logout() : base()
        {
        }

        // Server events
    }

    /// <summary>
    /// Page base class
    /// </summary>
    public class LogoutBase : Users
    {
        // Page ID
        public string PageID = "logout";

        // Project ID
        public string ProjectID = "{89456192-A767-4B60-B043-591A4AA6A5D7}";

        // Page object name
        public string PageObjName = "logout";

        // Title
        public string? Title = null; // Title for <title> tag

        // Page headings
        public string Heading = "";

        public string Subheading = "";

        public string PageHeader = "";

        public string PageFooter = "";

        // Token
        public string? Token = null; // DN

        public bool CheckToken = Config.CheckToken;

        // Action result // DN
        public IActionResult? ActionResult;

        // Cache // DN
        public IMemoryCache? Cache;

        // Page layout
        public bool UseLayout = true;

        // Page terminated // DN
        private bool _terminated = false;

        // Is terminated
        public bool IsTerminated => _terminated;

        // Is lookup
        public bool IsLookup => IsApi() && RouteValues.TryGetValue("controller", out object? name) && SameText(name, Config.ApiLookupAction);

        // Is AutoFill
        public bool IsAutoFill => IsLookup && SameText(Post("ajax"), "autofill");

        // Is AutoSuggest
        public bool IsAutoSuggest => IsLookup && SameText(Post("ajax"), "autosuggest");

        // Is modal lookup
        public bool IsModalLookup => IsLookup && SameText(Post("ajax"), "modal");

        // Page URL
        private string _pageUrl = "";

        // Constructor
        public LogoutBase()
        {
            // Initialize
            CurrentPage = this;

            // Language object
            Language = ResolveLanguage();

            // Table object (users)
            if (users == null || users is Users)
                users = this;
            users ??= this;

            // Start time
            StartTime = Environment.TickCount;

            // Debug message
            LoadDebugMessage();

            // Open connection
            Conn = Connection; // DN

            // User table object (users)
            UserTable = Resolve("usertable")!;
            UserTableConn = GetConnection(UserTable.DbId);
        }

        // Page action result
        public IActionResult PageResult()
        {
            if (ActionResult != null)
                return ActionResult;
            SetupMenus();
            return Controller.View();
        }

        // Page heading
        public string PageHeading
        {
            get {
                if (!Empty(Heading))
                    return Heading;
                else
                    return "";
            }
        }

        // Page subheading
        public string PageSubheading
        {
            get {
                if (!Empty(Subheading))
                    return Subheading;
                return "";
            }
        }

        // Page name
        public string PageName => "logout";

        // Page URL
        public string PageUrl
        {
            get {
                if (_pageUrl == "") {
                    _pageUrl = PageName + "?";
                }
                return _pageUrl;
            }
        }

        // Valid post
        protected async Task<bool> ValidPost() => !CheckToken || !IsPost() || IsApi() || Antiforgery != null && HttpContext != null && await Antiforgery.IsRequestValidAsync(HttpContext);

        // Create token
        public void CreateToken()
        {
            Token ??= HttpContext != null ? Antiforgery?.GetAndStoreTokens(HttpContext).RequestToken : null;
            CurrentToken = Token ?? ""; // Save to global variable
        }

        // Constructor
        public LogoutBase(Controller? controller = null): this() { // DN
            if (controller != null)
                Controller = controller;
        }

        /// <summary>
        /// Terminate page
        /// </summary>
        /// <param name="url">URL to rediect to</param>
        /// <returns>Page result</returns>
        public override IActionResult Terminate(string url = "") { // DN
            if (_terminated) // DN
                return new EmptyResult();

            // Page Unload event
            PageUnload();

            // Global Page Unloaded event
            PageUnloaded();
            if (!IsApi())
                PageRedirecting(ref url);

            // Gargage collection
            Collect(); // DN

            // Terminate
            _terminated = true; // DN

            // Return for API
            if (IsApi()) {
                var result = new Dictionary<string, string> { { "version", Config.ProductVersion } };
                if (!Empty(url)) // Add url
                    result.Add("url", GetUrl(url));
                foreach (var (key, value) in GetMessages()) // Add messages
                    result.Add(key, value);
                return Controller.Json(result);
            } else if (ActionResult != null) { // Check action result
                return ActionResult;
            }

            // Go to URL if specified
            if (!Empty(url)) {
                if (!Config.Debug)
                    ResponseClear();
                if (Response != null && !Response.HasStarted) {
                    SaveDebugMessage();
                    return Controller.LocalRedirect(AppPath(url));
                }
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Page run
        /// </summary>
        /// <returns>Page result</returns>
        public override async Task<IActionResult> Run()
        {
            // Use layout
            if (!Empty(Param("layout")) && !Param<bool>("layout"))
                UseLayout = false;

            // User profile
            Profile = ResolveProfile();

            // Security
            Security = ResolveSecurity();
            CurrentAction = Param("action"); // Set up current action

            // Global Page Loading event
            PageLoading();

            // Page Load event
            PageLoad();

            // Check token
            if (!await ValidPost())
                End(Language.Phrase("InvalidPostRequest"));

            // Check action result
            if (ActionResult != null) // Action result set by server event // DN
                return ActionResult;

            // Create token
            CreateToken();
            bool validate = true;
            string username = Security.CurrentUserName;

            // Call User LoggingOut event
            validate = UserLoggingOut(username);
            if (!validate) {
                string lastUrl = Security.LastUrl;
                if (Empty(lastUrl))
                    lastUrl = "index";
                return Terminate(lastUrl); // Go to last accessed URL
            } else {
                Cookie["password"] = ""; // Clear password
                Cookie["lasturl"] = ""; // Clear last URL
                if (Empty(Cookie["autologin"])) // Not auto login
                    Cookie["username"] = ""; // Clear user name
                await WriteAuditTrailOnLogout();

                // Call User LoggedOut event
                UserLoggedOut(username);

                // Clean upload temp folder
                CleanUploadTempPaths(Session.SessionId);

                // Unset all of the session variables
                Session.Clear();

                // Sign out
                if (HttpContext != null)
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // If session expired, show expired message
                if (Param<bool>("expired")) {
                    TempData["heading"] = Language.Phrase("Notice");
                    TempData["failure"] = Language.Phrase("SessionExpired");
                }
                if (Param<bool>("deleted"))
                    TempData["success"] = Language.Phrase("PersonalDataDeleteSuccess");
                return Terminate("login"); // Go to login page
            }
        }

        // Write audit trail on logout
        protected async Task WriteAuditTrailOnLogout() {
            var usr = CurrentUser();
            await WriteAuditTrailAsync(usr, Language.Phrase("AuditTrailLogout"), CurrentUserIpAddress());
        }

        // Page Load event
        public virtual void PageLoad() {
            //Log("Page Load");
        }

        // Page Unload event
        public virtual void PageUnload() {
            //Log("Page Unload");
        }

        // Page Redirecting event
        public virtual void PageRedirecting(ref string url) {
            //url = newurl;
        }

        // Message Showing event
        // type = ""|"success"|"failure"|"warning"
        public virtual void MessageShowing(ref string msg, string type) {
            // Note: Do not change msg outside the following 4 cases.
            if (type == "success") {
                //msg = "your success message";
            } else if (type == "failure") {
                //msg = "your failure message";
            } else if (type == "warning") {
                //msg = "your warning message";
            } else {
                //msg = "your message";
            }
        }

        // User Logging Out event
        public virtual bool UserLoggingOut(string usr) {
            // Enter your code here
            // To cancel, set return value to False
            return true;
        }

        // User Logged Out event
        public virtual void UserLoggedOut(string usr) {
            //Log("User Logged Out");
        }
    } // End page class
} // End Partial class
