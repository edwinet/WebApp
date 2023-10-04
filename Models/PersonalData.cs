namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// personalData
    /// </summary>
    public static PersonalData personalData
    {
        get => HttpData.Get<PersonalData>("personalData")!;
        set => HttpData["personalData"] = value;
    }

    /// <summary>
    /// Page class (personal_data)
    /// </summary>
    public class PersonalData : PersonalDataBase
    {
        // Constructor
        public PersonalData(Controller controller) : base(controller)
        {
        }

        // Constructor
        public PersonalData() : base()
        {
        }

        // Server events
    }

    /// <summary>
    /// Page base class
    /// </summary>
    public class PersonalDataBase : WebAppPage
    {
        // Page ID
        public string PageID = "personal_data";

        // Project ID
        public string ProjectID = "{89456192-A767-4B60-B043-591A4AA6A5D7}";

        // Page object name
        public string PageObjName = "personalData";

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
        public PersonalDataBase()
        {
            // Initialize
            CurrentPage = this;

            // Language object
            Language = ResolveLanguage();

            // Start time
            StartTime = Environment.TickCount;

            // Debug message
            LoadDebugMessage();

            // Open connection
            Conn = GetConnection();

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
        public string PageName => "personaldata";

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
        public PersonalDataBase(Controller? controller = null): this() { // DN
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

            // Global Page Unloaded event
            PageUnloaded();

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

        // Properties
        public DbField<SqlDbType> Password = new ("PersonalData", "password", 202, SqlDbType.NVarChar) {
                TableName = "PersonalData",
                Name = "password",
                Expression = "password"
            };

        public bool IsModal = false; // DN

        /// <summary>
        /// Page run
        /// </summary>
        /// <returns>Page result</returns>
        public override async Task<IActionResult> Run()
        {
            // Create Password field object (used by validation only)
            Password.EditAttrs.AppendClass("form-control ew-form-control");

            // Use layout
            if (!Empty(Param("layout")) && !Param<bool>("layout"))
                UseLayout = false;

            // User profile
            Profile = ResolveProfile();

            // Security
            Security = ResolveSecurity();

            // Global Page Loading event
            PageLoading();

            // Check token
            if (!await ValidPost())
                End(Language.Phrase("InvalidPostRequest"));

            // Check action result
            if (ActionResult != null) // Action result set by server event // DN
                return ActionResult;

            // Create token
            CreateToken();
            CurrentBreadcrumb = new ();
            var url = CurrentUrl(); // DN
            CurrentBreadcrumb.Add("personal_data", "PersonalDataTitle", url, "ew-personal-data", "", true);
            Heading = Language.Phrase("PersonalDataTitle");
            if (RouteValues.TryGetValue("cmd", out object? command)) { // DN
                if (SameText(command, "Download")) {
                    return await PersonalDataResult();
                } else if (SameText(command, "Delete") && IsPost()) {
                    if (await DeletePersonalData())
                        return Controller.LocalRedirect(GetUrl("logout?deleted=1"));
                }
            }

            // Set LoginStatus, Page Rendering and Page Render
            if (!IsApi() && !IsTerminated) {
                SetupLoginStatus(); // Setup login status

                // Pass login status to client side
                SetClientVar("login", LoginStatus);
            }
            return PageResult();
        }

        // Get person data
        protected async Task<IActionResult> PersonalDataResult() {
            var result = new Dictionary<string, object>();
            var fldNames = new string[] {"id", "first_name", "last_name", "email", "username", "password", "created_at", "updated_at"};
            string filter = GetUserFilter(Config.LoginUsernameFieldName, CurrentUserName());
            string sql = UserTable.GetSql(filter); // DN
            var row = await UserTableConn.GetRowAsync(sql);
            if (row != null) {
                foreach (var fldName in fldNames) {
                    if (row.ContainsKey(fldName) && GetUserInfo(fldName, row) is var info && info != null)
                        result.Add(fldName, info);
                }

                // Call PersonalData Downloading event
                PersonalDataDownloading(result);
                AddHeader(HeaderNames.ContentDisposition, "attachment; filename=" + PersonalDataFileName);
                return Controller.Json(result);
            } else {
                FailureMessage = Language.Phrase("NoRecord"); // No record found
                return PageResult();
            }
        }

        // Delete personal data
        protected async Task<bool> DeletePersonalData() {
            string filter = GetUserFilter(Config.LoginUsernameFieldName, CurrentUserName());
            string sql = UserTable.GetSql(filter); // DN
            var row = await UserTableConn.GetRowAsync(sql);
            if (row == null) {
                FailureMessage = Language.Phrase("NoRecord"); // No record found
                return false;
            }
            var pwd = Post(Password.FieldVar);
            if (ComparePassword(ConvertToString(GetUserInfo(Config.LoginPasswordFieldName, row)), pwd)) {
                try {
                    if (Config.DeleteUploadFiles)
                        UserTable.DeleteUploadedFiles(row);
                    if (await UserTable.DeleteAsync(row) > 0) {
                        // Call PersonalData Deleted event
                        PersonalDataDeleted(row);
                        return true;
                    } else {
                        FailureMessage = Language.Phrase("PersonalDataDeleteFailure");
                        return false;
                    }
                } catch (Exception e) {
                    if (Config.Debug)
                        throw;
                    FailureMessage = Language.Phrase("PersonalDataDeleteFailure") + ": " + e.Message; // Set up error message
                    return false;
                }
            } else {
                FailureMessage = Language.Phrase("InvalidPassword");
                return false;
            }
        }

        // Personal Data Deleted event
        public void PersonalDataDeleted(Dictionary<string, object> row) {
            //Log("PersonalData Deleted");
        }

        // Personal Data Downloading event
        public void PersonalDataDownloading(Dictionary<string, object> row) {
            //Log("PersonalData Downloading");
        }
    } // End page class
} // End Partial class
