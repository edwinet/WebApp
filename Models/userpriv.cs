namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// userpriv
    /// </summary>
    public static Userpriv userpriv
    {
        get => HttpData.Get<Userpriv>("userpriv")!;
        set => HttpData["userpriv"] = value;
    }

    /// <summary>
    /// Page class (userpriv)
    /// </summary>
    public class Userpriv : UserprivBase
    {
        // Constructor
        public Userpriv(Controller controller) : base(controller)
        {
        }

        // Constructor
        public Userpriv() : base()
        {
        }

        // Server events
    }

    /// <summary>
    /// Page base class
    /// </summary>
    public class UserprivBase : Roles
    {
        // Page ID
        public string PageID = "userpriv";

        // Project ID
        public string ProjectID = "{89456192-A767-4B60-B043-591A4AA6A5D7}";

        // Page object name
        public string PageObjName = "userpriv";

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
        public UserprivBase()
        {
            // Initialize
            CurrentPage = this;

            // Language object
            Language = ResolveLanguage();

            // Table object (roles)
            if (roles == null || roles is Roles)
                roles = this;
            roles ??= this;

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
        public string PageName => "userpriv";

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

        // Show Page Header
        public IHtmlContent ShowPageHeader()
        {
            string header = PageHeader;
            PageDataRendering(ref header);
            if (!Empty(header)) // Header exists, display
                return new HtmlString("<p id=\"ew-page-header\">" + header + "</p>");
            return HtmlString.Empty;
        }

        // Show Page Footer
        public IHtmlContent ShowPageFooter()
        {
            string footer = PageFooter;
            PageDataRendered(ref footer);
            if (!Empty(footer)) // Footer exists, display
                return new HtmlString("<p id=\"ew-page-footer\">" + footer + "</p>");
            return HtmlString.Empty;
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
        public UserprivBase(Controller? controller = null): this() { // DN
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

        public bool IsModal = false; // DN

        public bool Disabled; // DN

        public Dictionary<string, object> Privileges = new ();

        public int TableNameCount;

        [AllowNull]
        public List<UserLevel> UserLevelList;

        [AllowNull]
        public List<UserLevelPermission> UserLevelPrivList;

        [AllowNull]
        public List<UserLevelTablePermission> TableList;

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
            if (TableVar != "")
                Security.LoadTablePermissions(TableVar);
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
            CurrentBreadcrumb = new ();
            CurrentBreadcrumb.Add("list", "roles", AppPath("roleslist"), "", "roles");
            var url = CurrentUrl(); // DN
            CurrentBreadcrumb.Add("userpriv", "UserLevelPermission", url);
            Heading = Language.Phrase("UserLevelPermission");
            UserLevelList = Config.UserLevels;
            UserLevelPrivList = Config.UserLevelPermissions;
            TableList = Config.UserLevelTablePermissions.Where(t => t.Allowed).Select(t => { // Set up allowed table list // DN
                var temp = (UserLevelTablePermission)t.Clone();
                List<int> ids = ConvertToString(Security.CurrentUserLevelID).Split(Config.MultipleOptionSeparator).Where(x => int.TryParse(x, out _)).Select(int.Parse).ToList();
                int tempPriv = 0;
                foreach (int id in ids)
                    tempPriv += Security.GetUserLevelPriv(t.ProjectId + t.TableName, id);
                if (((Allow)tempPriv).HasFlag(Allow.Admin)) // Allow Admin
                    temp.Permission = tempPriv;
                return temp;
            }).ToList();
            TableNameCount = TableList.Count;
            StringValues sv;

            // Get action
            if (!Post("action", out sv)) {
                CurrentAction = "show"; // Display with input box
                // Load key from QueryString
                if (RouteValues.TryGetValue("id", out object? fldparm) && !Empty(fldparm)) { // DN
                    id.QueryValue = ConvertToString(fldparm);
                } else if (Get("id", out sv) && !Empty(sv)) {
                    id.QueryValue = sv.ToString();
                } else {
                    return Terminate("roleslist"); // Return to list
                }
                Disabled = (id.QueryValue == "-1"); // DN
            } else {
                CurrentAction = sv.ToString();
                // Get fields from form
                id.FormValue = Post("x_id");
                for (int i = 0; i < TableNameCount; i++) {
                    if (!Empty(Post("Table_" + i)))
                        Privileges.Add(i.ToString(), Post<int>("add_" + i) + Post<int>("delete_" + i) + Post<int>("edit_" + i) + Post<int>("list_" + i) + Post<int>("view_" + i) + Post<int>("search_" + i) + Post<int>("admin_" + i) + Post<int>("import_" + i) + Post<int>("lookup_" + i) + Post<int>("export_" + i));
                }
            }

            // Should not edit own permissions
            if (Security.HasUserLevelID(ConvertToInt(id.CurrentValue)))
                return Terminate("roleslist"); // Return to list
            switch (CurrentAction) {
                case "show": // Display
                    if (!await Security.SetupUserLevels()) // Get all User Level info
                        return Terminate("roleslist"); // Return to list
                    var list = new List<dynamic>();
                    for (int i = 0; i < TableNameCount; i++) {
                        int tempPriv = Security.GetUserLevelPriv(TableList[i].ProjectId + TableList[i].TableName, ConvertToInt(id.CurrentValue));
                        list.Add(new { table = GetTableCaption(i), name = TableList[i].TableName, index = i, permission = tempPriv, allowed = TableList[i].Permission });
                    }
                    Privileges.Add("disabled", Disabled);
                    Privileges.Add("permissions", list);
                    Privileges.Add("ids", Config.Privileges);
                    foreach (string privilege in Config.Privileges)
                        Privileges.Add(privilege, Enum.Parse<Allow>(TitleCaseInvariant(privilege)));
                    break;
                case "update": // Update
                    if (await EditRow()) { // Update record based on key
                        if (Empty(SuccessMessage))
                            SuccessMessage = Language.Phrase("UpdateSuccess"); // Set up update success message
                        // Alternatively, comment out the following line to go back to this page
                        return Terminate("roleslist"); // Return to list
                    }
                    break;
            }

            // Set LoginStatus, Page Rendering and Page Render
            if (!IsApi() && !IsTerminated) {
                SetupLoginStatus(); // Setup login status

                // Pass login status to client side
                SetClientVar("login", LoginStatus);
            }
            return PageResult();
        }

        // Update privileges (using main connection)
        public async Task<bool> EditRow()
        {
            var tbl = (IQueryFactory)Resolve(Config.UserLevelPrivTableName)!;
            List<object[]> valuesCollection = new ();
            foreach (var (key, value) in Privileges) {
                int i = ConvertToInt(key);
                int privilege = ConvertToInt(value) & TableList[i].Permission; // Set maximum allowed privilege (protect from hacking)
                var where = new Dictionary<string, object> {
                        { Config.UserLevelPrivTableNameFieldName, TableList[i].ProjectId + TableList[i].TableName },
                        { Config.UserLevelPrivUserLevelIdFieldName, ConvertToInt(id.CurrentValue) }
                    };
                if (await tbl.GetQueryBuilder(true).Where(where).ExistsAsync()) // If exists
                    await tbl.GetQueryBuilder(true).Where(where).UpdateAsync(new Dictionary<string, object> { { Config.UserLevelPrivPrivField, privilege } });
                else
                    valuesCollection.Add(new object[] {
                        TableList[i].ProjectId + TableList[i].TableName,
                        ConvertToInt(id.CurrentValue),
                        privilege
                    });
            }
            if (valuesCollection.Count > 0)
                await tbl.GetQueryBuilder(true).InsertAsync(new [] {
                        Config.UserLevelPrivTableNameFieldName,
                        Config.UserLevelPrivUserLevelIdFieldName,
                        Config.UserLevelPrivPrivFieldName
                    }, valuesCollection);
            return true;
        }

        // Get table caption
        protected string GetTableCaption(int i) {
            string caption = "";
            if (i < TableNameCount) {
                caption = Language.TablePhrase(TableList[i].TableVar, "TblCaption");
                if (caption == "")
                    caption = TableList[i].Caption;
                if (caption == "")
                    caption = Regex.Replace(TableList[i].TableName, @"^\{\w{8}-\w{4}-\w{4}-\w{4}-\w{12}\}/", ""); // Remove project ID
            }
            return caption;
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

        // Page Load event
        public virtual void PageRender() {
            //Log("Page Render");
        }

        // Page Data Rendering event
        public virtual void PageDataRendering(ref string header) {
            // Example:
            //header = "your header";
        }

        // Page Data Rendered event
        public virtual void PageDataRendered(ref string footer) {
            // Example:
            //footer = "your footer";
        }
    } // End page class
} // End Partial class
