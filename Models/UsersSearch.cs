namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// usersSearch
    /// </summary>
    public static UsersSearch usersSearch
    {
        get => HttpData.Get<UsersSearch>("usersSearch")!;
        set => HttpData["usersSearch"] = value;
    }

    /// <summary>
    /// Page class for users
    /// </summary>
    public class UsersSearch : UsersSearchBase
    {
        // Constructor
        public UsersSearch(Controller controller) : base(controller)
        {
        }

        // Constructor
        public UsersSearch() : base()
        {
        }
    }

    /// <summary>
    /// Page base class
    /// </summary>
    public class UsersSearchBase : Users
    {
        // Page ID
        public string PageID = "search";

        // Project ID
        public string ProjectID = "{89456192-A767-4B60-B043-591A4AA6A5D7}";

        // Table name
        public string TableName { get; set; } = "users";

        // Page object name
        public string PageObjName = "usersSearch";

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
        public UsersSearchBase()
        {
            // Initialize
            CurrentPage = this;

            // Table CSS class
            TableClass = "table table-striped table-bordered table-hover table-sm ew-desktop-table ew-search-table";

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
        public string PageName => "userssearch";

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
            id.SetVisibility();
            first_name.SetVisibility();
            last_name.SetVisibility();
            _email.SetVisibility();
            _username.SetVisibility();
            password.Visible = false;
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
        public UsersSearchBase(Controller? controller = null): this() { // DN
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

        public string FormClassName = "ew-form ew-search-form";

        public bool IsModal = false;

        public bool IsMobileOrModal = false;

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

            // Set up Breadcrumb
            SetupBreadcrumb();

            // Check modal
            if (IsModal)
                SkipHeaderFooter = true;
            IsMobileOrModal = IsMobile() || IsModal;

            // Get action
            CurrentAction = CurrentForm.GetValue("action");
            if (IsSearch) {
                // Build search string for advanced search, remove blank field
                LoadSearchValues(); // Get search values
                string srchStr = ValidateSearch() ? BuildAdvancedSearch() : "";
                if (!Empty(srchStr)) {
                    srchStr = "userslist?" + srchStr;
                    // Do not return Json for UseAjaxActions
                    if (IsModal && UseAjaxActions)
                        IsModal = false;
                    return Terminate(srchStr); // Go to List page
                }
            }

            // Restore search settings from Session
            if (!HasInvalidFields())
                LoadAdvancedSearch();

            // Render row for search
            RowType = RowType.Search;
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
                usersSearch?.PageRender();
            }
            return PageResult();
        }

        // Build advanced search
        protected string BuildAdvancedSearch() {
            string srchUrl = "";
            BuildSearchUrl(ref srchUrl, id); // id
            BuildSearchUrl(ref srchUrl, first_name); // first_name
            BuildSearchUrl(ref srchUrl, last_name); // last_name
            BuildSearchUrl(ref srchUrl, _email); // email
            BuildSearchUrl(ref srchUrl, _username); // username
            BuildSearchUrl(ref srchUrl, mobile); // mobile
            BuildSearchUrl(ref srchUrl, role_id); // role_id
            BuildSearchUrl(ref srchUrl, created_at); // created_at
            BuildSearchUrl(ref srchUrl, updated_at); // updated_at
            if (!Empty(srchUrl))
                srchUrl += "&";
            srchUrl += "cmd=search";
            return srchUrl;
        }

        // Build search URL
        protected void BuildSearchUrl(ref string url, DbField fld, bool oprOnly = false) {
            bool isValid;
            string wrk = "";
            string fldParm = fld.Param;
            string ctl = "x_" + fldParm;
            string ctl2 = "y_" + fldParm;
            if (fld.IsMultiSelect) { // DN
                ctl += "[]";
                ctl2 += "[]";
            }
            string fldVal = CurrentForm.GetValue(ctl);
            string fldOpr = CurrentForm.GetValue("z_" + fldParm);
            string fldCond = CurrentForm.GetValue("v_" + fldParm);
            string fldVal2 = CurrentForm.GetValue(ctl2);
            string fldOpr2 = CurrentForm.GetValue("w_" + fldParm);
            DataType fldDataType = fld.IsVirtual ? DataType.String : fld.DataType;
            fldVal = ConvertSearchValue(fldVal, fldOpr, fld); // For testing if numeric only
            fldVal2 = ConvertSearchValue(fldVal2, fldOpr2, fld); // For testing if numeric only
            fldOpr = ConvertSearchOperator(fldOpr, fld, fldVal);
            fldOpr2 = ConvertSearchOperator(fldOpr2, fld, fldVal2);
            if ((new [] { "BETWEEN", "NOT BETWEEN" }).Contains(fldOpr)) {
                isValid = fldDataType != DataType.Number || fld.VirtualSearch || IsNumericSearchValue(fldVal, fldOpr, fld) && IsNumericSearchValue(fldVal2, fldOpr2, fld);
                if (!Empty(fldVal) && !Empty(fldVal2) && isValid)
                    wrk = ctl + "=" + UrlEncode(fldVal) + "&" + ctl2 + "=" + UrlEncode(fldVal2) + "&z_" + fldParm + "=" + UrlEncode(fldOpr);
            } else {
                isValid = fldDataType != DataType.Number || fld.VirtualSearch || IsNumericSearchValue(fldVal, fldOpr, fld);
                if (!Empty(fldVal) && isValid && IsValidOperator(fldOpr)) {
                    wrk = ctl + "=" + UrlEncode(fldVal) + "&z_" + fldParm + "=" + UrlEncode(fldOpr);
                } else if ((new [] { "IS NULL", "IS NOT NULL", "IS EMPTY", "IS NOT EMPTY" }).Contains(fldOpr) || !Empty(fldOpr) && oprOnly && IsValidOperator(fldOpr)) {
                    wrk = "z_" + fldParm + "=" + UrlEncode(fldOpr);
                }
                isValid = fldDataType != DataType.Number || fld.VirtualSearch || IsNumericSearchValue(fldVal2, fldOpr2, fld);
                if (!Empty(fldVal2) && isValid && IsValidOperator(fldOpr2)) {
                    if (!Empty(wrk))
                        wrk += "&v_" + fldParm + "=" + fldCond + "&";
                    wrk += ctl2 + "=" + UrlEncode(fldVal2) + "&w_" + fldParm + "=" + UrlEncode(fldOpr2);
                } else if ((new [] { "IS NULL", "IS NOT NULL", "IS EMPTY", "IS NOT EMPTY" }).Contains(fldOpr2) || !Empty(fldOpr2) && oprOnly && IsValidOperator(fldOpr2)) {
                    if (!Empty(wrk))
                        wrk += "&v_" + fldParm + "=" + UrlEncode(fldCond) + "&";
                    wrk += "w_" + fldParm + "=" + UrlEncode(fldOpr2);
                }
            }
            if (!Empty(wrk)) {
                if (!Empty(url))
                    url += "&";
                url += wrk;
            }
        }

        // Load search values for validation // DN
        protected void LoadSearchValues() {
            // id
            if (!IsAddOrEdit)
                if (Form.ContainsKey("x_id"))
                    id.AdvancedSearch.SearchValue = CurrentForm.GetValue("x_id");
            if (Form.ContainsKey("z_id"))
                id.AdvancedSearch.SearchOperator = CurrentForm.GetValue("z_id");

            // first_name
            if (!IsAddOrEdit)
                if (Form.ContainsKey("x_first_name"))
                    first_name.AdvancedSearch.SearchValue = CurrentForm.GetValue("x_first_name");
            if (Form.ContainsKey("z_first_name"))
                first_name.AdvancedSearch.SearchOperator = CurrentForm.GetValue("z_first_name");

            // last_name
            if (!IsAddOrEdit)
                if (Form.ContainsKey("x_last_name"))
                    last_name.AdvancedSearch.SearchValue = CurrentForm.GetValue("x_last_name");
            if (Form.ContainsKey("z_last_name"))
                last_name.AdvancedSearch.SearchOperator = CurrentForm.GetValue("z_last_name");

            // _email
            if (!IsAddOrEdit)
                if (Form.ContainsKey("x__email"))
                    _email.AdvancedSearch.SearchValue = CurrentForm.GetValue("x__email");
            if (Form.ContainsKey("z__email"))
                _email.AdvancedSearch.SearchOperator = CurrentForm.GetValue("z__email");

            // _username
            if (!IsAddOrEdit)
                if (Form.ContainsKey("x__username"))
                    _username.AdvancedSearch.SearchValue = CurrentForm.GetValue("x__username");
            if (Form.ContainsKey("z__username"))
                _username.AdvancedSearch.SearchOperator = CurrentForm.GetValue("z__username");

            // mobile
            if (!IsAddOrEdit)
                if (Form.ContainsKey("x_mobile"))
                    mobile.AdvancedSearch.SearchValue = CurrentForm.GetValue("x_mobile");
            if (Form.ContainsKey("z_mobile"))
                mobile.AdvancedSearch.SearchOperator = CurrentForm.GetValue("z_mobile");

            // role_id
            if (!IsAddOrEdit)
                if (Form.ContainsKey("x_role_id"))
                    role_id.AdvancedSearch.SearchValue = CurrentForm.GetValue("x_role_id");
            if (Form.ContainsKey("z_role_id"))
                role_id.AdvancedSearch.SearchOperator = CurrentForm.GetValue("z_role_id");

            // created_at
            if (!IsAddOrEdit)
                if (Form.ContainsKey("x_created_at"))
                    created_at.AdvancedSearch.SearchValue = CurrentForm.GetValue("x_created_at");
            if (Form.ContainsKey("z_created_at"))
                created_at.AdvancedSearch.SearchOperator = CurrentForm.GetValue("z_created_at");
            if (Form.ContainsKey("v_created_at"))
                created_at.AdvancedSearch.SearchCondition = CurrentForm.GetValue("v_created_at");
            if (Form.ContainsKey("y_created_at"))
                created_at.AdvancedSearch.SearchValue2 = CurrentForm.GetValue("y_created_at");
            if (Form.ContainsKey("w_created_at"))
                created_at.AdvancedSearch.SearchOperator2 = CurrentForm.GetValue("w_created_at");

            // updated_at
            if (!IsAddOrEdit)
                if (Form.ContainsKey("x_updated_at"))
                    updated_at.AdvancedSearch.SearchValue = CurrentForm.GetValue("x_updated_at");
            if (Form.ContainsKey("z_updated_at"))
                updated_at.AdvancedSearch.SearchOperator = CurrentForm.GetValue("z_updated_at");
            if (Form.ContainsKey("v_updated_at"))
                updated_at.AdvancedSearch.SearchCondition = CurrentForm.GetValue("v_updated_at");
            if (Form.ContainsKey("y_updated_at"))
                updated_at.AdvancedSearch.SearchValue2 = CurrentForm.GetValue("y_updated_at");
            if (Form.ContainsKey("w_updated_at"))
                updated_at.AdvancedSearch.SearchOperator2 = CurrentForm.GetValue("w_updated_at");
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

                // id
                id.HrefValue = "";
                id.TooltipValue = "";

                // first_name
                first_name.HrefValue = "";
                first_name.TooltipValue = "";

                // last_name
                last_name.HrefValue = "";
                last_name.TooltipValue = "";

                // email
                _email.HrefValue = "";
                _email.TooltipValue = "";

                // username
                _username.HrefValue = "";
                _username.TooltipValue = "";

                // mobile
                mobile.HrefValue = "";
                mobile.TooltipValue = "";

                // role_id
                role_id.HrefValue = "";
                role_id.TooltipValue = "";

                // created_at
                created_at.HrefValue = "";
                created_at.TooltipValue = "";

                // updated_at
                updated_at.HrefValue = "";
                updated_at.TooltipValue = "";
            } else if (RowType == RowType.Search) {
                // id
                id.SetupEditAttributes();
                id.EditValue = id.AdvancedSearch.SearchValue; // DN
                id.PlaceHolder = RemoveHtml(id.Caption);

                // first_name
                first_name.SetupEditAttributes();
                if (!first_name.Raw)
                    first_name.AdvancedSearch.SearchValue = HtmlDecode(first_name.AdvancedSearch.SearchValue);
                first_name.EditValue = HtmlEncode(first_name.AdvancedSearch.SearchValue);
                first_name.PlaceHolder = RemoveHtml(first_name.Caption);

                // last_name
                last_name.SetupEditAttributes();
                if (!last_name.Raw)
                    last_name.AdvancedSearch.SearchValue = HtmlDecode(last_name.AdvancedSearch.SearchValue);
                last_name.EditValue = HtmlEncode(last_name.AdvancedSearch.SearchValue);
                last_name.PlaceHolder = RemoveHtml(last_name.Caption);

                // email
                _email.SetupEditAttributes();
                if (!_email.Raw)
                    _email.AdvancedSearch.SearchValue = HtmlDecode(_email.AdvancedSearch.SearchValue);
                _email.EditValue = HtmlEncode(_email.AdvancedSearch.SearchValue);
                _email.PlaceHolder = RemoveHtml(_email.Caption);

                // username
                _username.SetupEditAttributes();
                if (!_username.Raw)
                    _username.AdvancedSearch.SearchValue = HtmlDecode(_username.AdvancedSearch.SearchValue);
                _username.EditValue = HtmlEncode(_username.AdvancedSearch.SearchValue);
                _username.PlaceHolder = RemoveHtml(_username.Caption);

                // mobile
                mobile.SetupEditAttributes();
                if (!mobile.Raw)
                    mobile.AdvancedSearch.SearchValue = HtmlDecode(mobile.AdvancedSearch.SearchValue);
                mobile.EditValue = HtmlEncode(mobile.AdvancedSearch.SearchValue);
                mobile.PlaceHolder = RemoveHtml(mobile.Caption);

                // role_id
                role_id.SetupEditAttributes();
                if (!Security.CanAdmin) { // System admin
                    role_id.EditValue = Language.Phrase("PasswordMask");
                } else {
                    curVal = ConvertToString(role_id.AdvancedSearch.SearchValue)?.Trim() ?? "";
                    if (role_id.Lookup != null && IsDictionary(role_id.Lookup?.Options) && role_id.Lookup?.Options.Values.Count > 0) { // Load from cache // DN
                        role_id.EditValue = role_id.Lookup?.Options.Values.ToList();
                    } else { // Lookup from database
                        if (curVal == "") {
                            filterWrk = "0=1";
                        } else {
                            filterWrk = SearchFilter("[id]", "=", role_id.AdvancedSearch.SearchValue, DataType.Number, "");
                        }
                        sqlWrk = role_id.Lookup?.GetSql(true, filterWrk, null, this, false, true);
                        rswrk = sqlWrk != null ? Connection.GetRows(sqlWrk) : null; // Must use Sync to avoid overwriting ViewValue in RenderViewRow
                        role_id.EditValue = rswrk;
                    }
                    role_id.PlaceHolder = RemoveHtml(role_id.Caption);
                }

                // created_at
                created_at.SetupEditAttributes();
                created_at.EditValue = FormatDateTime(UnformatDateTime(created_at.AdvancedSearch.SearchValue, created_at.FormatPattern), created_at.FormatPattern); // DN
                created_at.PlaceHolder = RemoveHtml(created_at.Caption);
                created_at.SetupEditAttributes();
                created_at.EditValue2 = FormatDateTime(UnformatDateTime(created_at.AdvancedSearch.SearchValue2, created_at.FormatPattern), created_at.FormatPattern); // DN
                created_at.PlaceHolder = RemoveHtml(created_at.Caption);

                // updated_at
                updated_at.SetupEditAttributes();
                updated_at.EditValue = FormatDateTime(UnformatDateTime(updated_at.AdvancedSearch.SearchValue, updated_at.FormatPattern), updated_at.FormatPattern); // DN
                updated_at.PlaceHolder = RemoveHtml(updated_at.Caption);
                updated_at.SetupEditAttributes();
                updated_at.EditValue2 = FormatDateTime(UnformatDateTime(updated_at.AdvancedSearch.SearchValue2, updated_at.FormatPattern), updated_at.FormatPattern); // DN
                updated_at.PlaceHolder = RemoveHtml(updated_at.Caption);
            }
            if (RowType == RowType.Add || RowType == RowType.Edit || RowType == RowType.Search) // Add/Edit/Search row
                SetupFieldTitles();

            // Call Row Rendered event
            if (RowType != RowType.AggregateInit)
                RowRendered();
        }
        #pragma warning restore 1998

        // Validate search
        protected bool ValidateSearch() {
            // Check if validation required
            if (!Config.ServerValidate)
                return true;
            if (!CheckInteger(ConvertToString(id.AdvancedSearch.SearchValue))) {
                id.AddErrorMessage(id.GetErrorMessage(false));
            }
            if (!CheckPhone(ConvertToString(mobile.AdvancedSearch.SearchValue))) {
                mobile.AddErrorMessage(mobile.GetErrorMessage(false));
            }
            if (!CheckDate(ConvertToString(created_at.AdvancedSearch.SearchValue), created_at.FormatPattern)) {
                created_at.AddErrorMessage(created_at.GetErrorMessage(false));
            }
            if (!CheckDate(ConvertToString(created_at.AdvancedSearch.SearchValue2), created_at.FormatPattern)) {
                created_at.AddErrorMessage(created_at.GetErrorMessage(false));
            }
            if (!CheckDate(ConvertToString(updated_at.AdvancedSearch.SearchValue), updated_at.FormatPattern)) {
                updated_at.AddErrorMessage(updated_at.GetErrorMessage(false));
            }
            if (!CheckDate(ConvertToString(updated_at.AdvancedSearch.SearchValue2), updated_at.FormatPattern)) {
                updated_at.AddErrorMessage(updated_at.GetErrorMessage(false));
            }

            // Return validate result
            bool validateSearch = !HasInvalidFields();

            // Call Form CustomValidate event
            string formCustomError = "";
            validateSearch = validateSearch && FormCustomValidate(ref formCustomError);
            if (!Empty(formCustomError))
                FailureMessage = formCustomError;
            return validateSearch;
        }

        // Load advanced search
        public void LoadAdvancedSearch()
        {
            id.AdvancedSearch.Load();
            first_name.AdvancedSearch.Load();
            last_name.AdvancedSearch.Load();
            _email.AdvancedSearch.Load();
            _username.AdvancedSearch.Load();
            mobile.AdvancedSearch.Load();
            role_id.AdvancedSearch.Load();
            created_at.AdvancedSearch.Load();
            updated_at.AdvancedSearch.Load();
        }

        // Set up Breadcrumb
        protected void SetupBreadcrumb() {
            var breadcrumb = new Breadcrumb();
            string url = CurrentUrl();
            breadcrumb.Add("list", TableVar, AppPath(AddMasterUrl("userslist")), "", TableVar, true);
            string pageId = "search";
            breadcrumb.Add("search", pageId, url);
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
