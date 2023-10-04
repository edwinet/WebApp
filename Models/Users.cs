namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// users
    /// </summary>
    [MaybeNull]
    public static Users users
    {
        get => HttpData.Resolve<Users>("users");
        set => HttpData["users"] = value;
    }

    /// <summary>
    /// Table class for users
    /// </summary>
    public class Users : DbTable, IQueryFactory
    {
        public int RowCount = 0; // DN

        public bool UseSessionForListSql = true;

        // Column CSS classes
        public string LeftColumnClass = "col-sm-2 col-form-label ew-label";

        public string RightColumnClass = "col-sm-10";

        public string OffsetColumnClass = "col-sm-10 offset-sm-2";

        public string TableLeftColumnClass = "w-col-2";

        // Audit trail
        public bool AuditTrailOnAdd = true;

        public bool AuditTrailOnEdit = true;

        public bool AuditTrailOnDelete = true;

        public bool AuditTrailOnView = false;

        public bool AuditTrailOnViewData = false;

        public bool AuditTrailOnSearch = false;

        // Ajax / Modal
        public bool UseAjaxActions = false;

        public bool ModalSearch = true;

        public bool ModalView = true;

        public bool ModalAdd = true;

        public bool ModalEdit = true;

        public bool ModalUpdate = false;

        public bool InlineDelete = true;

        public bool ModalGridAdd = false;

        public bool ModalGridEdit = false;

        public bool ModalMultiEdit = false;

        public readonly DbField<SqlDbType> id;

        public readonly DbField<SqlDbType> first_name;

        public readonly DbField<SqlDbType> last_name;

        public readonly DbField<SqlDbType> _email;

        public readonly DbField<SqlDbType> _username;

        public readonly DbField<SqlDbType> password;

        public readonly DbField<SqlDbType> mobile;

        public readonly DbField<SqlDbType> photo;

        public readonly DbField<SqlDbType> role_id;

        public readonly DbField<SqlDbType> provider;

        public readonly DbField<SqlDbType> activated;

        public readonly DbField<SqlDbType> _profile;

        public readonly DbField<SqlDbType> created_at;

        public readonly DbField<SqlDbType> updated_at;

        // Constructor
        public Users()
        {
            // Language object // DN
            Language = ResolveLanguage();
            TableVar = "users";
            Name = "users";
            Type = "TABLE";
            UpdateTable = "dbo.users"; // Update Table
            DbId = "DB"; // DN
            ExportAll = false;
            ExportPageBreakCount = 0; // Page break per every n record (PDF only)
            ExportPageOrientation = "portrait"; // Page orientation (PDF only)
            ExportPageSize = "a4"; // Page size (PDF only)
            ExportColumnWidths = new float[] {  }; // Column widths (PDF only) // DN
            ExportExcelPageOrientation = ""; // Page orientation (EPPlus only)
            ExportExcelPageSize = ""; // Page size (EPPlus only)
            ExportWordPageOrientation = ""; // Page orientation (Word only)
            DetailAdd = false; // Allow detail add
            DetailEdit = false; // Allow detail edit
            DetailView = false; // Allow detail view
            ShowMultipleDetails = false; // Show multiple details
            GridAddRowCount = 5;
            AllowAddDeleteRow = true; // Allow add/delete row
            UseAjaxActions = UseAjaxActions || Config.UseAjaxActions;

            // id
            id = new (this, "x_id", 3, SqlDbType.Int) {
                Name = "id",
                Expression = "[id]",
                BasicSearchExpression = "CAST([id] AS NVARCHAR)",
                DateTimeFormat = -1,
                VirtualExpression = "[id]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "NO",
                InputTextType = "text",
                IsAutoIncrement = true, // Autoincrement field
                IsPrimaryKey = true, // Primary key field
                Nullable = false, // NOT NULL field
                DefaultErrorMessage = Language.Phrase("IncorrectInteger"),
                SearchOperators = new () { "=", "<>", "IN", "NOT IN", "<", "<=", ">", ">=", "BETWEEN", "NOT BETWEEN" },
                CustomMessage = Language.FieldPhrase("users", "id", "CustomMsg"),
                IsUpload = false
            };
            Fields.Add("id", id);

            // first_name
            first_name = new (this, "x_first_name", 202, SqlDbType.NVarChar) {
                Name = "first_name",
                Expression = "[first_name]",
                UseBasicSearch = true,
                BasicSearchExpression = "[first_name]",
                DateTimeFormat = -1,
                VirtualExpression = "[first_name]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "TEXT",
                InputTextType = "text",
                SearchOperators = new () { "=", "<>", "IN", "NOT IN", "STARTS WITH", "NOT STARTS WITH", "LIKE", "NOT LIKE", "ENDS WITH", "NOT ENDS WITH", "IS EMPTY", "IS NOT EMPTY", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "first_name", "CustomMsg"),
                IsUpload = false
            };
            Fields.Add("first_name", first_name);

            // last_name
            last_name = new (this, "x_last_name", 202, SqlDbType.NVarChar) {
                Name = "last_name",
                Expression = "[last_name]",
                UseBasicSearch = true,
                BasicSearchExpression = "[last_name]",
                DateTimeFormat = -1,
                VirtualExpression = "[last_name]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "TEXT",
                InputTextType = "text",
                SearchOperators = new () { "=", "<>", "IN", "NOT IN", "STARTS WITH", "NOT STARTS WITH", "LIKE", "NOT LIKE", "ENDS WITH", "NOT ENDS WITH", "IS EMPTY", "IS NOT EMPTY", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "last_name", "CustomMsg"),
                IsUpload = false
            };
            Fields.Add("last_name", last_name);

            // email
            _email = new (this, "x__email", 200, SqlDbType.VarChar) {
                Name = "email",
                Expression = "[email]",
                UseBasicSearch = true,
                BasicSearchExpression = "[email]",
                DateTimeFormat = -1,
                VirtualExpression = "[email]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "TEXT",
                InputTextType = "text",
                DefaultErrorMessage = Language.Phrase("IncorrectEmail"),
                SearchOperators = new () { "=", "<>", "IN", "NOT IN", "STARTS WITH", "NOT STARTS WITH", "LIKE", "NOT LIKE", "ENDS WITH", "NOT ENDS WITH", "IS EMPTY", "IS NOT EMPTY", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "_email", "CustomMsg"),
                IsUpload = false
            };
            Fields.Add("email", _email);

            // username
            _username = new (this, "x__username", 202, SqlDbType.NVarChar) {
                Name = "username",
                Expression = "[username]",
                UseBasicSearch = true,
                BasicSearchExpression = "[username]",
                DateTimeFormat = -1,
                VirtualExpression = "[username]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "TEXT",
                InputTextType = "text",
                Required = true, // Required field
                SearchOperators = new () { "=", "<>", "IN", "NOT IN", "STARTS WITH", "NOT STARTS WITH", "LIKE", "NOT LIKE", "ENDS WITH", "NOT ENDS WITH", "IS EMPTY", "IS NOT EMPTY", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "_username", "CustomMsg"),
                IsUpload = false
            };
            Fields.Add("username", _username);

            // password
            password = new (this, "x_password", 202, SqlDbType.NVarChar) {
                Name = "password",
                Expression = "[password]",
                BasicSearchExpression = "[password]",
                DateTimeFormat = -1,
                VirtualExpression = "[password]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "PASSWORD",
                InputTextType = "text",
                Required = true, // Required field
                Sortable = false, // Allow sort
                SearchOperators = new () { "=", "<>", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "password", "CustomMsg"),
                IsUpload = false
            };
            if (Config.EncryptedPassword)
                password.Raw = true;
            Fields.Add("password", password);

            // mobile
            mobile = new (this, "x_mobile", 200, SqlDbType.VarChar) {
                Name = "mobile",
                Expression = "[mobile]",
                UseBasicSearch = true,
                BasicSearchExpression = "[mobile]",
                DateTimeFormat = -1,
                VirtualExpression = "[mobile]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "TEXT",
                InputTextType = "text",
                DefaultErrorMessage = Language.Phrase("IncorrectPhone"),
                SearchOperators = new () { "=", "<>", "IN", "NOT IN", "STARTS WITH", "NOT STARTS WITH", "LIKE", "NOT LIKE", "ENDS WITH", "NOT ENDS WITH", "IS EMPTY", "IS NOT EMPTY", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "mobile", "CustomMsg"),
                IsUpload = false
            };
            Fields.Add("mobile", mobile);

            // photo
            photo = new (this, "x_photo", 202, SqlDbType.NVarChar) {
                Name = "photo",
                Expression = "[photo]",
                UseBasicSearch = true,
                BasicSearchExpression = "[photo]",
                DateTimeFormat = -1,
                VirtualExpression = "[photo]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "IMAGE",
                HtmlTag = "FILE",
                InputTextType = "text",
                SearchOperators = new () { "=", "<>", "STARTS WITH", "NOT STARTS WITH", "LIKE", "NOT LIKE", "ENDS WITH", "NOT ENDS WITH", "IS EMPTY", "IS NOT EMPTY", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "photo", "CustomMsg"),
                IsUpload = true
            };
            Fields.Add("photo", photo);

            // role_id
            role_id = new (this, "x_role_id", 3, SqlDbType.Int) {
                Name = "role_id",
                Expression = "[role_id]",
                BasicSearchExpression = "CAST([role_id] AS NVARCHAR)",
                DateTimeFormat = -1,
                VirtualExpression = "[role_id]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "SELECT",
                InputTextType = "text",
                UsePleaseSelect = true, // Use PleaseSelect by default
                PleaseSelectText = Language.Phrase("PleaseSelect"), // PleaseSelect text
                DefaultErrorMessage = Language.Phrase("IncorrectInteger"),
                SearchOperators = new () { "=", "<>", "<", "<=", ">", ">=", "BETWEEN", "NOT BETWEEN", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "role_id", "CustomMsg"),
                IsUpload = false
            };
            role_id.Lookup = new Lookup<DbField>(role_id, "roles", false, "id", new List<string> {"role_name", "", "", ""}, "", "", new List<string> {}, new List<string> {}, new List<string> {}, new List<string> {}, new List<string> {}, new List<string> {}, "", "", "[role_name]");
            role_id.GetDefault = () => 0;
            Fields.Add("role_id", role_id);

            // provider
            provider = new (this, "x_provider", 200, SqlDbType.VarChar) {
                Name = "provider",
                Expression = "[provider]",
                BasicSearchExpression = "[provider]",
                DateTimeFormat = -1,
                VirtualExpression = "[provider]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "HIDDEN",
                InputTextType = "text",
                SearchOperators = new () { "=", "<>", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "provider", "CustomMsg"),
                IsUpload = false
            };
            Fields.Add("provider", provider);

            // activated
            activated = new (this, "x_activated", 202, SqlDbType.NVarChar) {
                Name = "activated",
                Expression = "[activated]",
                BasicSearchExpression = "[activated]",
                DateTimeFormat = -1,
                VirtualExpression = "[activated]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "HIDDEN",
                InputTextType = "text",
                SearchOperators = new () { "=", "<>", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "activated", "CustomMsg"),
                IsUpload = false
            };
            Fields.Add("activated", activated);

            // profile
            _profile = new (this, "x__profile", 203, SqlDbType.NText) {
                Name = "profile",
                Expression = "[profile]",
                BasicSearchExpression = "[profile]",
                DateTimeFormat = -1,
                VirtualExpression = "[profile]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "HIDDEN",
                InputTextType = "text",
                Sortable = false, // Allow sort
                SearchOperators = new () { "=", "<>", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "_profile", "CustomMsg"),
                IsUpload = false
            };
            Fields.Add("profile", _profile);

            // created_at
            created_at = new (this, "x_created_at", 135, SqlDbType.DateTime) {
                Name = "created_at",
                Expression = "[created_at]",
                BasicSearchExpression = CastDateFieldForLike("[created_at]", 11, "DB"),
                DateTimeFormat = 11,
                VirtualExpression = "[created_at]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "TEXT",
                InputTextType = "text",
                DefaultErrorMessage = ConvertToString(Language.Phrase("IncorrectDate")).Replace("%s", DateFormat(11)),
                SearchOperators = new () { "=", "<>", "IN", "NOT IN", "<", "<=", ">", ">=", "BETWEEN", "NOT BETWEEN", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "created_at", "CustomMsg"),
                IsUpload = false
            };
            created_at.GetAutoUpdateValue = () => CurrentDateTime();
            Fields.Add("created_at", created_at);

            // updated_at
            updated_at = new (this, "x_updated_at", 135, SqlDbType.DateTime) {
                Name = "updated_at",
                Expression = "[updated_at]",
                BasicSearchExpression = CastDateFieldForLike("[updated_at]", 11, "DB"),
                DateTimeFormat = 11,
                VirtualExpression = "[updated_at]",
                IsVirtual = false,
                ForceSelection = false,
                SelectMultiple = false,
                VirtualSearch = false,
                ViewTag = "FORMATTED TEXT",
                HtmlTag = "TEXT",
                InputTextType = "text",
                DefaultErrorMessage = ConvertToString(Language.Phrase("IncorrectDate")).Replace("%s", DateFormat(11)),
                SearchOperators = new () { "=", "<>", "IN", "NOT IN", "<", "<=", ">", ">=", "BETWEEN", "NOT BETWEEN", "IS NULL", "IS NOT NULL" },
                CustomMessage = Language.FieldPhrase("users", "updated_at", "CustomMsg"),
                IsUpload = false
            };
            updated_at.GetAutoUpdateValue = () => CurrentDateTime();
            Fields.Add("updated_at", updated_at);

            // Call Table Load event
            TableLoad();
        }

        // Set Field Visibility
        public override bool GetFieldVisibility(string fldname)
        {
            var fld = FieldByName(fldname);
            return fld?.Visible ?? false; // Returns original value
        }

        // Invoke method // DN
        public object? Invoke(string name, object?[]? parameters = null)
        {
            var mi = this.GetType().GetMethod(name);
            if (mi != null)
                return IsAsyncMethod(mi)
                    ? InvokeAsync(mi, parameters).GetAwaiter().GetResult()
                    : mi.Invoke(this, parameters);
            return null;
        }

        // Invoke async method // DN
        public async Task<object?> InvokeAsync(MethodInfo? mi, object?[]? parameters = null)
        {
            if (mi != null) {
                dynamic? awaitable = mi.Invoke(this, parameters);
                if (awaitable != null) {
                    await awaitable;
                    return awaitable.GetAwaiter().GetResult();
                }
            }
            return null;
        }

        #pragma warning disable 1998
        // Invoke async method // DN
        public async Task<object> InvokeAsync(string name, object[]? parameters = null) => InvokeAsync(this.GetType().GetMethod(name), parameters);
        #pragma warning restore 1998

        // Check if Invoke async method // DN
        public bool IsAsyncMethod(MethodInfo? mi)
        {
            if (mi != null) {
                Type attType = typeof(AsyncStateMachineAttribute);
                return mi.GetCustomAttribute(attType) is AsyncStateMachineAttribute;
            }
            return false;
        }

        // Check if Invoke async method // DN
        public bool IsAsyncMethod(string name) => IsAsyncMethod(this.GetType().GetMethod(name));

        #pragma warning disable 618
        // Connection
        public virtual DatabaseConnectionBase<SqlConnection, SqlCommand, SqlDataReader, SqlDbType> Connection => GetConnection(DbId);
        #pragma warning restore 618

        // Get query factory
        public QueryFactory GetQueryFactory(bool main) => Connection.GetQueryFactory(main);

        // Get query builder
        public QueryBuilder GetQueryBuilder(bool main, string output = "")
        {
            var qf = GetQueryFactory(main);
            var qb = (XQuery)qf.Query(UpdateTable);
            if (output != "")
                qb.Compiler = Connection.GetCompiler(output); // Replace the compiler
            return qb;
        }

        // Get query builder with output Id (use secondary connection)
        public QueryBuilder GetQueryBuilder(string output) => GetQueryBuilder(false, output);

        // Get query builder without output Id (use secondary connection)
        public QueryBuilder GetQueryBuilder() => GetQueryBuilder(false);

        // Set left column class (must be predefined col-*-* classes of Bootstrap grid system)
        public void SetLeftColumnClass(string columnClass)
        {
            Match m = Regex.Match(columnClass, @"^col\-(\w+)\-(\d+)$");
            if (m.Success) {
                LeftColumnClass = columnClass + " col-form-label ew-label";
                RightColumnClass = "col-" + m.Groups[1].Value + "-" + ConvertToString(12 - ConvertToInt(m.Groups[2].Value));
                OffsetColumnClass = RightColumnClass + " " + columnClass.Replace("col-", "offset-");
                TableLeftColumnClass = Regex.Replace(columnClass, @"/^col-\w+-(\d+)$/", "w-col-$1"); // Change to w-col-*
            }
        }

        // Single column sort
        public void UpdateSort(DbField fld)
        {
            string lastSort, sortField, curSort, orderBy;
            if (CurrentOrder == fld.Name) {
                sortField = fld.Expression;
                lastSort = fld.Sort;
                if ((new[] { "ASC", "DESC", "NO" }).Contains(CurrentOrderType)) {
                    curSort = CurrentOrderType;
                } else {
                    curSort = lastSort;
                }
                orderBy = (new[] { "ASC", "DESC" }).Contains(curSort) ? sortField + " " + curSort : "";
                SessionOrderBy = orderBy; // Save to Session
            }
        }

        // Update field sort
        public void UpdateFieldSort()
        {
            string orderBy = SessionOrderBy; // Get ORDER BY from Session
            var flds = GetSortFields(orderBy);
            foreach (var (key, field) in Fields) {
                string fldSort = "";
                foreach (var fld in flds) {
                    if (fld[0] == field.Expression || fld[0] == field.VirtualExpression)
                        fldSort = fld[1];
                }
                field.Sort = fldSort;
            }
        }

        #pragma warning disable 1998
        // Render X Axis for chart
        public async Task<Dictionary<string, object>> RenderChartXAxis(string chartVar, Dictionary<string, object> chartRow)
        {
            return chartRow;
        }
        #pragma warning restore 1998

        // Table level SQL
        // FROM
        private string? _sqlFrom = null;

        public string SqlFrom
        {
            get => _sqlFrom ?? "dbo.users";
            set => _sqlFrom = value;
        }

        // SELECT
        private string? _sqlSelect = null;

        public string SqlSelect { // Select
            get => _sqlSelect ?? "SELECT * FROM " + SqlFrom;
            set => _sqlSelect = value;
        }

        // WHERE // DN
        private string? _sqlWhere = null;

        public string SqlWhere
        {
            get {
                string where = "";
                return _sqlWhere ?? where;
            }
            set {
                _sqlWhere = value;
            }
        }

        // Group By
        private string? _sqlGroupBy = null;

        public string SqlGroupBy
        {
            get => _sqlGroupBy ?? "";
            set => _sqlGroupBy = value;
        }

        // Having
        private string? _sqlHaving = null;

        public string SqlHaving
        {
            get => _sqlHaving ?? "";
            set => _sqlHaving = value;
        }

        // Order By
        private string? _sqlOrderBy = null;

        public string SqlOrderBy
        {
            get => _sqlOrderBy ?? "";
            set => _sqlOrderBy = value;
        }

        // Apply User ID filters
        public string ApplyUserIDFilters(string filter, string id = "")
        {
            if (!Empty(Security.CurrentUserID) && !Security.IsAdmin) { // Non system admin
                filter = AddUserIDFilter(filter, id);
            }
            return filter;
        }

        // Check if User ID security allows view all
        public bool UserIDAllow(string id = "")
        {
            int allow = UserIdAllowSecurity;
            return id switch {
                "add" => ((allow & 1) == 1),
                "copy" => ((allow & 1) == 1),
                "gridadd" => ((allow & 1) == 1),
                "register" => ((allow & 1) == 1),
                "addopt" => ((allow & 1) == 1),
                "edit" => ((allow & 4) == 4),
                "gridedit" => ((allow & 4) == 4),
                "update" => ((allow & 4) == 4),
                "changepassword" => ((allow & 4) == 4),
                "resetpassword" => ((allow & 4) == 4),
                "delete" => ((allow & 2) == 2),
                "view" => ((allow & 32) == 32),
                "search" => ((allow & 64) == 64),
                "lookup" => ((allow & 256) == 256),
                _ => ((allow & 8) == 8)
            };
        }

        /// <summary>
        /// Get record count by reading data reader (Async) // DN
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="c">Connection</param>
        /// <returns>Record count</returns>
        public async Task<int> GetRecordCountAsync(string sql, dynamic? c = null) {
            try {
                var cnt = 0;
                var conn = c ?? Connection;
                using var dr = await conn.OpenDataReaderAsync(sql);
                if (dr != null) {
                    while (await dr.ReadAsync())
                        cnt++;
                }
                return cnt;
            } catch {
                if (Config.Debug)
                    throw;
                return -1;
            }
        }

        /// <summary>
        /// Get record count by reading data reader // DN
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="c">Connection</param>
        /// <returns>Record count</returns>
        public int GetRecordCount(string sql, dynamic? c = null) => GetRecordCountAsync(sql, c).GetAwaiter().GetResult();

        /// <summary>
        /// Try to get record count by SELECT COUNT(*) (Async) // DN
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="c">Connection</param>
        /// <returns>Record count</returns>
        public async Task<int> TryGetRecordCountAsync(string sql, dynamic? c = null)
        {
            string orderBy = OrderBy;
            var conn = c ?? Connection;
            sql = Regex.Replace(sql, @"/\*BeginOrderBy\*/[\s\S]+/\*EndOrderBy\*/", "", RegexOptions.IgnoreCase).Trim(); // Remove ORDER BY clause (MSSQL)
            if (!Empty(orderBy) && sql.EndsWith(orderBy))
                sql = sql.Substring(0, sql.Length - orderBy.Length); // Remove ORDER BY clause
            try {
                string sqlcnt;
                if ((new[] { "TABLE", "VIEW", "LINKTABLE" }).Contains(Type) && sql.StartsWith(SqlSelect)) { // Handle Custom Field
                    sqlcnt = "SELECT COUNT(*) FROM " + SqlFrom + sql.Substring(SqlSelect.Length);
                } else {
                    sqlcnt = "SELECT COUNT(*) FROM (" + sql + ") EW_COUNT_TABLE";
                }
                return Convert.ToInt32(await conn?.ExecuteScalarAsync(sqlcnt));
            } catch {
                return await GetRecordCountAsync(sql, conn);
            }
        }

        /// <summary>
        /// Try to get record count by SELECT COUNT(*) // DN
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="c">Connection</param>
        /// <returns>Record count</returns>
        public int TryGetRecordCount(string sql, dynamic? c = null) => TryGetRecordCountAsync(sql, c).GetAwaiter().GetResult();

        // Get SQL
        public string GetSql(string where, string orderBy = "") => BuildSelectSql(SqlSelect, SqlWhere, SqlGroupBy, SqlHaving, SqlOrderBy, where, orderBy);

        // Table SQL
        public string CurrentSql
        {
            get {
                string filter = CurrentFilter;
                filter = ApplyUserIDFilters(filter); // Add User ID filter
                string sort = SessionOrderBy;
                return GetSql(filter, sort);
            }
        }

        // Table SQL with List page filter
        public string ListSql
        {
            get {
                string sort = "";
                string select = "";
                string filter = UseSessionForListSql ? SessionWhere : "";
                AddFilter(ref filter, CurrentFilter);
                RecordsetSelecting(ref filter);
                filter = ApplyUserIDFilters(filter); // Add User ID filter
                select = SqlSelect;
                sort = UseSessionForListSql ? SessionOrderBy : "";
                return BuildSelectSql(select, SqlWhere, SqlGroupBy, SqlHaving, SqlOrderBy, filter, sort);
            }
        }

        // Get ORDER BY clause
        public string OrderBy
        {
            get {
                string sort = SessionOrderBy;
                return BuildSelectSql("", "", "", "", SqlOrderBy, "", sort);
            }
        }

        /// <summary>
        /// Get record count based on filter (for detail record count in master table pages) (Async) // DN
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Record count</returns>
        public async Task<int> LoadRecordCountAsync(string filter) => await TryGetRecordCountAsync(GetSql(filter));

        /// <summary>
        /// Get record count based on filter (for detail record count in master table pages) // DN
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Record count</returns>
        public int LoadRecordCount(string filter) => TryGetRecordCount(GetSql(filter));

        /// <summary>
        /// Get record count (for current List page) (Async) // DN
        /// </summary>
        /// <returns>Record count</returns>
        public async Task<int> ListRecordCountAsync() => await TryGetRecordCountAsync(ListSql);

        /// <summary>
        /// Get record count (for current List page) // DN
        /// </summary>
        /// <returns>Record count</returns>
        public int ListRecordCount() => TryGetRecordCount(ListSql);

        /// <summary>
        /// Insert (Async)
        /// </summary>
        /// <param name="data">Data to be inserted (IDictionary|Anonymous)</param>
        /// <returns>Row affected</returns>
        public async Task<int> InsertAsync(object data)
        {
            int result = 0;
            IDictionary<string, object> row;
            if (data is IDictionary<string, object> d)
                row = d;
            else if (IsAnonymousType(data))
                row = ConvertToDictionary<object>(data);
            else
                throw new ArgumentException("Data must be of anonymous type or Dictionary<string, object> type", "data");
            row = row.Where(kvp => {
                var fld = FieldByName(kvp.Key);
                return fld != null && !fld.IsCustom;
            }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            if (row.Count == 0)
                return -1;
            var queryBuilder = GetQueryBuilder();
            try {
                var lastInsertedId = await queryBuilder.InsertGetIdAsync<int>(row);
                id.SetDbValue(lastInsertedId);
                result = 1;
            } catch (Exception e) {
                CurrentPage?.SetFailureMessage(e.Message);
                if (Config.Debug)
                    throw;
            }
            if (result > 0 && AuditTrailOnAdd)
                await WriteAuditTrailOnAdd(row);
            return result;
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="data">Data to be inserted (IDictionary|Anonymous)</param>
        /// <returns>Row affected</returns>
        public int Insert(object data) => InsertAsync(data).GetAwaiter().GetResult();

        /// <summary>
        /// Update (Async)
        /// </summary>
        /// <param name="data">Data to be updated (IDictionary|Anonymous)</param>
        /// <returns>Row affected</returns>
        public async Task<int> UpdateAsync(object data)
        {
            IDictionary<string, object> row;
            if (data is IDictionary<string, object> d)
                row = d;
            else if (IsAnonymousType(data))
                row = ConvertToDictionary<object>(data);
            else
                throw new ArgumentException("Data must be of anonymous type or Dictionary<string, object> type", "data");
            var where = GetRowFilterAsDictionary(row);
            return where != null ? await UpdateAsync(row, where) : -1; // Prevent update all record
        }

        /// <summary>
        /// Update (Async)
        /// </summary>
        /// <param name="data">Data to be updated (IDictionary|Anonymous)</param>
        /// <param name="where">Where (IDictionary|Anonymous|string)</param>
        /// <returns>Row affected</returns>
        public async Task<int> UpdateAsync(object data, object? where) => await UpdateAsync(data, where, null);

        #pragma warning disable 168, 219
        /// <summary>
        /// Update (Async)
        /// </summary>
        /// <param name="data">Data to be updated (IDictionary|Anonymous)</param>
        /// <param name="where">Where (IDictionary|Anonymous)</param>
        /// <param name="rsold">Old record</param>
        /// <returns>Row affected</returns>
        public async Task<int> UpdateAsync(object data, object? where, Dictionary<string, object>? rsold)
        {
            int result = -1;
            IDictionary<string, object> row;
            if (data is IDictionary<string, object> d)
                row = d;
            else if (IsAnonymousType(data))
                row = ConvertToDictionary<object>(data);
            else
                throw new ArgumentException("Data must be of anonymous type or Dictionary<string, object> type", "data");
            Dictionary<string, object> rscascade = new ();
            row = row.Where(kvp => FieldByName(kvp.Key) is DbField fld && !fld.IsCustom).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            if (row.Count == 0)
                return -1;
            var queryBuilder = GetQueryBuilder();
            string filter = CurrentFilter;
            if (!Empty(filter))
                queryBuilder.WhereRaw(filter);
            if (IsAnonymousType(where))
                queryBuilder.Where(where);
            else if (where is IDictionary<string, object> dict)
                queryBuilder.Where(dict);
            else if (where is string)
                queryBuilder.WhereRaw((string)where);
            if (rsold != null && GetRowFilterAsDictionary(rsold) is IDictionary<string, object> rsoldFilter) // Add filter from old record
                queryBuilder.Where(rsoldFilter);
            if (queryBuilder.HasComponent("where")) // Prevent update all records
                result = await queryBuilder.UpdateAsync(row);
            if (result > 0 && AuditTrailOnEdit && rsold != null) {
                var rsaudit = new Dictionary<string, object>(row);
                if (!rsaudit.ContainsKey("id"))
                    rsaudit.Add("id", rsold["id"]);
                await WriteAuditTrailOnEdit(rsold, rsaudit);
            }
            return result;
        }
        #pragma warning restore 168, 219

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="data">Data to be updated (IDictionary|Anonymous)</param>
        /// <returns>Row affected</returns>
        public int Update(object data) => UpdateAsync(data).GetAwaiter().GetResult();

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="data">Data to be updated (IDictionary|Anonymous)</param>
        /// <param name="where">Where (IDictionary|Anonymous|string)</param>
        /// <returns>Row affected</returns>
        public int Update(object data, object where) => UpdateAsync(data, where).GetAwaiter().GetResult();

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="data">Data to be updated (IDictionary|Anonymous)</param>
        /// <param name="where">Where (IDictionary|Anonymous|string)</param>
        /// <param name="rsold">Old record</param>
        /// <returns>Row affected</returns>
        public int Update(object data, object where, Dictionary<string, object> rsold) => UpdateAsync(data, where, rsold).GetAwaiter().GetResult();

        #pragma warning disable 168, 1998
        /// <summary>
        /// Delete (Async)
        /// </summary>
        /// <param name="data">Data to be removed (IDictionary|Anonymous)</param>
        /// <param name="where">Where (IDictionary|Anonymous|string)</param>
        /// <returns>Row affected</returns>
        public async Task<int> DeleteAsync(object? data, object? where = null)
        {
            bool delete = true;
            IDictionary<string, object>? row = null;
            if (data is IDictionary<string, object> d)
                row = d;
            else if (IsAnonymousType(data))
                row = ConvertToDictionary<object>(data);
            var queryBuilder = GetQueryBuilder(true); // Use main connection
            if (GetRowFilterAsDictionary(row) is IDictionary<string, object> rowFilter)
                queryBuilder.Where(rowFilter);
            if (IsAnonymousType(where))
                queryBuilder.Where(where);
            else if (where is IDictionary<string, object> dict)
                queryBuilder.Where(dict);
            else if (where is string)
                queryBuilder.WhereRaw((string)where);
            int result = delete && queryBuilder.HasComponent("where") // Avoid delete if no WHERE clause
                ? await queryBuilder.DeleteAsync(Connection.Transaction)
                : -1;
            if (result > 0 && AuditTrailOnDelete && row != null)
                await WriteAuditTrailOnDelete(row);
            return result;
        }
        #pragma warning restore 168, 1998

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="data">Data to be removed (IDictionary|Anonymous)</param>
        /// <param name="where">Where (IDictionary|Anonymous|string)</param>
        /// <returns>Row affected</returns>
        public int Delete(object data, object? where = null) => DeleteAsync(data, where).GetAwaiter().GetResult();

        // Load DbValue from recordset
        public void LoadDbValues(Dictionary<string, object>? row)
        {
            if (row == null)
                return;
            try {
                id.DbValue = row["id"]; // Set DB value only
                first_name.DbValue = row["first_name"]; // Set DB value only
                last_name.DbValue = row["last_name"]; // Set DB value only
                _email.DbValue = row["email"]; // Set DB value only
                _username.DbValue = row["username"]; // Set DB value only
                password.DbValue = row["password"]; // Set DB value only
                mobile.DbValue = row["mobile"]; // Set DB value only
                photo.Upload.DbValue = row["photo"];
                role_id.DbValue = row["role_id"]; // Set DB value only
                provider.DbValue = row["provider"]; // Set DB value only
                activated.DbValue = row["activated"]; // Set DB value only
                _profile.DbValue = row["profile"]; // Set DB value only
                created_at.DbValue = row["created_at"]; // Set DB value only
                updated_at.DbValue = row["updated_at"]; // Set DB value only
            } catch {}
        }

        public void DeleteUploadedFiles(Dictionary<string, object> row)
        {
            LoadDbValues(row);
            if (!Empty(row["photo"])) {
                DeleteFile(photo.OldPhysicalUploadPath + row["photo"]);
            }
        }

        // Record filter WHERE clause
        private string _sqlKeyFilter => "[id] = @id@";

        #pragma warning disable 168, 219
        // Get record filter as string
        public string GetRecordFilter(Dictionary<string, object>? row = null, bool current = false)
        {
            string keyFilter = _sqlKeyFilter;
            object? val = null, result = "";
            val = row?.TryGetValue("id", out result) ?? false
                ? result
                : !Empty(id.OldValue) && !current ? id.OldValue : id.CurrentValue;
            if (!IsNumeric(val))
                return "0=1"; // Invalid key
            if (val == null)
                return "0=1"; // Invalid key
            keyFilter = keyFilter.Replace("@id@", AdjustSql(val, DbId)); // Replace key value
            return keyFilter;
        }

        // Get record filter as Dictionary // DN
        public Dictionary<string, object>? GetRowFilterAsDictionary(IDictionary<string, object>? row = null)
        {
            Dictionary<string, object>? keyFilter = new ();
            object? val = null, result;
            val = row?.TryGetValue("id", out result) ?? false
                ? result
                : !Empty(id.OldValue) ? id.OldValue : id.CurrentValue;
            if (!IsNumeric(val))
                return null; // Invalid key
            if (Empty(val))
                return null; // Invalid key
            keyFilter.Add("id", val); // Add key value
            return keyFilter.Count > 0 ? keyFilter : null;
        }
        #pragma warning restore 168, 219

        // Return URL
        public string ReturnUrl
        {
            get {
                string name = Config.ProjectName + "_" + TableVar + "_" + Config.TableReturnUrl;
                // Get referer URL automatically
                if (!Empty(ReferUrl()) && !SameText(ReferPage(), CurrentPageName()) &&
                    !SameText(ReferPage(), "login")) {// Referer not same page or login page
                        Session[name] = ReferUrl(); // Save to Session
                }
                if (!Empty(Session[name])) {
                    return Session.GetString(name);
                } else {
                    return "userslist";
                }
            }
            set {
                Session[Config.ProjectName + "_" + TableVar + "_" + Config.TableReturnUrl] = value;
            }
        }

        // Get modal caption
        public string GetModalCaption(string pageName)
        {
            if (SameString(pageName, "usersview"))
                return Language.Phrase("View");
            else if (SameString(pageName, "usersedit"))
                return Language.Phrase("Edit");
            else if (SameString(pageName, "usersadd"))
                return Language.Phrase("Add");
            else
                return "";
        }

        // Default route URL
        public string DefaultRouteUrl
        {
            get {
                return "userslist";
            }
        }

        // API page name
        public string GetApiPageName(string action)
        {
            return action.ToLowerInvariant() switch {
                Config.ApiViewAction => "UsersView",
                Config.ApiAddAction => "UsersAdd",
                Config.ApiEditAction => "UsersEdit",
                Config.ApiDeleteAction => "UsersDelete",
                Config.ApiListAction => "UsersList",
                _ => String.Empty
            };
        }

        // Current URL
        public string GetCurrentUrl(string parm = "")
        {
            string url = CurrentPageName();
            if (!Empty(parm))
                url = KeyUrl(url, parm);
            else
                url = KeyUrl(url, Config.TableShowDetail + "=");
            return AddMasterUrl(url);
        }

        // List URL
        public string ListUrl => "userslist";

        // View URL
        public string ViewUrl => GetViewUrl();

        // View URL
        public string GetViewUrl(string parm = "")
        {
            string url = "";
            if (!Empty(parm))
                url = KeyUrl("usersview", parm);
            else
                url = KeyUrl("usersview", Config.TableShowDetail + "=");
            return AddMasterUrl(url);
        }

        // Add URL
        public string AddUrl { get; set; } = "usersadd";

        // Add URL
        public string GetAddUrl(string parm = "")
        {
            string url = "";
            if (!Empty(parm))
                url = "usersadd?" + parm;
            else
                url = "usersadd";
            return AppPath(AddMasterUrl(url));
        }

        // Edit URL
        public string EditUrl => GetEditUrl();

        // Edit URL (with parameter)
        public string GetEditUrl(string parm = "")
        {
            string url = "";
            url = KeyUrl("usersedit", parm);
            return AppPath(AddMasterUrl(url)); // DN
        }

        // Inline edit URL
        public string InlineEditUrl =>
            AppPath(AddMasterUrl(KeyUrl("userslist", "action=edit"))); // DN

        // Copy URL
        public string CopyUrl => GetCopyUrl();

        // Copy URL
        public string GetCopyUrl(string parm = "")
        {
            string url = "";
            url = KeyUrl("usersadd", parm);
            return AppPath(AddMasterUrl(url)); // DN
        }

        // Inline copy URL
        public string InlineCopyUrl =>
            AppPath(AddMasterUrl(KeyUrl("userslist", "action=copy"))); // DN

        // Delete URL
        public string DeleteUrl => UseAjaxActions && Param<bool>("infinitescroll") && CurrentPageID() == "list"
            ? AppPath(KeyUrl(Config.ApiUrl + Config.ApiDeleteAction + "/" + TableVar))
            : AppPath(KeyUrl("usersdelete")); // DN

        // Add master URL
        public string AddMasterUrl(string url)
        {
            return url;
        }

        // Get primary key as JSON
        public string KeyToJson(bool htmlEncode = false)
        {
            string json = "";
            json += "\"id\":" + ConvertToJson(id.CurrentValue, "number", true);
            json = "{" + json + "}";
            if (htmlEncode)
                json = HtmlEncode(json);
            return json;
        }

        // Add key value to URL
        public string KeyUrl(string url, string parm = "") { // DN
            if (!IsNull(id.CurrentValue)) {
                url += "/" + id.CurrentValue;
            } else {
                return "javascript:ew.alert(ew.language.phrase('InvalidRecord'));";
            }
            if (Empty(parm))
                return url;
            else
                return url + "?" + parm;
        }

        // Render sort
        public string RenderFieldHeader(DbField fld)
        {
            string sortUrl = "";
            string attrs = "";
            if (fld.Sortable) {
                sortUrl = SortUrl(fld);
                attrs = " role=\"button\" data-ew-action=\"sort\" data-ajax=\"" + (UseAjaxActions ? "true" : "false") + "\" data-sort-url=\"" + sortUrl + "\" data-sort-type=\"1\"";
                if (!Empty(ContextClass)) // Add context
                    attrs += " data-context=\"" + HtmlEncode(ContextClass) + "\"";
            }
            string html = "<div class=\"ew-table-header-caption\"" + attrs + ">" + fld.Caption + "</div>";
            if (!Empty(sortUrl)) {
                html += "<div class=\"ew-table-header-sort\">" + fld.SortIcon + "</div>";
            }
            if (!IsExport() && fld.UseFilter && Security.CanSearch) {
                html += "<div class=\"ew-filter-dropdown-btn\" data-ew-action=\"filter\" data-table=\"" + fld.TableVar + "\" data-field=\"" + fld.FieldVar +
                    "\"><div class=\"ew-table-header-filter\" role=\"button\" aria-haspopup=\"true\">" + Language.Phrase("Filter") + "</div></div>";
            }
            html = "<div class=\"ew-table-header-btn\">" + html + "</div>";
            if (UseCustomTemplate) {
                string scriptId = ("tpc_{id}").Replace("{id}", fld.TableVar + "_" + fld.Param);
                html = "<template id=\"" + scriptId + "\">" + html + "</template>";
            }
            return html;
        }

        // Sort URL (already URL-encoded)
        public string SortUrl(DbField fld)
        {
            if (!Empty(CurrentAction) || !Empty(Export) ||
                (new[] {141, 201, 203, 128, 204, 205}).Contains(fld.Type)) { // Unsortable data type
                return "";
            } else if (fld.Sortable) {
                string urlParm = "order=" + UrlEncode(fld.Name) + "&amp;ordertype=" + fld.NextSort;
                if (DashboardReport)
                    urlParm += "&amp;" + Config.PageDashboard + "=true";
                return AddMasterUrl(CurrentDashboardPageUrl() + "?" + urlParm);
            }
            return "";
        }

        #pragma warning disable 168, 219
        // Get key as string
        public string GetKey(bool current = false)
        {
            List<string> keys = new ();
            string val;
            val = current ? ConvertToString(id.CurrentValue) ?? "" : ConvertToString(id.OldValue) ?? "";
            if (Empty(val))
                return String.Empty;
            keys.Add(val);
            return String.Join(Config.CompositeKeySeparator, keys);
        }

        // Get record filter as string // DN
        public string GetKey(IDictionary<string, object> row)
        {
            List<string> keys = new ();
            object? val = null, result;
            val = row?.TryGetValue("id", out result) ?? false ? ConvertToString(result) : null;
            if (Empty(val))
                return String.Empty; // Invalid key
            keys.Add(ConvertToString(val)); // Add key value
            return String.Join(Config.CompositeKeySeparator, keys);
        }
        #pragma warning restore 168, 219

        // Set key
        public void SetKey(string key, bool current = false)
        {
            OldKey = key;
            string[] keys = OldKey.Split(Convert.ToChar(Config.CompositeKeySeparator));
            if (keys.Length == 1) {
                if (current) {
                    id.CurrentValue = keys[0];
                } else {
                    id.OldValue = keys[0];
                }
            }
        }

        #pragma warning disable 168
        // Get record keys
        public List<string> GetRecordKeys()
        {
            List<string> result = new ();
            StringValues sv;
            List<string> keysList = new ();
            if (Post("key_m[]", out sv) || Get("key_m[]", out sv)) { // DN
                keysList = ((StringValues)sv).Select(k => ConvertToString(k)).ToList();
            } else if (RouteValues.Count > 0 || Query.Count > 0 || Form.Count > 0) { // DN
                string key = "";
                string[] keyValues = {};
                if (IsApi() && RouteValues.TryGetValue("key", out object? k)) {
                    string str = ConvertToString(k);
                    keyValues = str.Split('/');
                }
                if (RouteValues.TryGetValue("id", out object? v0)) { // id // DN
                    key = UrlDecode(v0); // DN
                } else if (IsApi() && !Empty(keyValues)) {
                    key = keyValues[0];
                } else {
                    key = Param("id");
                }
                keysList.Add(key);
            }
            // Check keys
            foreach (var keys in keysList) {
                if (!IsNumeric(keys)) // id
                    continue;
                result.Add(keys);
            }
            return result;
        }
        #pragma warning restore 168

        // Get filter from records
        public string GetFilterFromRecords(IEnumerable<Dictionary<string, object>> rows) =>
            String.Join(" OR ", rows.Select(row => "(" + GetRecordFilter(row) + ")"));

        // Get filter from record keys
        public string GetFilterFromRecordKeys(bool setCurrent = true)
        {
            List<string> recordKeys = GetRecordKeys();
            string keyFilter = "";
            foreach (var keys in recordKeys) {
                if (!Empty(keyFilter))
                    keyFilter += " OR ";
                if (setCurrent)
                    id.CurrentValue = keys;
                else
                    id.OldValue = keys;
                keyFilter += "(" + GetRecordFilter() + ")";
            }
            return keyFilter;
        }

        #pragma warning disable 618
        // Load rows based on filter // DN
        public async Task<DbDataReader?> LoadReader(string filter, string sort = "", DatabaseConnectionBase<SqlConnection, SqlCommand, SqlDataReader, SqlDbType>? conn = null)
        {
            // Set up filter (SQL WHERE clause) and get return SQL
            string sql = GetSql(filter, sort);
            try {
                var dr = await (conn ?? Connection).OpenDataReaderAsync(sql);
                if (dr?.HasRows ?? false)
                    return dr;
                else
                    dr?.Close(); // Close unused data reader // DN
            } catch {}
            return null;
        }
        #pragma warning restore 618

        // Load row values from recordset
        public void LoadListRowValues(DbDataReader? dr)
        {
            if (dr == null)
                return;
            id.SetDbValue(dr["id"]);
            first_name.SetDbValue(dr["first_name"]);
            last_name.SetDbValue(dr["last_name"]);
            _email.SetDbValue(dr["email"]);
            _username.SetDbValue(dr["username"]);
            password.SetDbValue(dr["password"]);
            mobile.SetDbValue(dr["mobile"]);
            photo.Upload.DbValue = dr["photo"];
            photo.SetDbValue(photo.Upload.DbValue);
            role_id.SetDbValue(dr["role_id"]);
            provider.SetDbValue(dr["provider"]);
            activated.SetDbValue(dr["activated"]);
            _profile.SetDbValue(dr["profile"]);
            created_at.SetDbValue(dr["created_at"]);
            updated_at.SetDbValue(dr["updated_at"]);
        }

        // Render list content
        public async Task<string> RenderListContent(string filter)
        {
            string pageName = "UsersList";
            dynamic? page = CreateInstance(pageName, new object[] { Controller }); // DN
            if (page != null) {
                page.UseLayout = false; // DN
                await page.LoadRecordsetFromFilter(filter);
                string html = await GetViewOutput(pageName, page, false);
                page.Terminate(); // Terminate page and clean up
                return html;
            }
            return "";
        }

        #pragma warning disable 1998
        // Render list row values
        public async Task RenderListRow()
        {
            // Call Row Rendering event
            RowRendering();

            // Common render codes

            // id

            // first_name

            // last_name

            // email

            // username

            // password
            password.CellCssStyle = "white-space: nowrap;";

            // mobile

            // photo

            // role_id

            // provider

            // activated

            // profile

            // created_at

            // updated_at

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

            // profile
            _profile.ViewValue = _profile.CurrentValue;
            _profile.ViewCustomAttributes = "";

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

            // password
            password.HrefValue = "";
            password.TooltipValue = "";

            // mobile
            mobile.HrefValue = "";
            mobile.TooltipValue = "";

            // photo
            if (!IsNull(photo.Upload.DbValue)) {
                photo.HrefValue = ConvertToString(GetFileUploadUrl(photo, photo.HtmlDecode(ConvertToString(photo.Upload.DbValue)))); // Add prefix/suffix
                photo.LinkAttrs["target"] = ""; // Add target
                if (IsExport())
                    photo.HrefValue = FullUrl(ConvertToString(photo.HrefValue), "href");
            } else {
                photo.HrefValue = "";
            }
            photo.ExportHrefValue = photo.UploadPath + photo.Upload.DbValue;
            photo.TooltipValue = "";
            if (photo.UseColorbox) {
                if (Empty(photo.TooltipValue))
                    photo.LinkAttrs["title"] = Language.Phrase("ViewImageGallery");
                photo.LinkAttrs["data-rel"] = "users_x_photo";
                if (photo.LinkAttrs.ContainsKey("class")) {
                    photo.LinkAttrs.AppendClass("ew-lightbox");
                } else {
                    photo.LinkAttrs["class"] = "ew-lightbox";
                }
            }

            // role_id
            role_id.HrefValue = "";
            role_id.TooltipValue = "";

            // provider
            provider.HrefValue = "";
            provider.TooltipValue = "";

            // activated
            activated.HrefValue = "";
            activated.TooltipValue = "";

            // profile
            _profile.HrefValue = "";
            _profile.TooltipValue = "";

            // created_at
            created_at.HrefValue = "";
            created_at.TooltipValue = "";

            // updated_at
            updated_at.HrefValue = "";
            updated_at.TooltipValue = "";

            // Call Row Rendered event
            RowRendered();

            // Save data for Custom Template
            Rows.Add(CustomTemplateFieldValues());
        }
        #pragma warning restore 1998

        #pragma warning disable 1998
        // Render edit row values
        public async Task RenderEditRow()
        {
            // Call Row Rendering event
            RowRendering();

            // id
            id.SetupEditAttributes();
            id.EditValue = id.CurrentValue;
            id.EditValue = FormatNumber(id.EditValue, id.FormatPattern);
            id.ViewCustomAttributes = "";

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

            // photo
            photo.SetupEditAttributes();
            if (!IsNull(photo.Upload.DbValue)) {
                photo.ImageWidth = 80;
                photo.ImageHeight = 0;
                photo.ImageAlt = photo.Alt;
                photo.ImageCssClass = "ew-image";
                photo.EditValue = photo.Upload.DbValue;
            } else {
                photo.EditValue = "";
            }
            if (!Empty(photo.CurrentValue))
                    photo.Upload.FileName = ConvertToString(photo.CurrentValue);

            // role_id
            role_id.SetupEditAttributes();
            if (!Security.CanAdmin) { // System admin
                role_id.EditValue = Language.Phrase("PasswordMask");
            } else {
                role_id.PlaceHolder = RemoveHtml(role_id.Caption);
                if (!Empty(role_id.EditValue) && IsNumeric(role_id.EditValue))
                    role_id.EditValue = FormatNumber(role_id.EditValue, null);
            }

            // provider
            provider.SetupEditAttributes();

            // activated
            activated.SetupEditAttributes();

            // profile
            _profile.SetupEditAttributes();

            // created_at

            // updated_at

            // Call Row Rendered event
            RowRendered();
        }
        #pragma warning restore 1998

        // Aggregate list row values
        public void AggregateListRowValues()
        {
        }

        #pragma warning disable 1998
        // Aggregate list row (for rendering)
        public async Task AggregateListRow()
        {
            // Call Row Rendered event
            RowRendered();
        }
        #pragma warning restore 1998

        // Export data in HTML/CSV/Word/Excel/Email/PDF format
        public async Task ExportDocument(dynamic doc, DbDataReader dataReader, int startRec, int stopRec, string exportType = "")
        {
            if (doc == null)
                return;
            if (dataReader == null)
                return;
            if (!doc.ExportCustom) {
                // Write header
                doc.ExportTableHeader();
                if (doc.Horizontal) { // Horizontal format, write header
                    doc.BeginExportRow();
                    if (exportType == "view") {
                        doc.ExportCaption(id);
                        doc.ExportCaption(first_name);
                        doc.ExportCaption(last_name);
                        doc.ExportCaption(_email);
                        doc.ExportCaption(_username);
                        doc.ExportCaption(mobile);
                        doc.ExportCaption(photo);
                        doc.ExportCaption(role_id);
                        doc.ExportCaption(provider);
                        doc.ExportCaption(activated);
                        doc.ExportCaption(created_at);
                        doc.ExportCaption(updated_at);
                    } else {
                        doc.ExportCaption(id);
                        doc.ExportCaption(first_name);
                        doc.ExportCaption(last_name);
                        doc.ExportCaption(_email);
                        doc.ExportCaption(_username);
                        doc.ExportCaption(mobile);
                        doc.ExportCaption(photo);
                        doc.ExportCaption(role_id);
                        doc.ExportCaption(provider);
                        doc.ExportCaption(activated);
                        doc.ExportCaption(created_at);
                        doc.ExportCaption(updated_at);
                    }
                    doc.EndExportRow();
                }
            }
            int recCnt = startRec - 1;
            // Read first record for View Page // DN
            if (exportType == "view") {
                await dataReader.ReadAsync();
            // Move to and read first record for list page // DN
            } else {
                if (Connection.SelectOffset) {
                    await dataReader.ReadAsync();
                } else {
                    for (int i = 0; i < startRec; i++) // Move to the start record and use do-while loop
                        await dataReader.ReadAsync();
                }
            }
            int rowcnt = 0; // DN
            do { // DN
                recCnt++;
                if (recCnt >= startRec) {
                    rowcnt = recCnt - startRec + 1;

                    // Page break
                    if (ExportPageBreakCount > 0) {
                        if (rowcnt > 1 && (rowcnt - 1) % ExportPageBreakCount == 0)
                            doc.ExportPageBreak();
                    }
                    LoadListRowValues(dataReader);

                    // Render row
                    RowType = RowType.View; // Render view
                    ResetAttributes();
                    await RenderListRow();
                    if (!doc.ExportCustom) {
                        doc.BeginExportRow(rowcnt); // Allow CSS styles if enabled
                        if (exportType == "view") {
                            await doc.ExportField(id);
                            await doc.ExportField(first_name);
                            await doc.ExportField(last_name);
                            await doc.ExportField(_email);
                            await doc.ExportField(_username);
                            await doc.ExportField(mobile);
                            await doc.ExportField(photo);
                            await doc.ExportField(role_id);
                            await doc.ExportField(provider);
                            await doc.ExportField(activated);
                            await doc.ExportField(created_at);
                            await doc.ExportField(updated_at);
                        } else {
                            await doc.ExportField(id);
                            await doc.ExportField(first_name);
                            await doc.ExportField(last_name);
                            await doc.ExportField(_email);
                            await doc.ExportField(_username);
                            await doc.ExportField(mobile);
                            await doc.ExportField(photo);
                            await doc.ExportField(role_id);
                            await doc.ExportField(provider);
                            await doc.ExportField(activated);
                            await doc.ExportField(created_at);
                            await doc.ExportField(updated_at);
                        }
                        doc.EndExportRow(rowcnt);
                    }
                }

                // Call Row Export server event
                if (doc.ExportCustom)
                    RowExport(doc, dataReader);
            } while (recCnt < stopRec && await dataReader.ReadAsync()); // DN
            if (!doc.ExportCustom)
                doc.ExportTableFooter();
        }

        // User ID filter
        public string GetUserIDFilter(object userid)
        {
            string userIdFilter = "[id] = " + QuotedValue(userid, DataType.Number, Config.UserTableDbId);
            return userIdFilter;
        }

        // Add User ID filter
        public string AddUserIDFilter(string filter = "", string id = "")
        {
            string filterWrk = "";
            if (id == "")
                id = (CurrentPageID() == "list") ? CurrentAction : CurrentPageID();
            if (!UserIDAllow(id) && !Security.IsAdmin) {
                filterWrk = Security.UserIDList();
                if (!Empty(filterWrk))
                    filterWrk = "[id] IN (" + filterWrk + ")";
            }

            // Call User ID Filtering event
            UserIdFiltering(ref filterWrk);
            AddFilter(ref filter, filterWrk);
            return filter;
        }

        // User ID subquery
        public string GetUserIDSubquery(DbField fld, DbField masterfld)
        {
            string wrk = "";
            string sql = "SELECT " + masterfld.Expression + " FROM dbo.users";
            string filter = AddUserIDFilter();
            if (!Empty(filter))
                sql += " WHERE " + filter;
            var list = Connection.GetRows(sql);
            wrk = String.Join(",", list.Select(d => QuotedValue(d.Values.First(), masterfld.DataType, Config.UserTableDbId))); // List all values
            if (!Empty(wrk))
                wrk = fld.Expression + " IN (" + wrk + ")";
            else
                wrk = "0=1"; // No User ID value found
            return wrk;
        }

        // Table filter
        private string? _tableFilter = null;

        public string TableFilter
        {
            get => _tableFilter ?? "";
            set => _tableFilter = value;
        }

        // TblBasicSearchDefault
        private string? _tableBasicSearchDefault = null;

        public string TableBasicSearchDefault
        {
            get => _tableBasicSearchDefault ?? "";
            set => _tableBasicSearchDefault = value;
        }

        #pragma warning disable 1998
        // Get file data
        public async Task<IActionResult> GetFileData(string fldparm, string[] keys, bool resize, int width = -1, int height = -1)
        {
            if (width < 0)
                width = Config.ThumbnailDefaultWidth;
            if (height < 0)
                height = Config.ThumbnailDefaultHeight;

            // Set up field names
            string fldName = "", fileNameFld = "", fileTypeFld = "";
            if (SameText(fldparm, "photo")) {
                fldName = "photo";
                fileNameFld = "photo";
            } else {
                return JsonBoolResult.FalseResult; // Incorrect field
            }

            // Set up key values
            if (keys.Length == 1) {
                id.CurrentValue = keys[0];
            } else {
                return JsonBoolResult.FalseResult; // Incorrect key
            }

            // Set up filter (WHERE Clause)
            string filter = GetRecordFilter();
            CurrentFilter = filter;
            string sql = CurrentSql;
            using var rs = await Connection.GetDataReaderAsync(sql);
            if (rs != null && await rs.ReadAsync()) {
                var val = rs[fldName];
                if (!Empty(val)) {
                    if (Fields.TryGetValue(fldName, out DbField? fld) && fld != null) {
                        // Binary data
                        if (fld.IsBlob) {
                            byte[] data = (byte[])val;
                            if (resize && data.Length > 0)
                                ResizeBinary(ref data, ref width, ref height);
                            string? contentType = "";

                            // Write file type
                            if (!Empty(fileTypeFld) && !Empty(rs[fileTypeFld]))
                                contentType = ConvertToString(rs[fileTypeFld]);
                            else
                                contentType = ContentType(data);

                            // Write file data
                            if (data.Take(8).SequenceEqual(new byte[] {0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00}) && // Fix Office 2007 documents
                                !data.TakeLast(4).SequenceEqual(new byte[] {0x00, 0x00, 0x00, 0x00}))
                                    data.Concat(new byte[] {0x00, 0x00, 0x00, 0x00});

                            // Clear any debug message
                            // Response?.Clear();

                            // Return file content result // DN
                            string downloadFileName = !Empty(fileNameFld) && !Empty(rs[fileNameFld]) ?
                                ConvertToString(rs[fileNameFld]) :
                                DownloadFileName;
                            string ext = ContentExtension(data).Replace(".", ""); // Get file extension
                            if (ext == "pdf" && Config.EmbedPdf) // Embed Pdf // DN
                                downloadFileName = "";
                            if (!Empty(downloadFileName))
                                return Controller.File(data, contentType, downloadFileName);
                            else
                                return Controller.File(data, contentType);

                        // Upload to folder
                        } else {
                            List<string> files;
                            if (fld.UploadMultiple)
                                files = ConvertToString(val).Split(Config.MultipleUploadSeparator).ToList();
                            else
                                files = new () { ConvertToString(val) };
                            var mi = fld.GetType().GetMethod("GetUploadPath");
                            if (mi != null) // GetUploadPath
                                fld.UploadPath = ConvertToString(mi.Invoke(fld, null));
                            var result = files.ToDictionary(f => f, f => Config.EncryptFilePath
                                ? FullUrl(Config.ApiUrl + Config.ApiFileAction + "/" + TableVar + "/" + Encrypt(fld.PhysicalUploadPath + f))
                                : FullUrl(fld.HrefPath + f));
                            return new JsonBoolResult(new Dictionary<string, object> { { fld.Param, result } }, true);
                        }
                    }
                }
            }
            return JsonBoolResult.FalseResult; // Incorrect key
        }
        #pragma warning restore 1998

        // Write audit trail start/end for grid update
        public async Task WriteAuditTrailLog(string log) => await WriteAuditTrailAsync(CurrentUser(), log, "users");

        // Write audit trail (add page)
        public async Task WriteAuditTrailOnAdd(IDictionary<string, object> rs)
        {
            if (!AuditTrailOnAdd)
                return;
            string table = "users";

            // Get key value
            string key = GetKey(rs); // DN
            var usr = CurrentUser();
            foreach (string fldname in rs.Keys) {
                if (Fields.TryGetValue(fldname, out DbField? fld) && !fld.IsBlob) { // Ignore BLOB fields
                    object newvalue;
                    if (fld.HtmlTag == "PASSWORD") {
                        newvalue = Language.Phrase("PasswordMask"); // Password Field
                    } else if (fld.DataType == DataType.Memo) {
                        newvalue = Config.AuditTrailToDatabase ? rs[fldname] : "[MEMO]";
                    } else if (fld.DataType == DataType.Xml) { // XML Field
                        newvalue = "[XML]";
                    } else {
                        newvalue = rs[fldname];
                    }
                    if (fldname == Config.LoginPasswordFieldName)
                        newvalue = Language.Phrase("PasswordMask");
                    await WriteAuditTrailAsync(usr, "A", table, fldname, key, "", newvalue);
                }
            }
        }

        // Write audit trail (edit page)
        public async Task WriteAuditTrailOnEdit(IDictionary<string, object> rsold, IDictionary<string, object> rsnew)
        {
            if (!AuditTrailOnEdit)
                return;
            string table = "users";

            // Get key value
            string key = GetKey(rsold); // DN

            // Write audit trail
            var dt = DbCurrentDateTime();
            var id = ScriptName();
            var usr = CurrentUser();
            foreach (string fldname in rsnew.Keys) {
                if (Fields.TryGetValue(fldname, out DbField? fld) && !fld.IsBlob) { // Ignore BLOB fields
                    bool modified = false; object? oldvalue = null; object? newvalue = null;
                    if (fld.DataType == DataType.Date) { // DateTime field
                        modified = (FormatDateTime(rsold[fldname], 0) != FormatDateTime(rsnew[fldname], 0));
                    } else if (IsFloatType(fld.Type)) { // Float field
                        modified = ConvertToDouble(rsold[fldname]) != ConvertToDouble(rsnew[fldname]);
                    } else {
                        modified = !CompareValue(rsold[fldname], rsnew[fldname]);
                    }
                    if (modified) {
                        if (fld.HtmlTag == "PASSWORD") { // Password Field
                            oldvalue = Language.Phrase("PasswordMask");
                            newvalue = Language.Phrase("PasswordMask");
                        } else if (fld.DataType == DataType.Memo) { // Memo field
                            if (Config.AuditTrailToDatabase) {
                                oldvalue = rsold[fldname];
                                newvalue = rsnew[fldname];
                            } else {
                                oldvalue = "[MEMO]";
                                newvalue = "[MEMO]";
                            }
                        } else if (fld.DataType == DataType.Xml) { // XML field
                            oldvalue = "[XML]";
                            newvalue = "[XML]";
                        } else {
                            oldvalue = rsold[fldname];
                            newvalue = rsnew[fldname];
                        }
                        if (fldname == Config.LoginPasswordFieldName) {
                            oldvalue = Language.Phrase("PasswordMask");
                            newvalue = Language.Phrase("PasswordMask");
                        }
                        await WriteAuditTrailAsync(usr, "U", table, fldname, key, oldvalue, newvalue);
                    }
                }
            }
        }

        // Write audit trail (delete page)
        public async Task WriteAuditTrailOnDelete(IDictionary<string, object> rs)
        {
            if (!AuditTrailOnDelete)
                return;
            string table = "users";

            // Get key value
            string key = GetKey(rs); // DN

            // Write audit trail
            var dt = DbCurrentDateTime();
            var id = ScriptName();
            var usr = CurrentUser();
            foreach (string fldname in rs.Keys) {
                if (Fields.TryGetValue(fldname, out DbField? fld) && !fld.IsBlob) { // Ignore BLOB fields
                    object? oldvalue = null;
                    if (fld.HtmlTag == "PASSWORD") { // Password Field
                        oldvalue = Language.Phrase("PasswordMask"); // Password Field
                    } else if (fld.DataType == DataType.Memo) {
                        oldvalue = Config.AuditTrailToDatabase ? rs[fldname] : "[MEMO]";
                    } else if (fld.DataType == DataType.Xml) { // XML field
                        oldvalue = "[XML]";
                    } else {
                        oldvalue = rs[fldname];
                    }
                    if (fldname == Config.LoginPasswordFieldName)
                        oldvalue = Language.Phrase("PasswordMask");
                    await WriteAuditTrailAsync(usr, "D", table, fldname, key, oldvalue);
                }
            }
        }

        // Table level events

        // Table Load event
        public void TableLoad()
        {
            // Enter your code here
        }

        // Recordset Selecting event
        public void RecordsetSelecting(ref string filter) {
            // Enter your code here
        }

        // Recordset Selected event
        public void RecordsetSelected(DbDataReader rs) {
            // Enter your code here
        }

        // Recordset Searching event
        public void RecordsetSearching(ref string filter) {
            // Enter your code here
        }

        // Recordset Search Validated event
        public void RecordsetSearchValidated() {
            // Enter your code here
        }

        // Row_Selecting event
        public void RowSelecting(ref string filter) {
            // Enter your code here
        }

        // Row Selected event
        public void RowSelected(Dictionary<string, object> row) {
            //Log("Row Selected");
        }

        // Row Inserting event
        public bool RowInserting(Dictionary<string, object>? rsold, Dictionary<string, object> rsnew) {
            // Enter your code here
            // To cancel, set return value to False and error message to CancelMessage
            return true;
        }

        // Row Inserted event
        public void RowInserted(Dictionary<string, object>? rsold, Dictionary<string, object> rsnew) {
            //Log("Row Inserted");
        }

        // Row Updating event
        public bool RowUpdating(Dictionary<string, object> rsold, Dictionary<string, object> rsnew) {
            // Enter your code here
            // To cancel, set return value to False and error message to CancelMessage
            return true;
        }

        // Row Updated event
        public void RowUpdated(Dictionary<string, object> rsold, Dictionary<string, object> rsnew) {
            //Log("Row Updated");
        }

        // Row Update Conflict event
        public bool RowUpdateConflict(Dictionary<string, object> rsold, Dictionary<string, object> rsnew) {
            // Enter your code here
            // To ignore conflict, set return value to false
            return true;
        }

        // Recordset Deleting event
        public bool RowDeleting(Dictionary<string, object> rs) {
            // Enter your code here
            // To cancel, set return value to False and error message to CancelMessage
            return true;
        }

        // Row Deleted event
        public void RowDeleted(Dictionary<string, object> rs) {
            //Log("Row Deleted");
        }

        // Row Export event
        // doc = export document object
        public virtual void RowExport(dynamic doc, DbDataReader rs) {
            //doc.Text.Append("<div>" + MyField.ViewValue +"</div>"); // Build HTML with field value: rs["MyField"] or MyField.ViewValue
        }

        // Email Sending event
        public virtual bool EmailSending(Email email, dynamic? args) {
            //Log(email);
            return true;
        }

        // Lookup Selecting event
        public void LookupSelecting(DbField fld, ref string filter) {
            // Enter your code here
        }

        // Row Rendering event
        public void RowRendering() {
            // Enter your code here
        }

        // Row Rendered event
        public void RowRendered() {
            //VarDump(<FieldName>); // View field properties
        }

        // User ID Filtering event
        public void UserIdFiltering(ref string filter) {
            // Enter your code here
        }
    }
} // End Partial class
