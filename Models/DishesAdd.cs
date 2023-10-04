namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// dishesAdd
    /// </summary>
    public static DishesAdd dishesAdd
    {
        get => HttpData.Get<DishesAdd>("dishesAdd")!;
        set => HttpData["dishesAdd"] = value;
    }

    /// <summary>
    /// Page class for dishes
    /// </summary>
    public class DishesAdd : DishesAddBase
    {
        // Constructor
        public DishesAdd(Controller controller) : base(controller)
        {
        }

        // Constructor
        public DishesAdd() : base()
        {
        }
    }

    /// <summary>
    /// Page base class
    /// </summary>
    public class DishesAddBase : Dishes
    {
        // Page ID
        public string PageID = "add";

        // Project ID
        public string ProjectID = "{89456192-A767-4B60-B043-591A4AA6A5D7}";

        // Table name
        public string TableName { get; set; } = "dishes";

        // Page object name
        public string PageObjName = "dishesAdd";

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
        public DishesAddBase()
        {
            // Initialize
            CurrentPage = this;

            // Table CSS class
            TableClass = "table table-striped table-bordered table-hover table-sm ew-desktop-table ew-add-table";

            // Language object
            Language = ResolveLanguage();

            // Table object (dishes)
            if (dishes == null || dishes is Dishes)
                dishes = this;

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
        public string PageName => "dishesadd";

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
            _name.SetVisibility();
            description.SetVisibility();
            image.SetVisibility();
            price.SetVisibility();
            restaurant_id.SetVisibility();
            created_at.SetVisibility();
            updated_at.SetVisibility();
            created_by.SetVisibility();
            updated_by.SetVisibility();
        }

        // Constructor
        public DishesAddBase(Controller? controller = null): this() { // DN
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
                            result.Add("view", pageName == "dishesview" ? "1" : "0"); // If View page, no primary button
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
            await SetupLookupOptions(restaurant_id);
            await SetupLookupOptions(created_by);
            await SetupLookupOptions(updated_by);

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

            // Set up master/detail parameters
            // NOTE: must be after loadOldRecord to prevent master key values overwritten
            SetupMasterParms();

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
                        return Terminate("disheslist"); // No matching record, return to List page // DN
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
                        if (GetPageName(returnUrl) == "disheslist")
                            returnUrl = AddMasterUrl(ListUrl); // List page, return to List page with correct master key if necessary
                        else if (GetPageName(returnUrl) == "dishesview")
                            returnUrl = ViewUrl; // View page, return to View page with key URL directly

                        // Handle UseAjaxActions
                        if (IsModal && UseAjaxActions) {
                            IsModal = false;
                            if (GetPageName(returnUrl) != "disheslist") {
                                TempData["Return-Url"] = returnUrl; // Save return URL
                                returnUrl = "disheslist"; // Return list page content
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
                dishesAdd?.PageRender();
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
            image.Upload.Index = CurrentForm.Index;
            if (!await image.Upload.UploadFile()) // DN
                End(image.Upload.Message);
            image.CurrentValue = image.Upload.FileName;
        }
        #pragma warning restore 1998

        // Load default values
        protected void LoadDefaultValues() {
        }

        #pragma warning disable 1998
        // Load form values
        protected async Task LoadFormValues() {
            if (CurrentForm == null)
                return;
            bool validate = !Config.ServerValidate;
            string val;

            // Check field name 'name' before field var 'x__name'
            val = CurrentForm.HasValue("name") ? CurrentForm.GetValue("name") : CurrentForm.GetValue("x__name");
            if (!_name.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("name") && !CurrentForm.HasValue("x__name")) // DN
                    _name.Visible = false; // Disable update for API request
                else
                    _name.SetFormValue(val);
            }

            // Check field name 'description' before field var 'x_description'
            val = CurrentForm.HasValue("description") ? CurrentForm.GetValue("description") : CurrentForm.GetValue("x_description");
            if (!description.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("description") && !CurrentForm.HasValue("x_description")) // DN
                    description.Visible = false; // Disable update for API request
                else
                    description.SetFormValue(val);
            }

            // Check field name 'price' before field var 'x_price'
            val = CurrentForm.HasValue("price") ? CurrentForm.GetValue("price") : CurrentForm.GetValue("x_price");
            if (!price.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("price") && !CurrentForm.HasValue("x_price")) // DN
                    price.Visible = false; // Disable update for API request
                else
                    price.SetFormValue(val, true, validate);
            }

            // Check field name 'restaurant_id' before field var 'x_restaurant_id'
            val = CurrentForm.HasValue("restaurant_id") ? CurrentForm.GetValue("restaurant_id") : CurrentForm.GetValue("x_restaurant_id");
            if (!restaurant_id.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("restaurant_id") && !CurrentForm.HasValue("x_restaurant_id")) // DN
                    restaurant_id.Visible = false; // Disable update for API request
                else
                    restaurant_id.SetFormValue(val);
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

            // Check field name 'created_by' before field var 'x_created_by'
            val = CurrentForm.HasValue("created_by") ? CurrentForm.GetValue("created_by") : CurrentForm.GetValue("x_created_by");
            if (!created_by.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("created_by") && !CurrentForm.HasValue("x_created_by")) // DN
                    created_by.Visible = false; // Disable update for API request
                else
                    created_by.SetFormValue(val);
            }

            // Check field name 'updated_by' before field var 'x_updated_by'
            val = CurrentForm.HasValue("updated_by") ? CurrentForm.GetValue("updated_by") : CurrentForm.GetValue("x_updated_by");
            if (!updated_by.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("updated_by") && !CurrentForm.HasValue("x_updated_by")) // DN
                    updated_by.Visible = false; // Disable update for API request
                else
                    updated_by.SetFormValue(val);
            }

            // Check field name 'id' before field var 'x_id'
            val = CurrentForm.HasValue("id") ? CurrentForm.GetValue("id") : CurrentForm.GetValue("x_id");
            await GetUploadFiles(); // Get upload files
        }
        #pragma warning restore 1998

        // Restore form values
        public void RestoreFormValues()
        {
            _name.CurrentValue = _name.FormValue;
            description.CurrentValue = description.FormValue;
            price.CurrentValue = price.FormValue;
            restaurant_id.CurrentValue = restaurant_id.FormValue;
            created_at.CurrentValue = created_at.FormValue;
            created_at.CurrentValue = UnformatDateTime(created_at.CurrentValue, created_at.FormatPattern);
            updated_at.CurrentValue = updated_at.FormValue;
            updated_at.CurrentValue = UnformatDateTime(updated_at.CurrentValue, updated_at.FormatPattern);
            created_by.CurrentValue = created_by.FormValue;
            updated_by.CurrentValue = updated_by.FormValue;
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
            _name.SetDbValue(row["name"]);
            description.SetDbValue(row["description"]);
            image.Upload.DbValue = row["image"];
            image.SetDbValue(image.Upload.DbValue);
            price.SetDbValue(row["price"]);
            restaurant_id.SetDbValue(row["restaurant_id"]);
            created_at.SetDbValue(row["created_at"]);
            updated_at.SetDbValue(row["updated_at"]);
            created_by.SetDbValue(row["created_by"]);
            updated_by.SetDbValue(row["updated_by"]);
        }
        #pragma warning restore 162, 168, 1998, 4014

        // Return a row with default values
        protected Dictionary<string, object> NewRow() {
            var row = new Dictionary<string, object>();
            row.Add("id", id.DefaultValue ?? DbNullValue); // DN
            row.Add("name", _name.DefaultValue ?? DbNullValue); // DN
            row.Add("description", description.DefaultValue ?? DbNullValue); // DN
            row.Add("image", image.DefaultValue ?? DbNullValue); // DN
            row.Add("price", price.DefaultValue ?? DbNullValue); // DN
            row.Add("restaurant_id", restaurant_id.DefaultValue ?? DbNullValue); // DN
            row.Add("created_at", created_at.DefaultValue ?? DbNullValue); // DN
            row.Add("updated_at", updated_at.DefaultValue ?? DbNullValue); // DN
            row.Add("created_by", created_by.DefaultValue ?? DbNullValue); // DN
            row.Add("updated_by", updated_by.DefaultValue ?? DbNullValue); // DN
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

            // name
            _name.RowCssClass = "row";

            // description
            description.RowCssClass = "row";

            // image
            image.RowCssClass = "row";

            // price
            price.RowCssClass = "row";

            // restaurant_id
            restaurant_id.RowCssClass = "row";

            // created_at
            created_at.RowCssClass = "row";

            // updated_at
            updated_at.RowCssClass = "row";

            // created_by
            created_by.RowCssClass = "row";

            // updated_by
            updated_by.RowCssClass = "row";

            // View row
            if (RowType == RowType.View) {
                // id
                id.ViewValue = id.CurrentValue;
                id.ViewCustomAttributes = "";

                // name
                _name.ViewValue = ConvertToString(_name.CurrentValue); // DN
                _name.ViewCustomAttributes = "";

                // description
                description.ViewValue = description.CurrentValue;
                description.ViewCustomAttributes = "";

                // image
                if (!IsNull(image.Upload.DbValue)) {
                    image.ImageWidth = 120;
                    image.ImageHeight = 0;
                    image.ImageAlt = image.Alt;
                    image.ImageCssClass = "ew-image";
                    image.ViewValue = image.Upload.DbValue;
                } else {
                    image.ViewValue = "";
                }
                image.ViewCustomAttributes = "";

                // price
                price.ViewValue = price.CurrentValue;
                price.ViewValue = FormatNumber(price.ViewValue, price.FormatPattern);
                price.ViewCustomAttributes = "";

                // restaurant_id
                curVal = ConvertToString(restaurant_id.CurrentValue);
                if (!Empty(curVal)) {
                    if (restaurant_id.Lookup != null && IsDictionary(restaurant_id.Lookup?.Options) && restaurant_id.Lookup?.Options.Values.Count > 0) { // Load from cache // DN
                        restaurant_id.ViewValue = restaurant_id.LookupCacheOption(curVal);
                    } else { // Lookup from database // DN
                        filterWrk = SearchFilter("[id]", "=", restaurant_id.CurrentValue, DataType.Number, "");
                        sqlWrk = restaurant_id.Lookup?.GetSql(false, filterWrk, null, this, true, true);
                        rswrk = sqlWrk != null ? Connection.GetRows(sqlWrk) : null; // Must use Sync to avoid overwriting ViewValue in RenderViewRow
                        if (rswrk?.Count > 0 && restaurant_id.Lookup != null) { // Lookup values found
                            var listwrk = restaurant_id.Lookup?.RenderViewRow(rswrk[0]);
                            restaurant_id.ViewValue = restaurant_id.HighlightLookup(ConvertToString(rswrk[0]), restaurant_id.DisplayValue(listwrk));
                        } else {
                            restaurant_id.ViewValue = FormatNumber(restaurant_id.CurrentValue, restaurant_id.FormatPattern);
                        }
                    }
                } else {
                    restaurant_id.ViewValue = DbNullValue;
                }
                restaurant_id.ViewCustomAttributes = "";

                // created_at
                created_at.ViewValue = created_at.CurrentValue;
                created_at.ViewValue = FormatDateTime(created_at.ViewValue, created_at.FormatPattern);
                created_at.ViewCustomAttributes = "";

                // updated_at
                updated_at.ViewValue = updated_at.CurrentValue;
                updated_at.ViewValue = FormatDateTime(updated_at.ViewValue, updated_at.FormatPattern);
                updated_at.ViewCustomAttributes = "";

                // created_by
                created_by.ViewValue = created_by.CurrentValue;
                curVal = ConvertToString(created_by.CurrentValue);
                if (!Empty(curVal)) {
                    if (created_by.Lookup != null && IsDictionary(created_by.Lookup?.Options) && created_by.Lookup?.Options.Values.Count > 0) { // Load from cache // DN
                        created_by.ViewValue = created_by.LookupCacheOption(curVal);
                    } else { // Lookup from database // DN
                        filterWrk = SearchFilter("[id]", "=", created_by.CurrentValue, DataType.Number, "");
                        sqlWrk = created_by.Lookup?.GetSql(false, filterWrk, null, this, true, true);
                        rswrk = sqlWrk != null ? Connection.GetRows(sqlWrk) : null; // Must use Sync to avoid overwriting ViewValue in RenderViewRow
                        if (rswrk?.Count > 0 && created_by.Lookup != null) { // Lookup values found
                            var listwrk = created_by.Lookup?.RenderViewRow(rswrk[0]);
                            created_by.ViewValue = created_by.HighlightLookup(ConvertToString(rswrk[0]), created_by.DisplayValue(listwrk));
                        } else {
                            created_by.ViewValue = FormatNumber(created_by.CurrentValue, created_by.FormatPattern);
                        }
                    }
                } else {
                    created_by.ViewValue = DbNullValue;
                }
                created_by.ViewCustomAttributes = "";

                // updated_by
                updated_by.ViewValue = updated_by.CurrentValue;
                curVal = ConvertToString(updated_by.CurrentValue);
                if (!Empty(curVal)) {
                    if (updated_by.Lookup != null && IsDictionary(updated_by.Lookup?.Options) && updated_by.Lookup?.Options.Values.Count > 0) { // Load from cache // DN
                        updated_by.ViewValue = updated_by.LookupCacheOption(curVal);
                    } else { // Lookup from database // DN
                        filterWrk = SearchFilter("[id]", "=", updated_by.CurrentValue, DataType.Number, "");
                        sqlWrk = updated_by.Lookup?.GetSql(false, filterWrk, null, this, true, true);
                        rswrk = sqlWrk != null ? Connection.GetRows(sqlWrk) : null; // Must use Sync to avoid overwriting ViewValue in RenderViewRow
                        if (rswrk?.Count > 0 && updated_by.Lookup != null) { // Lookup values found
                            var listwrk = updated_by.Lookup?.RenderViewRow(rswrk[0]);
                            updated_by.ViewValue = updated_by.HighlightLookup(ConvertToString(rswrk[0]), updated_by.DisplayValue(listwrk));
                        } else {
                            updated_by.ViewValue = FormatNumber(updated_by.CurrentValue, updated_by.FormatPattern);
                        }
                    }
                } else {
                    updated_by.ViewValue = DbNullValue;
                }
                updated_by.ViewCustomAttributes = "";

                // name
                _name.HrefValue = "";

                // description
                description.HrefValue = "";

                // image
                if (!IsNull(image.Upload.DbValue)) {
                    image.HrefValue = ConvertToString(GetFileUploadUrl(image, image.HtmlDecode(ConvertToString(image.Upload.DbValue)))); // Add prefix/suffix
                    image.LinkAttrs["target"] = ""; // Add target
                    if (IsExport())
                        image.HrefValue = FullUrl(ConvertToString(image.HrefValue), "href");
                } else {
                    image.HrefValue = "";
                }
                image.ExportHrefValue = image.UploadPath + image.Upload.DbValue;

                // price
                price.HrefValue = "";

                // restaurant_id
                restaurant_id.HrefValue = "";

                // created_at
                created_at.HrefValue = "";

                // updated_at
                updated_at.HrefValue = "";

                // created_by
                created_by.HrefValue = "";

                // updated_by
                updated_by.HrefValue = "";
            } else if (RowType == RowType.Add) {
                // name
                _name.SetupEditAttributes();
                if (!_name.Raw)
                    _name.CurrentValue = HtmlDecode(_name.CurrentValue);
                _name.EditValue = HtmlEncode(_name.CurrentValue);
                _name.PlaceHolder = RemoveHtml(_name.Caption);

                // description
                description.SetupEditAttributes();
                description.EditValue = description.CurrentValue; // DN
                description.PlaceHolder = RemoveHtml(description.Caption);

                // image
                image.SetupEditAttributes();
                if (!IsNull(image.Upload.DbValue)) {
                    image.ImageWidth = 120;
                    image.ImageHeight = 0;
                    image.ImageAlt = image.Alt;
                    image.ImageCssClass = "ew-image";
                    image.EditValue = image.Upload.DbValue;
                } else {
                    image.EditValue = "";
                }
                if (!Empty(image.CurrentValue))
                        image.Upload.FileName = ConvertToString(image.CurrentValue);
                image.Upload.DbValue = DbNullValue;
                if ((IsShow || IsCopy) && !EventCancelled)
                    await RenderUploadField(image);

                // price
                price.SetupEditAttributes();
                price.EditValue = price.CurrentValue; // DN
                price.PlaceHolder = RemoveHtml(price.Caption);
                if (!Empty(price.EditValue) && IsNumeric(price.EditValue))
                    price.EditValue = FormatNumber(price.EditValue, null);

                // restaurant_id
                restaurant_id.SetupEditAttributes();
                if (!Empty(restaurant_id.SessionValue)) {
                    restaurant_id.CurrentValue = ForeignKeyValue(restaurant_id.SessionValue);
                    curVal = ConvertToString(restaurant_id.CurrentValue);
                    if (!Empty(curVal)) {
                        if (restaurant_id.Lookup != null && IsDictionary(restaurant_id.Lookup?.Options) && restaurant_id.Lookup?.Options.Values.Count > 0) { // Load from cache // DN
                            restaurant_id.ViewValue = restaurant_id.LookupCacheOption(curVal);
                        } else { // Lookup from database // DN
                            filterWrk = SearchFilter("[id]", "=", restaurant_id.CurrentValue, DataType.Number, "");
                            sqlWrk = restaurant_id.Lookup?.GetSql(false, filterWrk, null, this, true, true);
                            rswrk = sqlWrk != null ? Connection.GetRows(sqlWrk) : null; // Must use Sync to avoid overwriting ViewValue in RenderViewRow
                            if (rswrk?.Count > 0 && restaurant_id.Lookup != null) { // Lookup values found
                                var listwrk = restaurant_id.Lookup?.RenderViewRow(rswrk[0]);
                                restaurant_id.ViewValue = restaurant_id.HighlightLookup(ConvertToString(rswrk[0]), restaurant_id.DisplayValue(listwrk));
                            } else {
                                restaurant_id.ViewValue = FormatNumber(restaurant_id.CurrentValue, restaurant_id.FormatPattern);
                            }
                        }
                    } else {
                        restaurant_id.ViewValue = DbNullValue;
                    }
                    restaurant_id.ViewCustomAttributes = "";
                } else {
                    curVal = ConvertToString(restaurant_id.CurrentValue)?.Trim() ?? "";
                    if (restaurant_id.Lookup != null && IsDictionary(restaurant_id.Lookup?.Options) && restaurant_id.Lookup?.Options.Values.Count > 0) { // Load from cache // DN
                        restaurant_id.EditValue = restaurant_id.Lookup?.Options.Values.ToList();
                    } else { // Lookup from database
                        if (curVal == "") {
                            filterWrk = "0=1";
                        } else {
                            filterWrk = SearchFilter("[id]", "=", restaurant_id.CurrentValue, DataType.Number, "");
                        }
                        sqlWrk = restaurant_id.Lookup?.GetSql(true, filterWrk, null, this, false, true);
                        rswrk = sqlWrk != null ? Connection.GetRows(sqlWrk) : null; // Must use Sync to avoid overwriting ViewValue in RenderViewRow
                        restaurant_id.EditValue = rswrk;
                    }
                    restaurant_id.PlaceHolder = RemoveHtml(restaurant_id.Caption);
                    if (!Empty(restaurant_id.EditValue) && IsNumeric(restaurant_id.EditValue))
                        restaurant_id.EditValue = FormatNumber(restaurant_id.EditValue, null);
                }

                // created_at

                // updated_at

                // created_by

                // updated_by

                // Add refer script

                // name
                _name.HrefValue = "";

                // description
                description.HrefValue = "";

                // image
                if (!IsNull(image.Upload.DbValue)) {
                    image.HrefValue = ConvertToString(GetFileUploadUrl(image, image.HtmlDecode(ConvertToString(image.Upload.DbValue)))); // Add prefix/suffix
                    image.LinkAttrs["target"] = ""; // Add target
                    if (IsExport())
                        image.HrefValue = FullUrl(ConvertToString(image.HrefValue), "href");
                } else {
                    image.HrefValue = "";
                }
                image.ExportHrefValue = image.UploadPath + image.Upload.DbValue;

                // price
                price.HrefValue = "";

                // restaurant_id
                restaurant_id.HrefValue = "";

                // created_at
                created_at.HrefValue = "";

                // updated_at
                updated_at.HrefValue = "";

                // created_by
                created_by.HrefValue = "";

                // updated_by
                updated_by.HrefValue = "";
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
            if (_name.Required) {
                if (!_name.IsDetailKey && Empty(_name.FormValue)) {
                    _name.AddErrorMessage(ConvertToString(_name.RequiredErrorMessage).Replace("%s", _name.Caption));
                }
            }
            if (description.Required) {
                if (!description.IsDetailKey && Empty(description.FormValue)) {
                    description.AddErrorMessage(ConvertToString(description.RequiredErrorMessage).Replace("%s", description.Caption));
                }
            }
            if (image.Required) {
                if (image.Upload.FileName == "" && !image.Upload.KeepFile) {
                    image.AddErrorMessage(ConvertToString(image.RequiredErrorMessage).Replace("%s", image.Caption));
                }
            }
            if (price.Required) {
                if (!price.IsDetailKey && Empty(price.FormValue)) {
                    price.AddErrorMessage(ConvertToString(price.RequiredErrorMessage).Replace("%s", price.Caption));
                }
            }
            if (!CheckNumber(price.FormValue)) {
                price.AddErrorMessage(price.GetErrorMessage(false));
            }
            if (restaurant_id.Required) {
                if (!restaurant_id.IsDetailKey && Empty(restaurant_id.FormValue)) {
                    restaurant_id.AddErrorMessage(ConvertToString(restaurant_id.RequiredErrorMessage).Replace("%s", restaurant_id.Caption));
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
            if (created_by.Required) {
                if (!created_by.IsDetailKey && Empty(created_by.FormValue)) {
                    created_by.AddErrorMessage(ConvertToString(created_by.RequiredErrorMessage).Replace("%s", created_by.Caption));
                }
            }
            if (updated_by.Required) {
                if (!updated_by.IsDetailKey && Empty(updated_by.FormValue)) {
                    updated_by.AddErrorMessage(ConvertToString(updated_by.RequiredErrorMessage).Replace("%s", updated_by.Caption));
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
                // name
                _name.SetDbValue(rsnew, _name.CurrentValue);

                // description
                description.SetDbValue(rsnew, description.CurrentValue);

                // image
                if (image.Visible && !image.Upload.KeepFile) {
                    image.Upload.DbValue = ""; // No need to delete old file
                    if (Empty(image.Upload.FileName)) {
                        rsnew["image"] = DbNullValue;
                    } else {
                        FixUploadTempFileNames(image);
                        rsnew["image"] = image.Upload.FileName;
                    }
                }

                // price
                price.SetDbValue(rsnew, price.CurrentValue, Empty(price.CurrentValue));

                // restaurant_id
                restaurant_id.SetDbValue(rsnew, restaurant_id.CurrentValue, Empty(restaurant_id.CurrentValue));

                // created_at
                created_at.CurrentValue = created_at.GetAutoUpdateValue();
                created_at.SetDbValue(rsnew, ConvertToDateTime(created_at.CurrentValue, created_at.FormatPattern), false);

                // updated_at
                updated_at.CurrentValue = updated_at.GetAutoUpdateValue();
                updated_at.SetDbValue(rsnew, ConvertToDateTime(updated_at.CurrentValue, updated_at.FormatPattern), false);

                // created_by
                created_by.CurrentValue = created_by.GetAutoUpdateValue();
                created_by.SetDbValue(rsnew, created_by.CurrentValue, false);

                // updated_by
                updated_by.CurrentValue = updated_by.GetAutoUpdateValue();
                updated_by.SetDbValue(rsnew, updated_by.CurrentValue, false);
            } catch (Exception e) {
                if (Config.Debug)
                    throw;
                FailureMessage = e.Message;
                return JsonBoolResult.FalseResult;
            }
            if (image.Visible && !image.Upload.KeepFile) {
                if (!Empty(image.Upload.FileName)) {
                    image.Upload.DbValue = DbNullValue;
                    FixUploadFileNames(image);
                    image.SetDbValue(rsnew, image.Upload.FileName);
                }
            }

            // Update current values
            SetCurrentValues(rsnew);
            string? masterFilter;
            Dictionary<string, object?> detailKeys;
            bool validMasterRecord;

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
                if (result) {
                    if (image.Visible && !image.Upload.KeepFile) {
                        image.Upload.DbValue = DbNullValue;
                        if (!await SaveUploadFiles(image, ConvertToString(rsnew["image"]), false))
                        {
                            FailureMessage = Language.Phrase("UploadError7");
                            return JsonBoolResult.FalseResult;
                        }
                    }
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

        // Set up master/detail based on QueryString
        protected void SetupMasterParms() {
            bool validMaster = false;
            StringValues masterTblVar;
            StringValues fk;
            Dictionary<string, object> foreignKeys = new ();

            // Get the keys for master table
            if (Query.TryGetValue(Config.TableShowMaster, out masterTblVar) || Query.TryGetValue(Config.TableMaster, out masterTblVar)) { // Do not use Get()
                if (Empty(masterTblVar)) {
                    validMaster = true;
                    DbMasterFilter = "";
                    DbDetailFilter = "";
                }
                if (masterTblVar == "restaurants") {
                    validMaster = true;
                    if (restaurants != null && (Get("fk_id", out fk) || Get("restaurant_id", out fk))) {
                        restaurants.id.QueryValue = fk;
                        restaurant_id.QueryValue = restaurants.id.QueryValue;
                        restaurant_id.SessionValue = restaurant_id.QueryValue;
                        foreignKeys.Add("restaurant_id", fk);
                        if (!IsNumeric(restaurant_id.QueryValue))
                            validMaster = false;
                    } else {
                        validMaster = false;
                    }
                }
            } else if (Form.TryGetValue(Config.TableShowMaster, out masterTblVar) || Form.TryGetValue(Config.TableMaster, out masterTblVar)) {
                if (masterTblVar == "") {
                    validMaster = true;
                    DbMasterFilter = "";
                    DbDetailFilter = "";
                }
                if (masterTblVar == "restaurants") {
                    validMaster = true;
                    if (restaurants != null && (Post("fk_id", out fk) || Post("restaurant_id", out fk))) {
                        restaurants.id.FormValue = fk;
                        restaurant_id.FormValue = restaurants.id.FormValue;
                        restaurant_id.SessionValue = restaurant_id.FormValue;
                        foreignKeys.Add("restaurant_id", fk);
                        if (!IsNumeric(restaurant_id.FormValue))
                            validMaster = false;
                    } else {
                        validMaster = false;
                    }
                }
            }
            if (validMaster) {
                // Save current master table
                CurrentMasterTable = masterTblVar.ToString();

                // Clear previous master key from Session
                if (masterTblVar != "restaurants") {
                    if (!foreignKeys.ContainsKey("restaurant_id")) // Not current foreign key
                        restaurant_id.SessionValue = "";
                }
            }
            DbMasterFilter = MasterFilterFromSession; // Get master filter from session
            DbDetailFilter = DetailFilterFromSession; // Get detail filter from session
        }

        // Set up Breadcrumb
        protected void SetupBreadcrumb() {
            var breadcrumb = new Breadcrumb();
            string url = CurrentUrl();
            breadcrumb.Add("list", TableVar, AppPath(AddMasterUrl("disheslist")), "", TableVar, true);
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
