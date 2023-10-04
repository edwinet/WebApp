namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]/")]
[EnableCors("ApiCorsPolicy")]
[HttpCacheExpiration(CacheLocation = CacheLocation.Private, NoStore = true, MaxAge = 0)]
public abstract class ApiController : Controller
{
    public static Lang Language = ResolveLanguage();

    // Dispose
    // protected override void Dispose(bool disposing)
    // {
    //     try
    //     {
    //         base.Dispose(disposing);
    //     }
    //     finally
    //     {
    //     }
    // }
}

/// <summary>
/// List records from a table
/// </summary>
/// <example>
/// api/list/cars
/// </example>
[Authorize(Policy = "ApiUserLevel")]
public class ListController : ApiController
{
    [HttpGet("{table}")]
    public async Task<IActionResult> List([FromRoute] string table)
    {
        if (Config.NamedTypes.TryGetValue(table, out Type? classType) && classType != null) {
            var obj = CreateInstance(classType.Name + "List", new object[] { this });
            if (obj != null)
                return await obj.Run();
        }
        return new JsonBoolResult(new { success = false, error = Language.Phrase("FailedToCreate"), version = Config.ProductVersion }, false);
    }
}

/// <summary>
/// Get a record from a table
/// </summary>
/// <example>
/// api/view/cars/1/...
/// </example>
[Authorize(Policy = "ApiUserLevel")]
public class ViewController : ApiController
{
    [HttpGet("{table}/{**key}")]
    public async Task<IActionResult> Get([FromRoute] string table)
    {
        if (Config.NamedTypes.TryGetValue(table, out Type? classType) && classType != null) {
            var obj = CreateInstance(classType.Name + "View", new object[] { this });
            if (obj != null)
                return await obj.Run();
        }
        return new JsonBoolResult(new { success = false, error = Language.Phrase("FailedToCreate"), version = Config.ProductVersion }, false);
    }
}

/// <summary>
/// Insert a record to a table by POST
/// </summary>
/// <example>
/// api/add
/// </example>
[Authorize(Policy = "ApiUserLevel")]
public class AddController : ApiController
{
    [HttpPost("{table}")]
    public async Task<IActionResult> Add([FromRoute] string table)
    {
        if (Config.NamedTypes.TryGetValue(table, out Type? classType) && classType != null) {
            var obj = CreateInstance(classType.Name + "Add", new object[] { this });
            if (obj != null)
                return await obj.Run();
        }
        return new JsonBoolResult(new { success = false, error = Language.Phrase("FailedToCreate"), version = Config.ProductVersion }, false);
    }
}

/// <summary>
/// Edit a record by POST
/// </summary>
/// <example>
/// api/edit/cars/1/...
/// </example>
[Authorize(Policy = "ApiUserLevel")]
public class EditController : ApiController
{
    [HttpPost("{table}/{**key}")]
    public async Task<IActionResult> Edit([FromRoute] string table)
    {
        if (Config.NamedTypes.TryGetValue(table, out Type? classType) && classType != null) {
            var obj = CreateInstance(classType.Name + "Edit", new object[] { this });
            if (obj != null)
                return await obj.Run();
        }
        return new JsonBoolResult(new { success = false, error = Language.Phrase("FailedToCreate"), version = Config.ProductVersion }, false);
    }
}

/// <summary>
/// Delete a record from a table
/// </summary>
/// <example>
/// api/delete/cars/1/...
/// </example>
[Authorize(Policy = "ApiUserLevel")]
public class DeleteController : ApiController
{
    [HttpGet("{table}/{**key}")]
    [HttpPost("{table}")]
    public async Task<IActionResult> Delete([FromRoute] string table)
    {
        if (Config.NamedTypes.TryGetValue(table, out Type? classType) && classType != null) {
            var obj = CreateInstance(classType.Name + "Delete", new object[] { this });
            if (obj != null)
                return await obj.Run();
        }
        return new JsonBoolResult(new { success = false, error = Language.Phrase("FailedToCreate"), version = Config.ProductVersion }, false);
    }
}

/// <summary>
/// Export file
/// </summary>
/// <example>
/// api/export/cars
/// </example>
[Authorize(Policy = "ApiUserLevel2")]
public class ExportController : ApiController
{
    /// <summary>
    /// Export file by export type and table name
    /// </summary>
    /// <param name="type">Export type</param>
    /// <param name="table">Table name</param>
    /// <returns>Export content</returns>
    [HttpPost("{type}/{table}")]
    [HttpGet("{type}/{table}")]
    public async Task<IActionResult> ExportData([FromRoute] string type, [FromRoute] string table)
    {
        ExportHandler obj = new (this);
        return await obj.ExportData(type, table);
    }

    /// <summary>
    /// Export file by export type, table name and primary key
    /// </summary>
    /// <param name="type">Export type</param>
    /// <param name="table">Table name</param>
    /// <param name="key">Primary key</param>
    /// <returns>Export content</returns>
    [HttpPost("{type}/{table}/{**key}")]
    [HttpGet("{type}/{table}/{**key}")]
    public async Task<IActionResult> ExportData([FromRoute] string type, [FromRoute] string table, [FromRoute] string key)
    {
        ExportHandler obj = new (this);
        return await obj.ExportData(type, table, key.Split('/'));
    }

    /// <summary>
    /// Search export file
    /// </summary>
    /// <param name="search">File guid / search</param>
    /// <returns>Export content</returns>
    [HttpGet("{search}")]
    public async Task<IActionResult> Search([FromRoute] string search)
    {
        ExportHandler obj = new (this);
        return await obj.Search(search);
    }
}

/// <summary>
/// Register a record to a table by POST
/// </summary>
/// <example>
/// api/register
/// </example>
[AllowAnonymous]
public class RegisterController : ApiController
{
    // Post
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var obj = CreateInstance("Register", new object[] { this });
        if (obj != null)
            return await obj.Run();
        return new JsonBoolResult(new { success = false, error = Language.Phrase("FailedToCreate"), version = Config.ProductVersion }, false);
    }
}

/// <summary>
/// Login by POST
/// </summary>
/// <example>
/// api/login
/// </example>
[AllowAnonymous]
public class LoginController : ApiController
{
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] LoginModel model)
    {
        // User profile
        Profile = ResolveProfile();

        // Security
        Security = ResolveSecurity();

        // As an example, AuthService.CreateToken can return Jose.JWT.Encode(claims, YourTokenSecretKey, Jose.JwsAlgorithm.HS256);
        if (await Security.ValidateUser(model, false))
            return Ok(new { JWT = Security.CreateJwtToken(model.Expire * 60 * 60, model.Permission) });
        return BadRequest(Language.Phrase("InvalidUidPwd"));
    }
}

/// <summary>
/// Get a file
/// </summary>
/// <example>
/// api/file/cars/Picture/1/...
/// </example>
public class FileController : ApiController
{
    /// <summary>
    /// Get file by table name, field name and primary key
    /// </summary>
    /// <param name="table">Table name</param>
    /// <param name="field">Field name</param>
    /// <param name="key">Primary key</param>
    /// <returns>File result</returns>
    [Authorize(Policy = "UserLevel")]
    [HttpGet("{table}/{field}/{**key}")]
    public async Task<IActionResult> GetFile([FromRoute] string table, [FromRoute] string field, [FromRoute] string key)
    {
        Language = ResolveLanguage(); // Set up CurrentNumberFormat
        FileViewer obj = new (this);
        return await obj.GetFile(table, field, key.Split('/'));
    }

    /// <summary>
    /// Get file by table name and file path
    /// </summary>
    /// <param name="table">Table name</param>
    /// <param name="fn">File path</param>
    /// <returns>File result</returns>
    [Authorize(Policy = "UserLevel")]
    [HttpGet("{table}/{fn}")]
    public async Task<IActionResult> GetFile([FromRoute] string table, [FromRoute] string fn)
    {
        Language = ResolveLanguage(); // Set up CurrentNumberFormat
        FileViewer obj = new (this);
        return await obj.GetFile(table, fn);
    }

    /// <summary>
    /// Get file by file path
    /// </summary>
    /// <param name="fn">File path</param>
    /// <returns>File result</returns>
    [AllowAnonymous]
    [HttpGet("{fn}")]
    public async Task<IActionResult> GetFile([FromRoute] string fn)
    {
        Language = ResolveLanguage(); // Set up CurrentNumberFormat
        FileViewer obj = new (this);
        return await obj.GetFile(fn);
    }
}

/// <summary>
/// File upload
/// </summary>
/// <example>
/// api/upload
/// </example>
[Authorize(Policy = "ApiUserLevel")]
public class UploadController : ApiController
{
    [HttpPost]
    [HttpPut]
    public async Task<IActionResult> Post() => await HttpUpload.GetUploadedFiles();
}

/// <summary>
/// File upload with jQuery File Upload
/// </summary>
/// <example>
/// api/jupload
/// </example>
[AutoValidateAntiforgeryToken]
[Authorize(Policy = "UserLevel")]
[ApiExplorerSettings(IgnoreApi = true)]
public class JUploadController : ApiController
{
    [HttpGet]
    [HttpPost]
    [HttpPut]
    public async Task<IActionResult> Index()
    {
        var obj = new UploadHandler(this);
        return await obj.Run();
    }
}

/// <summary>
/// Session handler
/// </summary>
/// <example>
/// api/session
/// </example>
[AutoValidateAntiforgeryToken]
[Authorize(Policy = "UserLevel")]
[ApiExplorerSettings(IgnoreApi = true)]
public class SessionController : ApiController
{
    [HttpGet]
    public IActionResult Get()
    {
        var obj = new SessionHandler(this);
        return obj.GetSession();
    }
}

/// <summary>
/// Lookup (UpdateOption/ModalLookup/AutoSuggest/AutoFill)
/// </summary>
/// <example>
/// api/lookup
/// </example>
[Authorize(Policy = "UserLevel")]
[ApiExplorerSettings(IgnoreApi = true)]
public class LookupController : ApiController
{
    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> Post([FromForm] string page) // Single request
    {
        dynamic? obj = Resolve(page); // Get object created in API permission handler
        var res = await obj?.Lookup() ?? new { success = false, error = Language.Phrase("FailedToCreate"), version = Config.ProductVersion };
        return new JsonBoolResult((object)res, res.TryGetValue("result", out object? v) ? ConvertToString(v) == "OK" : false);
    }

    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> Batch([FromBody] List<Dictionary<string, string>> pages) // Batch request (json)
    {
        List<object> responses = new ();
        foreach (Dictionary<string, string> req in pages) {
            if (req.TryGetValue(Config.ApiLookupPage, out string? page) && req.TryGetValue(Config.ApiFieldName, out string? fieldName)) {
                dynamic? obj = Resolve(page); // Get object
                dynamic? tbl = obj?.FieldByName(fieldName)?.Lookup?.GetTable(); // Get table
                if (tbl != null) {
                    Security.LoadTablePermissions(tbl.TableVar);
                    object res = Security.CanLookup
                        ? await obj?.Lookup(req) ?? new { success = false, error = Language.Phrase("FailedToCreate"), version = Config.ProductVersion }
                        : new { success = false, error = "401 " + Language.Phrase("401"), version = Config.ProductVersion };
                    responses.Add(res);
                }
            }
        }
        return Json(responses);
    }
}

/// <summary>
/// Chart exporter
/// </summary>
/// <example>
/// api/chart
/// </example>
[AutoValidateAntiforgeryToken]
[Authorize(Policy = "UserLevel")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ChartController : ApiController
{
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var exporter = new ChartExporter(this);
        return await exporter.Export();
    }
}

/// <summary>
/// Permissions
/// </summary>
/// <example>
/// api/permissions/{userlevel}
/// </example>
[Authorize(Policy = "ApiUserLevel")]
public class PermissionsController : ApiController
{
    [AllowAnonymous]
    [HttpGet("{userlevel?}")]
    public IActionResult Get([FromRoute] string userlevel)
    {
        if (!ValidApiRequest())
            return new EmptyResult();
        Security.SetupUserLevel();

        // Check user level
        int userLevel = -2; // Default anonymous
        List<int> userLevels = new ();
        userLevels.Add(userLevel);
        if (Security.IsLoggedIn) {
            if (Security.IsSysAdmin && IsNumeric(userlevel) && userlevel != "-1") { // Get permissions for user level
                if (Security.UserLevelIDExists(Convert.ToInt32(userlevel))) { // Make sure user level exists
                    userLevel = Convert.ToInt32(userlevel);
                    userLevels.Clear();
                    userLevels.Add(userLevel);
                }
            } else { // Get current user permissions
                userLevel = Convert.ToInt32(Security.CurrentUserLevelID);
                userLevels = Security.UserLevelID;
            }
        }
        Dictionary<string, int> privs = new ();
        var wrkTable = Config.UserLevelTablePermissions;
        foreach (var table in wrkTable) {
            if (table.Allowed) {
                int priv = 0;
                foreach (int lvl in userLevels)
                    priv |= Security.GetUserLevelPriv(table.ProjectId + table.TableName, lvl);
                privs.Add(table.TableVar, priv);
            }
        }
        return Json(new { userlevel = userLevel, permissions = privs });
    }

    // Post with route
    [HttpPost("{userlevel}")]
    public async Task<IActionResult> PostWithRoute([FromRoute] string userlevel)
    {
        if (!ValidApiRequest())
            return new EmptyResult();
        await Security.SetupUserLevels();

        // Check if admin
        if (!Security.IsSysAdmin)
            return new EmptyResult();

        // Check user level
        int userLevel;
        if (IsNumeric(userlevel) && userlevel != "-1") { // Set permissions for user level
            userLevel = Convert.ToInt32(userlevel);
        } else {
            return new EmptyResult();
        }
        Dictionary<string, int> privs = new ();
        Dictionary<string, int> privsOut = new ();
        StringValues sv;
        var wrkTable = Config.UserLevelTablePermissions;
        foreach (var table in wrkTable) {
            if (table.Allowed && Post(table.TableVar, out sv)) {
                int priv = Convert.ToInt32(sv);
                privs.Add(table.ProjectId + table.TableName, priv);
                privsOut.Add(table.TableName, priv);
            }
        }
        var mi = Security.GetType().GetMethod("UpdatePermissions", BindingFlags.Public | BindingFlags.Instance);
        if (mi?.Invoke(Security, new object[] { userLevel, privs }) is Task<bool> res)
            await res; // Update Permissions
        return Json(new { success = true, userlevel = userLevel, permissions = privsOut });
    }
}
