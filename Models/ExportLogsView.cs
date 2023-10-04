namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// exportLogsView
    /// </summary>
    public static ExportLogsView exportLogsView
    {
        get => HttpData.Get<ExportLogsView>("exportLogsView")!;
        set => HttpData["exportLogsView"] = value;
    }

    /// <summary>
    /// Page class for export_logs
    /// </summary>
    public class ExportLogsView : ExportLogsViewBase
    {
        // Constructor
        public ExportLogsView(Controller controller) : base(controller)
        {
        }

        // Constructor
        public ExportLogsView() : base()
        {
        }
    }

    /// <summary>
    /// Page base class
    /// </summary>
    public class ExportLogsViewBase : ExportLogs
    {
        // Page ID
        public string PageID = "view";

        // Project ID
        public string ProjectID = "{89456192-A767-4B60-B043-591A4AA6A5D7}";

        // Table name
        public string TableName { get; set; } = "export_logs";

        // Page object name
        public string PageObjName = "exportLogsView";

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
        public ExportLogsViewBase()
        {
            // Initialize
            CurrentPage = this;

            // Table CSS class
            TableClass = "table table-striped table-bordered table-hover table-sm ew-view-table";

            // Language object
            Language = ResolveLanguage();

            // Table object (exportLogs)
            if (exportLogs == null || exportLogs is ExportLogs)
                exportLogs = this;

            // DN
            string[] keys = new string[0];
            StringValues str = "";
            object? obj = null;
            string currentPageName = CurrentPageName();
            string currentUrl = AppPath(currentPageName); // DN
            if (IsApi()) {
                if (RouteValues.TryGetValue("key", out object? k) && !Empty(k))
                    keys = ConvertToString(k).Split('/');
                if (keys.Length > 0)
                    RecordKeys["file_id"] = keys[0];
            } else {
                RecordKeys["file_id"] = RouteValues.TryGetValue("file_id", out obj) && obj != null ? UrlDecode(obj) : Get("file_id"); // DN
            }

            // Start time
            StartTime = Environment.TickCount;

            // Debug message
            LoadDebugMessage();

            // Open connection
            Conn = Connection; // DN

            // User table object (users)
            UserTable = Resolve("usertable")!;
            UserTableConn = GetConnection(UserTable.DbId);

            // Other options
            OtherOptions["detail"] = new () { TagClassName = "ew-detail-option" };
            OtherOptions["action"] = new () { TagClassName = "ew-action-option" };
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
        public string PageName => "exportlogsview";

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

        // Update URLs
        public string InlineAddUrl = "";

        public string GridAddUrl = "";

        public string GridEditUrl = "";

        public string MultiEditUrl = "";

        public string MultiDeleteUrl = "";

        public string MultiUpdateUrl = "";

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
            file_id.SetVisibility();
            created_at.SetVisibility();
            _username.SetVisibility();
            export_type.SetVisibility();
            entity.SetVisibility();
            key_value.SetVisibility();
            filename.SetVisibility();
            _request.SetVisibility();
        }

        // Constructor
        public ExportLogsViewBase(Controller? controller = null): this() { // DN
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
                            result.Add("view", pageName == "exportlogsview" ? "1" : "0"); // If View page, no primary button
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
            key += UrlEncode(ConvertToString(dict.ContainsKey("file_id") ? dict["file_id"] : file_id.CurrentValue));
            return key;
        }

        // Hide fields for Add/Edit
        protected void HideFieldsForAddEdit() {
        }

        private Pager? _pager; // DN

        public int DisplayRecords = 1; // Number of display records

        public int StartRecord;

        public int StopRecord;

        public int TotalRecords = -1;

        public int RecordRange = 10;

        public int RecordCount;

        public Dictionary<string, string> RecordKeys = new ();

        public bool IsModal = false;

        public string SearchWhere = "";

        public string DbMasterFilter = "";

        public string DbDetailFilter = "";

        public bool MasterRecordExists;

        public DbDataReader? Recordset = null;

        public Pager Pager
        {
            get {
                _pager ??= new PrevNextPager(this, StartRecord, DisplayRecords, TotalRecords, "", RecordRange, AutoHidePager);
                _pager.PageNumberName = Config.TableStartRec; // Same as start record
                _pager.PagePhraseId = "Record"; // Show as record
                return _pager;
            }
        }

        #pragma warning disable 168, 219
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

            // Check modal
            if (IsModal)
                SkipHeaderFooter = true;

            // Load current record
            bool loadCurrentRecord = false;
            string returnUrl = "";
            bool matchRecord = false;
            string[] keyValues = {};
            object? v;
            StringValues sv;
            if (IsApi()) {
                if (RouteValues.TryGetValue(Config.ApiKeyName, out object? k)) {
                    if (!Empty(k))
                        keyValues = ConvertToString(k).Split('/');
                } else { // Get key from query string
                    string key = Get(Config.ApiKeyName);
                    if (!Empty(key))
                        keyValues = key.Split(',');
                }
                if (keyValues.Length == 0)
                    return new JsonBoolResult(new { success = false, error = Language.Phrase("NoRecord"), version = Config.ProductVersion }, false);
            }

            // Check for start / page parameters
            if (IsNumeric(Get(Config.TableStartRec)) || IsNumeric(Get(Config.TablePageNumber)))
                loadCurrentRecord = true;
            if (RouteValues.TryGetValue("file_id", out v) && !Empty(v)) { // DN
                file_id.QueryValue = UrlDecode(v); // DN
                RecordKeys["file_id"] = file_id.QueryValue;
            } else if (Get("file_id", out sv)) {
                file_id.QueryValue = sv.ToString();
                RecordKeys["file_id"] = file_id.QueryValue;
            } else if (IsApi() && !Empty(keyValues)) {
                file_id.QueryValue = ConvertToString(keyValues[0]);
                RecordKeys["file_id"] = file_id.QueryValue;
            } else if (!loadCurrentRecord) {
                returnUrl = "exportlogslist"; // Return to list
            }

            // Get action
            CurrentAction = "show"; // Display form
            switch (CurrentAction) {
                case "show": // Get a record to display
                    if (!IsModal) { // Normal view page
                        StartRecord = 1; // Initialize start position
                        Recordset = await LoadRecordset(); // Load records
                        TotalRecords = await ListRecordCountAsync(); // Get record count // DN
                        if (TotalRecords <= 0) { // No record found // DN
                            if (Empty(SuccessMessage) && Empty(FailureMessage))
                                FailureMessage = Language.Phrase("NoRecord"); // Set no record message
                            if (IsApi()) {
                                if (!Empty(SuccessMessage))
                                    return new JsonBoolResult(new { success = true, message = SuccessMessage, version = Config.ProductVersion }, true);
                                else
                                    return new JsonBoolResult(new { success = false, error = FailureMessage, version = Config.ProductVersion }, false);
                            } else {
                                return Terminate("exportlogslist"); // Return to list page
                            }
                        } else if (loadCurrentRecord) { // Load current record position
                            SetupStartRecord(); // Set up start record position
                            // Point to current record
                            if (Recordset != null && StartRecord <= TotalRecords) {
                                matchRecord = true;
                                for (int i = 1; i <= StartRecord; i++)
                                    await Recordset.ReadAsync();
                                // Redirect to correct record
                                await LoadRowValues(Recordset);
                                string url = GetCurrentUrl();
                                return Terminate(url);
                            }
                        } else { // Match key values
                            if (Recordset != null) {
                                while (await Recordset.ReadAsync()) {
                                    if (SameString(file_id.CurrentValue, Recordset["file_id"])) {
                                        StartRecordNumber = StartRecord; // Save record position
                                        matchRecord = true;
                                        break;
                                    } else {
                                        StartRecord++;
                                    }
                                }
                            }
                        }
                        if (!matchRecord) {
                            if (Empty(SuccessMessage) && Empty(FailureMessage))
                                FailureMessage = Language.Phrase("NoRecord"); // Set no record message
                            if (IsApi()) {
                                if (!Empty(SuccessMessage))
                                    return new JsonBoolResult(new { success = true, message = SuccessMessage, version = Config.ProductVersion }, true);
                                else
                                    return new JsonBoolResult(new { success = false, error = FailureMessage, version = Config.ProductVersion }, false);
                            } else {
                                return Terminate("exportlogslist"); // Return to list page
                            }
                        } else {
                            await LoadRowValues(Recordset); // Load row values
                        }
                    } else { // Modal view page
                        bool res;
                        if (IsApi()) {
                            string filter = GetRecordFilter();
                            CurrentFilter = filter;
                            string sql = CurrentSql;
                            var conn = await GetConnectionAsync();
                            Recordset = await conn.GetDataReaderAsync(sql);
                            res = !Empty(Recordset) && await Recordset.ReadAsync();
                        } else {
                            res = await LoadRow();
                        }
                        if (!res) { // Load record based on key
                            if (Empty(SuccessMessage) && Empty(FailureMessage))
                                FailureMessage = Language.Phrase("NoRecord"); // Set no record message
                            if (IsApi()) {
                                if (!Empty(SuccessMessage))
                                    return new JsonBoolResult(new { success = true, message = SuccessMessage, version = Config.ProductVersion }, true);
                                else
                                    return new JsonBoolResult(new { success = false, error = FailureMessage, version = Config.ProductVersion }, false);
                            } else {
                                return Terminate("exportlogslist"); // Return to list page
                            }
                        }
                    } // End modal checking
                    break;
            }
            if (!Empty(returnUrl))
                return Terminate(returnUrl);

            // Render row
            RowType = RowType.View;
            ResetAttributes();
            await RenderRow();

            // Set up Breadcrumb
            if (!IsExport())
                SetupBreadcrumb();

            // Normal return
            if (IsApi()) // Get current record only
                if (!IsExport())
                    return Controller.Json(new { success = true, TableVar = await GetRecordFromRecordset(Recordset), version = Config.ProductVersion });

            // Set LoginStatus, Page Rendering and Page Render
            if (!IsApi() && !IsTerminated) {
                SetupLoginStatus(); // Setup login status

                // Pass login status to client side
                SetClientVar("login", LoginStatus);

                // Global Page Rendering event
                PageRendering();

                // Page Render event
                exportLogsView?.PageRender();
            }
            return PageResult();
        }
        #pragma warning restore 168, 219

        // Set up other options
        #pragma warning disable 168, 219

        public void SetupOtherOptions()
        {
            var options = OtherOptions;
            var option = options["action"];
            ListOption item;
            string links = "";

            // Set up action default
            option = options["action"];
            option.DropDownButtonPhrase = "ButtonActions";
            option.UseDropDownButton = !IsJsonResponse() && true;
            option.UseButtonGroup = true;
            item = option.AddGroupOption();
            item.Body = "";
            item.Visible = false;
        }
        #pragma warning restore 168, 219

        // Load recordset // DN
        public async Task<DbDataReader?> LoadRecordset(int offset = -1, int rowcnt = -1)
        {
            // Load list page SQL
            string sql = ListSql;

            // Load recordset // DN
            var dr = await Connection.SelectLimit(sql, rowcnt, offset, !Empty(OrderBy) || !Empty(SessionOrderBy));

            // Call Recordset Selected event
            RecordsetSelected(dr);
            return dr;
        }

        // Load rows // DN
        public async Task<List<Dictionary<string, object>>> LoadRows(int offset = -1, int rowcnt = -1)
        {
            // Load list page SQL
            string sql = ListSql;

            // Load rows // DN
            using var dr = await Connection.SelectLimit(sql, rowcnt, offset, !Empty(OrderBy) || !Empty(SessionOrderBy));
            var rows = await Connection.GetRowsAsync(dr);
            dr.Close(); // Close datareader before return
            return rows;
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
            file_id.SetDbValue(row["file_id"]);
            created_at.SetDbValue(row["created_at"]);
            _username.SetDbValue(row["username"]);
            export_type.SetDbValue(row["export_type"]);
            entity.SetDbValue(row["entity"]);
            key_value.SetDbValue(row["key_value"]);
            filename.SetDbValue(row["filename"]);
            _request.SetDbValue(row["request"]);
        }
        #pragma warning restore 162, 168, 1998, 4014

        // Return a row with default values
        protected Dictionary<string, object> NewRow() {
            var row = new Dictionary<string, object>();
            row.Add("file_id", file_id.DefaultValue ?? DbNullValue); // DN
            row.Add("created_at", created_at.DefaultValue ?? DbNullValue); // DN
            row.Add("username", _username.DefaultValue ?? DbNullValue); // DN
            row.Add("export_type", export_type.DefaultValue ?? DbNullValue); // DN
            row.Add("entity", entity.DefaultValue ?? DbNullValue); // DN
            row.Add("key_value", key_value.DefaultValue ?? DbNullValue); // DN
            row.Add("filename", filename.DefaultValue ?? DbNullValue); // DN
            row.Add("request", _request.DefaultValue ?? DbNullValue); // DN
            return row;
        }

        #pragma warning disable 1998
        // Render row values based on field settings
        public async Task RenderRow()
        {
            SetupOtherOptions();

            // Call Row Rendering event
            RowRendering();

            // Common render codes for all row types

            // file_id

            // created_at

            // username

            // export_type

            // entity

            // key_value

            // filename

            // request

            // View row
            if (RowType == RowType.View) {
                // file_id
                file_id.ViewValue = ConvertToString(file_id.CurrentValue); // DN
                file_id.ViewCustomAttributes = "";

                // created_at
                created_at.ViewValue = created_at.CurrentValue;
                created_at.ViewValue = FormatDateTime(created_at.ViewValue, created_at.FormatPattern);
                created_at.ViewCustomAttributes = "";

                // username
                _username.ViewValue = ConvertToString(_username.CurrentValue); // DN
                _username.ViewCustomAttributes = "";

                // export_type
                export_type.ViewValue = ConvertToString(export_type.CurrentValue); // DN
                export_type.ViewCustomAttributes = "";

                // entity
                entity.ViewValue = ConvertToString(entity.CurrentValue); // DN
                entity.ViewCustomAttributes = "";

                // key_value
                key_value.ViewValue = ConvertToString(key_value.CurrentValue); // DN
                key_value.ViewCustomAttributes = "";

                // filename
                filename.ViewValue = ConvertToString(filename.CurrentValue); // DN
                filename.ViewCustomAttributes = "";

                // request
                _request.ViewValue = _request.CurrentValue;
                _request.ViewCustomAttributes = "";

                // file_id
                file_id.HrefValue = "";
                file_id.TooltipValue = "";

                // created_at
                created_at.HrefValue = "";
                created_at.TooltipValue = "";

                // username
                _username.HrefValue = "";
                _username.TooltipValue = "";

                // export_type
                export_type.HrefValue = "";
                export_type.TooltipValue = "";

                // entity
                entity.HrefValue = "";
                entity.TooltipValue = "";

                // key_value
                key_value.HrefValue = "";
                key_value.TooltipValue = "";

                // filename
                filename.HrefValue = "";
                filename.TooltipValue = "";

                // request
                _request.HrefValue = "";
                _request.TooltipValue = "";
            }

            // Call Row Rendered event
            if (RowType != RowType.AggregateInit)
                RowRendered();
        }
        #pragma warning restore 1998

        // Set up Breadcrumb
        protected void SetupBreadcrumb() {
            var breadcrumb = new Breadcrumb();
            string url = CurrentUrl();
            breadcrumb.Add("list", TableVar, AppPath(AddMasterUrl("exportlogslist")), "", TableVar, true);
            string pageId = "view";
            breadcrumb.Add("view", pageId, url);
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

        // Set up starting record parameters
        public void SetupStartRecord()
        {
            // Exit if DisplayRecords = 0
            if (DisplayRecords == 0)
                return;
            string pageNo = Get(Config.TablePageNumber);
            string startRec = Get(Config.TableStartRec);
            bool infiniteScroll = false;
            string recordNo = !Empty(pageNo) ? pageNo : startRec; // Record number = page number or start record
            if (!Empty(recordNo) && IsNumeric(recordNo))
                StartRecord = ConvertToInt(recordNo);
            else
                StartRecord = StartRecordNumber;

            // Check if correct start record counter
            if (StartRecord <= 0) // Avoid invalid start record counter
                StartRecord = 1; // Reset start record counter
            else if (StartRecord > TotalRecords) // Avoid starting record > total records
                StartRecord = ((TotalRecords - 1) / DisplayRecords) * DisplayRecords + 1; // Point to last page first record
            else if ((StartRecord - 1) % DisplayRecords != 0)
                StartRecord = ((StartRecord - 1) / DisplayRecords) * DisplayRecords + 1; // Point to page boundary
            if (!infiniteScroll)
                StartRecordNumber = StartRecord;
        }

        // Get page count
        public int PageCount
        {
            get {
                return ConvertToInt(Math.Ceiling((double)TotalRecords / DisplayRecords));
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

        // Page Exporting event
        // doc = export document object
        public virtual bool PageExporting(ref dynamic doc) {
            //doc.Text.Append("<p>" + "my header" + "</p>"); // Export header
            //return false; // Return false to skip default export and use Row_Export event
            return true; // Return true to use default export and skip Row_Export event
        }

        // Page Exported event
        // doc = export document object
        public virtual void PageExported(dynamic doc) {
            //doc.Text.Append("my footer"); // Export footer
            //Log("Text: {Text}", doc.Text.ToString());
        }
    } // End page class
} // End Partial class
