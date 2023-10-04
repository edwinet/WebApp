namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// usersAdd
    /// </summary>
    public static UsersAdd usersAdd
    {
        get => HttpData.Get<UsersAdd>("usersAdd")!;
        set => HttpData["usersAdd"] = value;
    }

    /// <summary>
    /// Page class for users
    /// </summary>
    public class UsersAdd : UsersAddBase
    {
        // Constructor
        public UsersAdd(Controller controller) : base(controller)
        {
        }

        // Constructor
        public UsersAdd() : base()
        {
        }
    }

    /// <summary>
    /// Page base class
    /// </summary>
    public class UsersAddBase : Users
    {
        // Page ID
        public string PageID = "add";

        // Project ID
        public string ProjectID = "{89456192-A767-4B60-B043-591A4AA6A5D7}";

        // Table name
        public string TableName { get; set; } = "users";

        // Page object name
        public string PageObjName = "usersAdd";

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
        public UsersAddBase()
        {
            // Initialize
            CurrentPage = this;

            // Table CSS class
            TableClass = "table table-striped table-bordered table-hover table-sm ew-desktop-table ew-add-table";

            // Language object
            Language = ResolveLanguage();

            // Table object (users)
            if (users == null || users is Users)
                users = this;

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
                else if (!Empty(Caption))
                    return Caption;
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
                if (!Empty(TableName))
                    return Language.Phrase(PageID);
                return "";
            }
        }

        // Page name
        public string PageName => "usersadd";

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

        // Set field visibility
        public void SetVisibility()
        {
            id.Visible = false;
            first_name.SetVisibility();
            last_name.SetVisibility();
            _email.SetVisibility();
            _username.SetVisibility();
            password.SetVisibility();
            mobile.SetVisibility();
            photo.Visible = false;
            role_id.SetVisibility();
            provider.Visible = false;
            activated.Visible = false;
            _profile.Visible = false;
            created_at.SetVisibility();
            updated_at.SetVisibility();
        }

        // Constructor
        public UsersAddBase(Controller? controller = null): this() { // DN
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
                    // Handle modal response (Assume return to modal for simplicity)
                    if (IsModal) { // Show as modal
                        var result = new Dictionary<string, string> { {"url", GetUrl(url)}, {"modal", "1"} };
                        string pageName = GetPageName(url);
                        if (pageName != ListUrl) { // Not List page
                            result.Add("caption", GetModalCaption(pageName));
                            result.Add("view", pageName == "usersview" ? "1" : "0"); // If View page, no primary button
                        } else { // List page
                            // result.Add("list", PageID == "search" ? "1" : "0"); // Refresh List page if current page is Search page
                            result.Add("error", FailureMessage); // List page should not be shown as modal => error
                            ClearFailureMessage();
                        }
                        return Controller.Json(result);
                    } else {
                        SaveDebugMessage();
                        return Controller.LocalRedirect(AppPath(url));
                    }
                }
            }
            return new EmptyResult();
        }

        // Get all records from datareader
        [return: NotNullIfNotNull("dr")]
        protected async Task<List<Dictionary<string, object>>> GetRecordsFromRecordset(DbDataReader? dr)
        {
            var rows = new List<Dictionary<string, object>>();
            while (dr != null && await dr.ReadAsync()) {
                await LoadRowValues(dr); // Set up DbValue/CurrentValue
                if (GetRecordFromDictionary(GetDictionary(dr)) is Dictionary<string, object> row)
                    rows.Add(row);
            }
            return rows;
        }

        // Get all records from the list of records
        #pragma warning disable 1998

        protected async Task<List<Dictionary<string, object>>> GetRecordsFromRecordset(List<Dictionary<string, object>>? list)
        {
            var rows = new List<Dictionary<string, object>>();
            if (list != null) {
                foreach (var row in list) {
                    if (GetRecordFromDictionary(row) is Dictionary<string, object> d)
                       rows.Add(row);
                }
            }
            return rows;
        }
        #pragma warning restore 1998

        // Get the first record from datareader
        [return: NotNullIfNotNull("dr")]
        protected async Task<Dictionary<string, object>?> GetRecordFromRecordset(DbDataReader? dr)
        {
            if (dr != null) {
                await LoadRowValues(dr); // Set up DbValue/CurrentValue
                return GetRecordFromDictionary(GetDictionary(dr));
            }
            return null;
        }

        // Get the first record from the list of records
        protected Dictionary<string, object>? GetRecordFromRecordset(List<Dictionary<string, object>>? list) =>
            list != null && list.Count > 0 ? GetRecordFromDictionary(list[0]) : null;

        // Get record from Dictionary
        protected Dictionary<string, object>? GetRecordFromDictionary(Dictionary<string, object>? dict) {
            if (dict == null)
                return null;
            var row = new Dictionary<string, object>();
            foreach (var (key, value) in dict) {
                if (Fields.TryGetValue(key, out DbField? fld)) {
                    if (fld.Visible || fld.IsPrimaryKey) { // Primary key or Visible
                        if (fld.HtmlTag == "FILE") { // Upload field
                            if (Empty(value)) {
                                // row[key] = null;
                            } else {
                                if (fld.DataType == DataType.Blob) {
                                    string url = FullUrl(GetPageName(Config.ApiUrl) + "/" + Config.ApiFileAction + "/" + fld.TableVar + "/" + fld.Param + "/" + GetRecordKeyValue(dict)); // Query string format
                                    row[key] = new Dictionary<string, object> { { "type", ContentType((byte[])value) }, { "url", url }, { "name", fld.Param + ContentExtension((byte[])value) } };
                                } else if (!fld.UploadMultiple || !ConvertToString(value).Contains(Config.MultipleUploadSeparator)) { // Single file
                                    string url = FullUrl(GetPageName(Config.ApiUrl) + "/" + Config.ApiFileAction + "/" + fld.TableVar + "/" + Encrypt(fld.PhysicalUploadPath + ConvertToString(value))); // Query string format
                                    row[key] = new Dictionary<string, object> { { "type", ContentType(ConvertToString(value)) }, { "url", url }, { "name", ConvertToString(value) } };
                                } else { // Multiple files
                                    var files = ConvertToString(value).Split(Config.MultipleUploadSeparator);
                                    row[key] = files.Where(file => !Empty(file)).Select(file => new Dictionary<string, object> { { "type", ContentType(file) }, { "url", FullUrl(GetPageName(Config.ApiUrl) + "/" + Config.ApiFileAction + "/" + fld.TableVar + "/" + Encrypt(fld.PhysicalUploadPath + file)) }, { "name", file } });
                                }
                            }
                        } else {
                            string val = ConvertToString(value);
                            if (fld.DataType == DataType.Date && value is DateTime dt)
                                val = dt.ToString("s");
                            row[key] = ConvertToString(val);
                        }
                    }
                }
            }
            return row;
        }

        // Get record key value from array
        protected string GetRecordKeyValue(Dictionary<string, object> dict) {
            string key = "";
            key += UrlEncode(ConvertToString(dict.ContainsKey("id") ? dict["id"] : id.CurrentValue));
            return key;
        }

        // Hide fields for Add/Edit
        protected void HideFieldsForAddEdit() {
            if (IsAdd || IsCopy || IsGridAdd)
                id.Visible = false;
        }

        #pragma warning disable 219
        /// <summary>
        /// Lookup data from table
        /// </summary>
        public async Task<Dictionary<string, object>> Lookup(Dictionary<string, string>? dict = null)
        {
            Language = ResolveLanguage();
            Security = ResolveSecurity();
            string? v;

            // Get lookup object
            string fieldName = IsDictionary(dict) && dict.TryGetValue("field", out v) && v != null ? v : Post("field");
            var lookupField = FieldByName(fieldName);
            var lookup = lookupField?.Lookup;
            if (lookup == null) // DN
                return new Dictionary<string, object>();
            string lookupType = IsDictionary(dict) && dict.TryGetValue("ajax", out v) && v != null ? v : (Post("ajax") ?? "unknown");
            int pageSize = -1;
            int offset = -1;
            string searchValue = "";
            if (SameText(lookupType, "modal") || SameText(lookupType, "filter")) {
                searchValue = IsDictionary(dict) && (dict.TryGetValue("q", out v) && v != null || dict.TryGetValue("sv", out v) && v != null)
                    ? v
                    : (Param("q") ?? Post("sv"));
                pageSize = IsDictionary(dict) && (dict.TryGetValue("n", out v) || dict.TryGetValue("recperpage", out v))
                    ? ConvertToInt(v)
                    : (IsNumeric(Param("n")) ? Param<int>("n") : (Post("recperpage", out StringValues rpp) ? ConvertToInt(rpp.ToString()) : 10));
            } else if (SameText(lookupType, "autosuggest")) {
                searchValue = IsDictionary(dict) && dict.TryGetValue("q", out v) && v != null ? v : Param("q");
                pageSize = IsDictionary(dict) && dict.TryGetValue("n", out v) ? ConvertToInt(v) : (IsNumeric(Param("n")) ? Param<int>("n") : -1);
                if (pageSize <= 0)
                    pageSize = Config.AutoSuggestMaxEntries;
            }
            int start = IsDictionary(dict) && dict.TryGetValue("start", out v) ? ConvertToInt(v) : (IsNumeric(Param("start")) ? Param<int>("start") : -1);
            int page = IsDictionary(dict) && dict.TryGetValue("page", out v) ? ConvertToInt(v) : (IsNumeric(Param("page")) ? Param<int>("page") : -1);
            offset = start >= 0 ? start : (page > 0 && pageSize > 0 ? (page - 1) * pageSize : 0);
            string userSelect = Decrypt(IsDictionary(dict) && dict.TryGetValue("s", out v) && v != null ? v : Post("s"));
            string userFilter = Decrypt(IsDictionary(dict) && dict.TryGetValue("f", out v) && v != null ? v : Post("f"));
            string userOrderBy = Decrypt(IsDictionary(dict) && dict.TryGetValue("o", out v) && v != null ? v : Post("o"));

            // Selected records from modal, skip parent/filter fields and show all records
            lookup.LookupType = lookupType; // Lookup type
            lookup.FilterValues.Clear(); // Clear filter values first
            StringValues keys = IsDictionary(dict) && dict.TryGetValue("keys", out v) && !Empty(v)
                ? (StringValues)v
                : (Post("keys[]", out StringValues k) ? (StringValues)k : StringValues.Empty);
            if (!Empty(keys)) { // Selected records from modal
                lookup.FilterFields = new (); // Skip parent fields if any
                pageSize = -1; // Show all records
                lookup.FilterValues.Add(String.Join(",", keys.ToArray()));
            } else { // Lookup values
                string lookupValue = IsDictionary(dict) && (dict.TryGetValue("v0", out v) && v != null || dict.TryGetValue("lookupValue", out v) && v != null)
                    ? v
                    : (Post<string>("v0") ?? Post("lookupValue"));
                lookup.FilterValues.Add(lookupValue);
            }
            int cnt = IsDictionary(lookup.FilterFields) ? lookup.FilterFields.Count : 0;
            for (int i = 1; i <= cnt; i++) {
                var val = UrlDecode(IsDictionary(dict) && dict.TryGetValue("v" + i, out v) ? v : Post("v" + i));
                if (val != null) // DN
                    lookup.FilterValues.Add(val);
            }
            lookup.SearchValue = searchValue;
            lookup.PageSize = pageSize;
            lookup.Offset = offset;
            if (userSelect != "")
                lookup.UserSelect = userSelect;
            if (userFilter != "")
                lookup.UserFilter = userFilter;
            if (userOrderBy != "")
                lookup.UserOrderBy = userOrderBy;
            return await lookup.ToJson(this);
        }
        #pragma warning restore 219

        // Properties
        public string FormClassName = "ew-form ew-add-form";

        public bool IsModal = false;

        public bool IsMobileOrModal = false;

        public string DbMasterFilter = "";

        public string DbDetailFilter = "";

        public int StartRecord;

        public DbDataReader? Recordset = null; // Reserved // DN

        public bool CopyRecord;

        /// <summary>
        /// Page run
        /// </summary>
        /// <returns>Page result</returns>
        public override async Task<IActionResult> Run()
        {
            // Is modal
            IsModal = Param<bool>("modal");
            UseLayout = UseLayout && !IsModal;

            // Use layout
            if (!Empty(Param("layout")) && !Param<bool>("layout"))
                UseLayout = false;

            // User profile
            Profile = ResolveProfile();

            // Security
            Security = ResolveSecurity();
            if (TableVar != "")
                Security.LoadTablePermissions(TableVar);

            // Create form object
            CurrentForm ??= new ();
            await CurrentForm.Init();
            CurrentAction = Param("action"); // Set up current action
            SetVisibility();

            // Do not use lookup cache
            if (!Config.LookupCachePageIds.Contains(PageID))
                SetUseLookupCache(false);

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

            // Hide fields for add/edit
            if (!UseAjaxActions)
                HideFieldsForAddEdit();
            // Use inline delete
            if (UseAjaxActions)
                InlineDelete = true;

            // Set up lookup cache
            await SetupLookupOptions(role_id);

            // Load default values for add
            LoadDefaultValues();

            // Check modal
            if (IsModal)
                SkipHeaderFooter = true;
            IsMobileOrModal = IsMobile() || IsModal;
            bool postBack = false;
            StringValues sv;

            // Set up current action
            if (IsApi()) {
                CurrentAction = "insert"; // Add record directly
                postBack = true;
            } else if (!Empty(Post("action"))) {
                CurrentAction = Post("action"); // Get form action
                if (Post(OldKeyName, out sv))
                    SetKey(sv.ToString());
                postBack = true;
            } else {
                // Load key from QueryString
                string[] keyValues = {};
                object? rv;
                if (RouteValues.TryGetValue("key", out object? k))
                    keyValues = ConvertToString(k).Split('/'); // For Copy page
                if (RouteValues.TryGetValue("id", out rv)) { // DN
                    id.QueryValue = UrlDecode(rv); // DN
                } else if (Get("id", out sv)) {
                    id.QueryValue = sv.ToString();
                }
                OldKey = GetKey(true); // Get from CurrentValue
                CopyRecord = !Empty(OldKey);
                if (CopyRecord) {
                    CurrentAction = "copy"; // Copy record
                    SetKey(OldKey); // Set up record key
                } else {
                    CurrentAction = "show"; // Display blank record
                }
            }

            // Load old record / default values
            var rsold = await LoadOldRecord();

            // Load form values
            if (postBack) {
                await LoadFormValues(); // Load form values
            }

            // Validate form if post back
            if (postBack) {
                if (!await ValidateForm()) {
                    EventCancelled = true; // Event cancelled
                    RestoreFormValues(); // Restore form values
                    if (IsApi())
                        return Terminate();
                    else
                        CurrentAction = "show"; // Form error, reset action
                }
            }

            // Perform current action
            switch (CurrentAction) {
                case "copy": // Copy an existing record
                    if (rsold == null) { // Record not loaded
                        if (Empty(FailureMessage))
                            FailureMessage = Language.Phrase("NoRecord"); // No record found
                        return Terminate("userslist"); // No matching record, return to List page // DN
                    }
                    break;
                case "insert": // Add new record // DN
                    SendEmail = true; // Send email on add success
                    var res = await AddRow(rsold);
                    if (res) { // Add successful
                        if (Empty(SuccessMessage) && Post("addopt") != "1") // Skip success message for addopt (done in JavaScript)
                            SuccessMessage = Language.Phrase("AddSuccess"); // Set up success message
                        string returnUrl = "";
                        returnUrl = ReturnUrl;
                        if (GetPageName(returnUrl) == "userslist")
                            returnUrl = AddMasterUrl(ListUrl); // List page, return to List page with correct master key if necessary
                        else if (GetPageName(returnUrl) == "usersview")
                            returnUrl = ViewUrl; // View page, return to View page with key URL directly

                        // Handle UseAjaxActions
                        if (IsModal && UseAjaxActions) {
                            IsModal = false;
                            if (GetPageName(returnUrl) != "userslist") {
                                TempData["Return-Url"] = returnUrl; // Save return URL
                                returnUrl = "userslist"; // Return list page content
                            }
                        }
                        if (IsJsonResponse()) { // Return to caller
                            ClearMessages(); // Clear messages for Json response
                            return res;
                        } else {
                            return Terminate(returnUrl);
                        }
                    } else if (IsApi()) { // API request, return
                        return Terminate();
                    } else {
                        EventCancelled = true; // Event cancelled
                        RestoreFormValues(); // Add failed, restore form values
                    }
                    break;
            }

            // Set up Breadcrumb
            SetupBreadcrumb();

            // Render row based on row type
            RowType = RowType.Add; // Render add type

            // Render row
            ResetAttributes();
            await RenderRow();

            // Set LoginStatus, Page Rendering and Page Render
            if (!IsApi() && !IsTerminated) {
                SetupLoginStatus(); // Setup login status

                // Pass login status to client side
                SetClientVar("login", LoginStatus);

                // Global Page Rendering event
                PageRendering();

                // Page Render event
                usersAdd?.PageRender();
            }
            return PageResult();
        }

        // Confirm page
        public bool ConfirmPage = false; // DN

        #pragma warning disable 1998
        // Get upload files
        public async Task GetUploadFiles()
        {
            // Get upload data
        }
        #pragma warning restore 1998

        // Load default values
        protected void LoadDefaultValues() {
            role_id.DefaultValue = role_id.GetDefault(); // DN
            role_id.OldValue = role_id.DefaultValue;
        }

        #pragma warning disable 1998
        // Load form values
        protected async Task LoadFormValues() {
            if (CurrentForm == null)
                return;
            bool validate = !Config.ServerValidate;
            string val;

            // Check field name 'first_name' before field var 'x_first_name'
            val = CurrentForm.HasValue("first_name") ? CurrentForm.GetValue("first_name") : CurrentForm.GetValue("x_first_name");
            if (!first_name.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("first_name") && !CurrentForm.HasValue("x_first_name")) // DN
                    first_name.Visible = false; // Disable update for API request
                else
                    first_name.SetFormValue(val);
            }

            // Check field name 'last_name' before field var 'x_last_name'
            val = CurrentForm.HasValue("last_name") ? CurrentForm.GetValue("last_name") : CurrentForm.GetValue("x_last_name");
            if (!last_name.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("last_name") && !CurrentForm.HasValue("x_last_name")) // DN
                    last_name.Visible = false; // Disable update for API request
                else
                    last_name.SetFormValue(val);
            }

            // Check field name 'email' before field var 'x__email'
            val = CurrentForm.HasValue("email") ? CurrentForm.GetValue("email") : CurrentForm.GetValue("x__email");
            if (!_email.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("email") && !CurrentForm.HasValue("x__email")) // DN
                    _email.Visible = false; // Disable update for API request
                else
                    _email.SetFormValue(val, true, validate);
            }

            // Check field name 'username' before field var 'x__username'
            val = CurrentForm.HasValue("username") ? CurrentForm.GetValue("username") : CurrentForm.GetValue("x__username");
            if (!_username.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("username") && !CurrentForm.HasValue("x__username")) // DN
                    _username.Visible = false; // Disable update for API request
                else
                    _username.SetFormValue(val);
            }

            // Check field name 'password' before field var 'x_password'
            val = CurrentForm.HasValue("password") ? CurrentForm.GetValue("password") : CurrentForm.GetValue("x_password");
            if (!password.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("password") && !CurrentForm.HasValue("x_password")) // DN
                    password.Visible = false; // Disable update for API request
                else
                    password.SetFormValue(val);
            }

            // Check field name 'mobile' before field var 'x_mobile'
            val = CurrentForm.HasValue("mobile") ? CurrentForm.GetValue("mobile") : CurrentForm.GetValue("x_mobile");
            if (!mobile.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("mobile") && !CurrentForm.HasValue("x_mobile")) // DN
                    mobile.Visible = false; // Disable update for API request
                else
                    mobile.SetFormValue(val, true, validate);
            }

            // Check field name 'role_id' before field var 'x_role_id'
            val = CurrentForm.HasValue("role_id") ? CurrentForm.GetValue("role_id") : CurrentForm.GetValue("x_role_id");
            if (!role_id.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("role_id") && !CurrentForm.HasValue("x_role_id")) // DN
                    role_id.Visible = false; // Disable update for API request
                else
                    role_id.SetFormValue(val);
            }

            // Check field name 'created_at' before field var 'x_created_at'
            val = CurrentForm.HasValue("created_at") ? CurrentForm.GetValue("created_at") : CurrentForm.GetValue("x_created_at");
            if (!created_at.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("created_at") && !CurrentForm.HasValue("x_created_at")) // DN
                    created_at.Visible = false; // Disable update for API request
                else
                    created_at.SetFormValue(val);
                created_at.CurrentValue = UnformatDateTime(created_at.CurrentValue, created_at.FormatPattern);
            }

            // Check field name 'updated_at' before field var 'x_updated_at'
            val = CurrentForm.HasValue("updated_at") ? CurrentForm.GetValue("updated_at") : CurrentForm.GetValue("x_updated_at");
            if (!updated_at.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("updated_at") && !CurrentForm.HasValue("x_updated_at")) // DN
                    updated_at.Visible = false; // Disable update for API request
                else
                    updated_at.SetFormValue(val);
                updated_at.CurrentValue = UnformatDateTime(updated_at.CurrentValue, updated_at.FormatPattern);
            }

            // Check field name 'id' before field var 'x_id'
            val = CurrentForm.HasValue("id") ? CurrentForm.GetValue("id") : CurrentForm.GetValue("x_id");
        }
        #pragma warning restore 1998

        // Restore form values
        public void RestoreFormValues()
        {
            first_name.CurrentValue = first_name.FormValue;
            last_name.CurrentValue = last_name.FormValue;
            _email.CurrentValue = _email.FormValue;
            _username.CurrentValue = _username.FormValue;
            password.CurrentValue = password.FormValue;
            mobile.CurrentValue = mobile.FormValue;
            role_id.CurrentValue = role_id.FormValue;
            created_at.CurrentValue = created_at.FormValue;
            created_at.CurrentValue = UnformatDateTime(created_at.CurrentValue, created_at.FormatPattern);
            updated_at.CurrentValue = updated_at.FormValue;
            updated_at.CurrentValue = UnformatDateTime(updated_at.CurrentValue, updated_at.FormatPattern);
        }

        // Load row based on key values
        public async Task<bool> LoadRow()
        {
            string filter = GetRecordFilter();

            // Call Row Selecting event
            RowSelecting(ref filter);

            // Load SQL based on filter
            CurrentFilter = filter;
            string sql = CurrentSql;
            bool res = false;
            try {
                var row = await Connection.GetRowAsync(sql);
                if (row != null) {
                    await LoadRowValues(row);
                    res = true;
                } else {
                    return false;
                }
            } catch {
                if (Config.Debug)
                    throw;
            }

            // Check if valid User ID
            if (res) {
                res = ShowOptionLink("add");
                if (!res)
                    FailureMessage = DeniedMessage();
            }
            return res;
        }

        #pragma warning disable 162, 168, 1998, 4014
        // Load row values from data reader
        public async Task LoadRowValues(DbDataReader? dr = null) => await LoadRowValues(GetDictionary(dr));

        // Load row values from recordset
        public async Task LoadRowValues(Dictionary<string, object>? row)
        {
            row ??= NewRow();

            // Call Row Selected event
            RowSelected(row);
            id.SetDbValue(row["id"]);
            first_name.SetDbValue(row["first_name"]);
            last_name.SetDbValue(row["last_name"]);
            _email.SetDbValue(row["email"]);
            _username.SetDbValue(row["username"]);
            password.SetDbValue(row["password"]);
            mobile.SetDbValue(row["mobile"]);
            photo.Upload.DbValue = row["photo"];
            photo.SetDbValue(photo.Upload.DbValue);
            role_id.SetDbValue(row["role_id"]);
            provider.SetDbValue(row["provider"]);
            activated.SetDbValue(row["activated"]);
            _profile.SetDbValue(row["profile"]);
            created_at.SetDbValue(row["created_at"]);
            updated_at.SetDbValue(row["updated_at"]);
        }
        #pragma warning restore 162, 168, 1998, 4014

        // Return a row with default values
        protected Dictionary<string, object> NewRow() {
            var row = new Dictionary<string, object>();
            row.Add("id", id.DefaultValue ?? DbNullValue); // DN
            row.Add("first_name", first_name.DefaultValue ?? DbNullValue); // DN
            row.Add("last_name", last_name.DefaultValue ?? DbNullValue); // DN
            row.Add("email", _email.DefaultValue ?? DbNullValue); // DN
            row.Add("username", _username.DefaultValue ?? DbNullValue); // DN
            row.Add("password", password.DefaultValue ?? DbNullValue); // DN
            row.Add("mobile", mobile.DefaultValue ?? DbNullValue); // DN
            row.Add("photo", photo.DefaultValue ?? DbNullValue); // DN
            row.Add("role_id", role_id.DefaultValue ?? DbNullValue); // DN
            row.Add("provider", provider.DefaultValue ?? DbNullValue); // DN
            row.Add("activated", activated.DefaultValue ?? DbNullValue); // DN
            row.Add("profile", _profile.DefaultValue ?? DbNullValue); // DN
            row.Add("created_at", created_at.DefaultValue ?? DbNullValue); // DN
            row.Add("updated_at", updated_at.DefaultValue ?? DbNullValue); // DN
            return row;
        }

        #pragma warning disable 618, 1998
        // Load old record
        protected async Task<Dictionary<string, object>?> LoadOldRecord(DatabaseConnectionBase<SqlConnection, SqlCommand, SqlDataReader, SqlDbType>? cnn = null) {
            // Load old record
            Dictionary<string, object>? row = null;
            bool validKey = !Empty(OldKey);
            if (validKey) {
                SetKey(OldKey);
                CurrentFilter = GetRecordFilter();
                string sql = CurrentSql;
                try {
                    row = await (cnn ?? Connection).GetRowAsync(sql);
                } catch {
                    row = null;
                }
            }
            await LoadRowValues(row); // Load row values
            return row;
        }
        #pragma warning restore 618, 1998

        #pragma warning disable 1998
        // Render row values based on field settings
        public async Task RenderRow()
        {
            // Call Row Rendering event
            RowRendering();

            // Common render codes for all row types

            // id
            id.RowCssClass = "row";

            // first_name
            first_name.RowCssClass = "row";

            // last_name
            last_name.RowCssClass = "row";

            // email
            _email.RowCssClass = "row";

            // username
            _username.RowCssClass = "row";

            // password
            password.RowCssClass = "row";

            // mobile
            mobile.RowCssClass = "row";

            // photo
            photo.RowCssClass = "row";

            // role_id
            role_id.RowCssClass = "row";

            // provider
            provider.RowCssClass = "row";

            // activated
            activated.RowCssClass = "row";

            // profile
            _profile.RowCssClass = "row";

            // created_at
            created_at.RowCssClass = "row";

            // updated_at
            updated_at.RowCssClass = "row";

            // View row
            if (RowType == RowType.View) {
                // id
                id.ViewValue = id.CurrentValue;
                id.ViewValue = FormatNumber(id.ViewValue, id.FormatPattern);
                id.ViewCustomAttributes = "";

                // first_name
                first_name.ViewValue = ConvertToString(first_name.CurrentValue); // DN
                first_name.ViewCustomAttributes = "";

                // last_name
                last_name.ViewValue = ConvertToString(last_name.CurrentValue); // DN
                last_name.ViewCustomAttributes = "";

                // email
                _email.ViewValue = ConvertToString(_email.CurrentValue); // DN
                _email.ViewCustomAttributes = "";

                // username
                _username.ViewValue = ConvertToString(_username.CurrentValue); // DN
                _username.ViewCustomAttributes = "";

                // password
                password.ViewValue = Language.Phrase("PasswordMask");
                password.ViewCustomAttributes = "";

                // mobile
                mobile.ViewValue = ConvertToString(mobile.CurrentValue); // DN
                mobile.ViewCustomAttributes = "";

                // photo
                if (!IsNull(photo.Upload.DbValue)) {
                    photo.ImageWidth = 80;
                    photo.ImageHeight = 0;
                    photo.ImageAlt = photo.Alt;
                    photo.ImageCssClass = "ew-image";
                    photo.ViewValue = photo.Upload.DbValue;
                } else {
                    photo.ViewValue = "";
                }
                photo.ViewCustomAttributes = "";

                // role_id
                if (Security.CanAdmin) { // System admin
                    curVal = ConvertToString(role_id.CurrentValue);
                    if (!Empty(curVal)) {
                        if (role_id.Lookup != null && IsDictionary(role_id.Lookup?.Options) && role_id.Lookup?.Options.Values.Count > 0) { // Load from cache // DN
                            role_id.ViewValue = role_id.LookupCacheOption(curVal);
                        } else { // Lookup from database // DN
                            filterWrk = SearchFilter("[id]", "=", role_id.CurrentValue, DataType.Number, "");
                            sqlWrk = role_id.Lookup?.GetSql(false, filterWrk, null, this, true, true);
                            rswrk = sqlWrk != null ? Connection.GetRows(sqlWrk) : null; // Must use Sync to avoid overwriting ViewValue in RenderViewRow
                            if (rswrk?.Count > 0 && role_id.Lookup != null) { // Lookup values found
                                var listwrk = role_id.Lookup?.RenderViewRow(rswrk[0]);
                                role_id.ViewValue = role_id.HighlightLookup(ConvertToString(rswrk[0]), role_id.DisplayValue(listwrk));
                            } else {
                                role_id.ViewValue = FormatNumber(role_id.CurrentValue, role_id.FormatPattern);
                            }
                        }
                    } else {
                        role_id.ViewValue = DbNullValue;
                    }
                } else {
                    role_id.ViewValue = Language.Phrase("PasswordMask");
                }
                role_id.ViewCustomAttributes = "";

                // provider
                provider.ViewValue = provider.CurrentValue;
                provider.ViewCustomAttributes = "";

                // activated
                activated.ViewValue = activated.CurrentValue;
                activated.ViewCustomAttributes = "";

                // created_at
                created_at.ViewValue = created_at.CurrentValue;
                created_at.ViewValue = FormatDateTime(created_at.ViewValue, created_at.FormatPattern);
                created_at.ViewCustomAttributes = "";

                // updated_at
                updated_at.ViewValue = updated_at.CurrentValue;
                updated_at.ViewValue = FormatDateTime(updated_at.ViewValue, updated_at.FormatPattern);
                updated_at.ViewCustomAttributes = "";

                // first_name
                first_name.HrefValue = "";

                // last_name
                last_name.HrefValue = "";

                // email
                _email.HrefValue = "";

                // username
                _username.HrefValue = "";

                // password
                password.HrefValue = "";

                // mobile
                mobile.HrefValue = "";

                // role_id
                role_id.HrefValue = "";

                // created_at
                created_at.HrefValue = "";
                created_at.TooltipValue = "";

                // updated_at
                updated_at.HrefValue = "";
            } else if (RowType == RowType.Add) {
                // first_name
                first_name.SetupEditAttributes();
                if (!first_name.Raw)
                    first_name.CurrentValue = HtmlDecode(first_name.CurrentValue);
                first_name.EditValue = HtmlEncode(first_name.CurrentValue);
                first_name.PlaceHolder = RemoveHtml(first_name.Caption);

                // last_name
                last_name.SetupEditAttributes();
                if (!last_name.Raw)
                    last_name.CurrentValue = HtmlDecode(last_name.CurrentValue);
                last_name.EditValue = HtmlEncode(last_name.CurrentValue);
                last_name.PlaceHolder = RemoveHtml(last_name.Caption);

                // email
                _email.SetupEditAttributes();
                if (!_email.Raw)
                    _email.CurrentValue = HtmlDecode(_email.CurrentValue);
                _email.EditValue = HtmlEncode(_email.CurrentValue);
                _email.PlaceHolder = RemoveHtml(_email.Caption);

                // username
                _username.SetupEditAttributes();
                if (!_username.Raw)
                    _username.CurrentValue = HtmlDecode(_username.CurrentValue);
                _username.EditValue = HtmlEncode(_username.CurrentValue);
                _username.PlaceHolder = RemoveHtml(_username.Caption);

                // password
                password.SetupEditAttributes(new () { { "class", "ew-password-strength" } } );
                password.EditValue = Language.Phrase("PasswordMask"); // Show as masked password
                password.PlaceHolder = RemoveHtml(password.Caption);

                // mobile
                mobile.SetupEditAttributes();
                if (!mobile.Raw)
                    mobile.CurrentValue = HtmlDecode(mobile.CurrentValue);
                mobile.EditValue = HtmlEncode(mobile.CurrentValue);
                mobile.PlaceHolder = RemoveHtml(mobile.Caption);

                // role_id
                role_id.SetupEditAttributes();
                if (!Security.CanAdmin) { // System admin
                    role_id.EditValue = Language.Phrase("PasswordMask");
                } else {
                    curVal = ConvertToString(role_id.CurrentValue)?.Trim() ?? "";
                    if (role_id.Lookup != null && IsDictionary(role_id.Lookup?.Options) && role_id.Lookup?.Options.Values.Count > 0) { // Load from cache // DN
                        role_id.EditValue = role_id.Lookup?.Options.Values.ToList();
                    } else { // Lookup from database
                        if (curVal == "") {
                            filterWrk = "0=1";
                        } else {
                            filterWrk = SearchFilter("[id]", "=", role_id.CurrentValue, DataType.Number, "");
                        }
                        sqlWrk = role_id.Lookup?.GetSql(true, filterWrk, null, this, false, true);
                        rswrk = sqlWrk != null ? Connection.GetRows(sqlWrk) : null; // Must use Sync to avoid overwriting ViewValue in RenderViewRow
                        role_id.EditValue = rswrk;
                    }
                    role_id.PlaceHolder = RemoveHtml(role_id.Caption);
                    if (!Empty(role_id.EditValue) && IsNumeric(role_id.EditValue))
                        role_id.EditValue = FormatNumber(role_id.EditValue, null);
                }

                // created_at

                // updated_at

                // Add refer script

                // first_name
                first_name.HrefValue = "";

                // last_name
                last_name.HrefValue = "";

                // email
                _email.HrefValue = "";

                // username
                _username.HrefValue = "";

                // password
                password.HrefValue = "";

                // mobile
                mobile.HrefValue = "";

                // role_id
                role_id.HrefValue = "";

                // created_at
                created_at.HrefValue = "";

                // updated_at
                updated_at.HrefValue = "";
            }
            if (RowType == RowType.Add || RowType == RowType.Edit || RowType == RowType.Search) // Add/Edit/Search row
                SetupFieldTitles();

            // Call Row Rendered event
            if (RowType != RowType.AggregateInit)
                RowRendered();
        }
        #pragma warning restore 1998

        #pragma warning disable 1998
        // Validate form
        protected async Task<bool> ValidateForm() {
            // Check if validation required
            if (!Config.ServerValidate)
                return true;
            bool validateForm = true;
            if (first_name.Required) {
                if (!first_name.IsDetailKey && Empty(first_name.FormValue)) {
                    first_name.AddErrorMessage(ConvertToString(first_name.RequiredErrorMessage).Replace("%s", first_name.Caption));
                }
            }
            if (last_name.Required) {
                if (!last_name.IsDetailKey && Empty(last_name.FormValue)) {
                    last_name.AddErrorMessage(ConvertToString(last_name.RequiredErrorMessage).Replace("%s", last_name.Caption));
                }
            }
            if (_email.Required) {
                if (!_email.IsDetailKey && Empty(_email.FormValue)) {
                    _email.AddErrorMessage(ConvertToString(_email.RequiredErrorMessage).Replace("%s", _email.Caption));
                }
            }
            if (!CheckEmail(_email.FormValue)) {
                _email.AddErrorMessage(_email.GetErrorMessage(false));
            }
            if (_username.Required) {
                if (!_username.IsDetailKey && Empty(_username.FormValue)) {
                    _username.AddErrorMessage(ConvertToString(_username.RequiredErrorMessage).Replace("%s", _username.Caption));
                }
            }
            if (!_username.Raw && Config.RemoveXss && CheckUsername(_username.FormValue)) {
                _username.AddErrorMessage(Language.Phrase("InvalidUsernameChars"));
            }
            if (password.Required) {
                if (!password.IsDetailKey && Empty(password.FormValue)) {
                    password.AddErrorMessage(ConvertToString(password.RequiredErrorMessage).Replace("%s", password.Caption));
                }
            }
            if (!password.Raw && Config.RemoveXss && CheckPassword(password.FormValue)) {
                password.AddErrorMessage(Language.Phrase("InvalidPasswordChars"));
            }
            if (mobile.Required) {
                if (!mobile.IsDetailKey && Empty(mobile.FormValue)) {
                    mobile.AddErrorMessage(ConvertToString(mobile.RequiredErrorMessage).Replace("%s", mobile.Caption));
                }
            }
            if (!CheckPhone(mobile.FormValue)) {
                mobile.AddErrorMessage(mobile.GetErrorMessage(false));
            }
            if (role_id.Required) {
                if (Security.CanAdmin && !role_id.IsDetailKey && Empty(role_id.FormValue)) {
                    role_id.AddErrorMessage(ConvertToString(role_id.RequiredErrorMessage).Replace("%s", role_id.Caption));
                }
            }
            if (created_at.Required) {
                if (!created_at.IsDetailKey && Empty(created_at.FormValue)) {
                    created_at.AddErrorMessage(ConvertToString(created_at.RequiredErrorMessage).Replace("%s", created_at.Caption));
                }
            }
            if (updated_at.Required) {
                if (!updated_at.IsDetailKey && Empty(updated_at.FormValue)) {
                    updated_at.AddErrorMessage(ConvertToString(updated_at.RequiredErrorMessage).Replace("%s", updated_at.Caption));
                }
            }

            // Return validate result
            validateForm = validateForm && !HasInvalidFields();

            // Call Form CustomValidate event
            string formCustomError = "";
            validateForm = validateForm && FormCustomValidate(ref formCustomError);
            if (!Empty(formCustomError))
                FailureMessage = formCustomError;
            return validateForm;
        }
        #pragma warning restore 1998

        // Add record
        #pragma warning disable 168, 219

        protected async Task<JsonBoolResult> AddRow(Dictionary<string, object>? rsold = null) { // DN
            bool result = false;

            // Set new row
            Dictionary<string, object> rsnew = new ();
            try {
                // first_name
                first_name.SetDbValue(rsnew, first_name.CurrentValue);

                // last_name
                last_name.SetDbValue(rsnew, last_name.CurrentValue);

                // email
                _email.SetDbValue(rsnew, _email.CurrentValue);

                // username
                _username.SetDbValue(rsnew, _username.CurrentValue);

                // password
                if (Config.EncryptedPassword && !IsMaskedPassword(password.CurrentValue)) {
                    password.CurrentValue = EncryptPassword(Config.CaseSensitivePassword ? ConvertToString(password.CurrentValue) : ConvertToString(password.CurrentValue).ToLower());
                }
                password.SetDbValue(rsnew, password.CurrentValue);

                // mobile
                mobile.SetDbValue(rsnew, mobile.CurrentValue);

                // role_id
                if (Security.CanAdmin) { // System admin
                role_id.SetDbValue(rsnew, role_id.CurrentValue, Empty(role_id.CurrentValue));
                }

                // created_at
                created_at.CurrentValue = created_at.GetAutoUpdateValue();
                created_at.SetDbValue(rsnew, ConvertToDateTime(created_at.CurrentValue, created_at.FormatPattern), false);

                // updated_at
                updated_at.CurrentValue = updated_at.GetAutoUpdateValue();
                updated_at.SetDbValue(rsnew, ConvertToDateTime(updated_at.CurrentValue, updated_at.FormatPattern), false);

                // id
            } catch (Exception e) {
                if (Config.Debug)
                    throw;
                FailureMessage = e.Message;
                return JsonBoolResult.FalseResult;
            }

            // Update current values
            SetCurrentValues(rsnew);

            // Check if valid User ID
            bool validUser = false;
            string userIdMsg;
            if (!Empty(Security.CurrentUserID) && !Security.IsAdmin) { // Non system admin
                validUser = Security.IsValidUserID(id.CurrentValue);
                if (!validUser) {
                    userIdMsg = Language.Phrase("UnAuthorizedUserID").Replace("%c", ConvertToString(Security.CurrentUserID));
                    userIdMsg = userIdMsg.Replace("%u", ConvertToString(id.CurrentValue));
                    FailureMessage = userIdMsg;
                    return JsonBoolResult.FalseResult;
                }
            }

            // Load db values from rsold
            LoadDbValues(rsold);

            // Call Row Inserting event
            bool insertRow = RowInserting(rsold, rsnew);
            if (insertRow) {
                try {
                    result = ConvertToBool(await InsertAsync(rsnew));
                    rsnew["id"] = id.CurrentValue!;
                } catch (Exception e) {
                    if (Config.Debug)
                        throw;
                    FailureMessage = e.Message;
                    result = false;
                }
            } else {
                if (SuccessMessage != "" || FailureMessage != "") {
                    // Use the message, do nothing
                } else if (CancelMessage != "") {
                    FailureMessage = CancelMessage;
                    CancelMessage = "";
                } else {
                    FailureMessage = Language.Phrase("InsertCancelled");
                }
                result = false;
            }

            // Call Row Inserted event
            if (result)
                RowInserted(rsold, rsnew);

            // Write JSON for API request
            Dictionary<string, object> d = new ();
            d.Add("success", result);
            if (IsJsonResponse() && result) {
                if (GetRecordFromDictionary(rsnew) is var row && row != null) {
                    string table = TableVar;
                    d.Add(table, row);
                }
                d.Add("action", Config.ApiAddAction);
                d.Add("version", Config.ProductVersion);
                return new JsonBoolResult(d, result);
            }
            return new JsonBoolResult(d, result);
        }

        // Show link optionally based on User ID
        protected bool ShowOptionLink(string pageId = "") { // DN
            if (Security.IsLoggedIn && !Security.IsAdmin && !UserIDAllow(pageId))
                return Security.IsValidUserID(id.CurrentValue);
            return true;
        }

        // Set up Breadcrumb
        protected void SetupBreadcrumb() {
            var breadcrumb = new Breadcrumb();
            string url = CurrentUrl();
            breadcrumb.Add("list", TableVar, AppPath(AddMasterUrl("userslist")), "", TableVar, true);
            string pageId = IsCopy ? "Copy" : "Add";
            breadcrumb.Add("add", pageId, url);
            CurrentBreadcrumb = breadcrumb;
        }

        // Setup lookup options
        public async Task SetupLookupOptions(DbField fld)
        {
            if (fld.Lookup == null)
                return;
            Func<string>? lookupFilter = null;
            dynamic conn = Connection;
            if (fld.Lookup.Options.Count is int c && c == 0) {
                // Always call to Lookup.GetSql so that user can setup Lookup.Options in Lookup Selecting server event
                var sql = fld.Lookup.GetSql(false, "", lookupFilter, this);

                // Set up lookup cache
                if (!fld.HasLookupOptions && fld.UseLookupCache && !Empty(sql) && fld.Lookup.ParentFields.Count == 0 && fld.Lookup.Options.Count == 0) {
                    int totalCnt = await TryGetRecordCountAsync(sql, conn);
                    if (totalCnt > fld.LookupCacheCount) // Total count > cache count, do not cache
                        return;
                    var dict = new Dictionary<string, Dictionary<string, object>>();
                    var values = new List<object>();
                    List<Dictionary<string, object>> rs = await conn.GetRowsAsync(sql);
                    if (rs != null) {
                        for (int i = 0; i < rs.Count; i++) {
                            var row = rs[i];
                            row = fld.Lookup?.RenderViewRow(row, Resolve(fld.Lookup.LinkTable));
                            string key = row?.Values.First()?.ToString() ?? String.Empty;
                            if (!dict.ContainsKey(key) && row != null)
                                dict.Add(key, row);
                        }
                    }
                    fld.Lookup?.SetOptions(dict);
                }
            }
        }

        // Close recordset
        public void CloseRecordset()
        {
            using (Recordset) {} // Dispose
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

        // Page Breaking event
        public void PageBreaking(ref bool brk, ref string content) {
            // Example:
            //	brk = false; // Skip page break, or
            //	content = "<div style=\"page-break-after:always;\">&nbsp;</div>"; // Modify page break content
        }

        // Form Custom Validate event
        public virtual bool FormCustomValidate(ref string customError) {
            //Return error message in customError
            return true;
        }
    } // End page class
} // End Partial class
