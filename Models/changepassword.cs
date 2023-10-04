namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// changePassword
    /// </summary>
    public static ChangePassword changePassword
    {
        get => HttpData.Get<ChangePassword>("changePassword")!;
        set => HttpData["changePassword"] = value;
    }

    /// <summary>
    /// Page class (change_password)
    /// </summary>
    public class ChangePassword : ChangePasswordBase
    {
        // Constructor
        public ChangePassword(Controller controller) : base(controller)
        {
        }

        // Constructor
        public ChangePassword() : base()
        {
        }

        // Server events
    }

    /// <summary>
    /// Page base class
    /// </summary>
    public class ChangePasswordBase : Users
    {
        // Page ID
        public string PageID = "change_password";

        // Project ID
        public string ProjectID = "{89456192-A767-4B60-B043-591A4AA6A5D7}";

        // Page object name
        public string PageObjName = "changePassword";

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
        public ChangePasswordBase()
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
        public string PageName => "changepassword";

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
        public ChangePasswordBase(Controller? controller = null): this() { // DN
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
                    // Handle modal response
                    if (IsModal) { // Show as modal
                        var result = new { url = GetUrl(url) };
                        return Controller.Json(result);
                    } else {
                        SaveDebugMessage();
                        return Controller.LocalRedirect(AppPath(url));
                    }
                }
            }
            return new EmptyResult();
        }

        public DbField<SqlDbType> OldPassword = new (Resolve("usertable"), "opwd", 202, SqlDbType.NVarChar) {
                Name = "opwd",
                Expression = "opwd"
            };

        public DbField<SqlDbType> NewPassword = new (Resolve("usertable"), "npwd", 202, SqlDbType.NVarChar) {
                Name = "npwd",
                Expression = "npwd"
            };

        public DbField<SqlDbType> ConfirmPassword = new (Resolve("usertable"), "cpwd", 202, SqlDbType.NVarChar) {
                Name = "cpwd",
                Expression = "cpwd"
            };

        public bool IsModal = false;

        #pragma warning disable 219
        /// <summary>
        /// Page run
        /// </summary>
        /// <returns>Page result</returns>
        public override async Task<IActionResult> Run()
        {
            OffsetColumnClass = ""; // Override user table

            // Create Password fields object (used by validation only)
            OldPassword.EditAttrs.AppendClass("form-control ew-form-control");
            NewPassword.EditAttrs.AppendClass("form-control ew-form-control");
            NewPassword.EditAttrs.AppendClass("ew-password-strength");
            ConfirmPassword.EditAttrs.AppendClass("form-control ew-form-control");
            if (Config.EncryptedPassword) {
                OldPassword.Raw = true;
                NewPassword.Raw = true;
                ConfirmPassword.Raw = true;
            }

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

            // Check modal
            if (IsModal)
                SkipHeaderFooter = true;
            bool validate = true;
            string emailAddress = "", userName = "";
            Dictionary<string, object> rsnew = new(), rsold;
            CurrentBreadcrumb = new ();
            string url = CurrentUrl(); // DN
            CurrentBreadcrumb.Add("change_password", "ChangePasswordPage", url, "", "", true);
            Heading = Language.Phrase("ChangePasswordPage");
            if (IsPost()) {
                OldPassword.SetFormValue(Post(OldPassword.FieldVar));
                NewPassword.SetFormValue(Post(NewPassword.FieldVar));
                ConfirmPassword.SetFormValue(Post(ConfirmPassword.FieldVar));
                validate = await ValidateForm();
            }
            bool pwdUpdated = false;
            if (IsPost() && validate) {
                // Setup variables
                userName = Security.CurrentUserName;
                if (IsPasswordReset())
                    userName = Session.GetString(Config.SessionUserProfileUserName);
                string filter = GetUserFilter(Config.LoginUsernameFieldName, userName);

                // Set up filter (WHERE clause)
                CurrentFilter = filter;
                string sql = CurrentSql;
                using var dr = await UserTableConn.OpenDataReaderAsync(sql);
                if (dr != null && await dr.ReadAsync()) {
                    if (IsPasswordReset() || ComparePassword(ConvertToString(GetUserInfo(Config.LoginPasswordFieldName, dr)), ConvertToString(OldPassword.CurrentValue))) {
                        var validPwd = true;
                        rsold = UserTableConn.GetRow(dr);
                        if (!IsPasswordReset()) {
                            string newPassword = ConvertToString(NewPassword.CurrentValue); // Work around for ref, to be improved
                            validPwd = UserChangePassword(rsold, userName, ConvertToString(OldPassword.CurrentValue), ref newPassword);
                            NewPassword.CurrentValue = newPassword;
                        }
                        if (validPwd) {
                            string newPassword = Config.EncryptedPassword ?
                                (EncryptPassword(Config.CaseSensitivePassword ? ConvertToString(NewPassword.CurrentValue) : ConvertToString(NewPassword.CurrentValue).ToLower())) :
                                ConvertToString(NewPassword.CurrentValue); // DN
                            rsnew[Config.LoginPasswordFieldName] = newPassword; // Change Password
                            dr.Dispose(); // Avoid database lock for SQLite // DN
                            if (await UpdateAsync(rsnew, null, rsold) > 0)
                                pwdUpdated = true;
                        } else {
                            FailureMessage = Language.Phrase("InvalidNewPassword");
                        }
                    } else {
                        FailureMessage = Language.Phrase("InvalidPassword");
                    }
                }
            }
            if (pwdUpdated) {
                if (Empty(SuccessMessage))
                    SuccessMessage = Language.Phrase("PasswordChanged"); // Set up success message
                if (IsPasswordReset()) {
                    Session[Config.SessionStatus] = "";
                    Session[Config.SessionUserProfileUserName] = "";
                }
                return Terminate("index"); // Return to default page
            }

            // Set LoginStatus, Page Rendering and Page Render
            if (!IsApi() && !IsTerminated) {
                SetupLoginStatus(); // Setup login status

                // Pass login status to client side
                SetClientVar("login", LoginStatus);
            }
            return PageResult();
        }
        #pragma warning restore 219

        #pragma warning disable 1998
        // Validate form
        protected async Task<bool> ValidateForm() {
            // Check if validation required
            if (!Config.ServerValidate)
                return true;
            bool valid = true;
            string oldPassword = ConvertToString(OldPassword.CurrentValue);
            string newPassword = ConvertToString(NewPassword.CurrentValue);
            string confirmPassword = ConvertToString(ConfirmPassword.CurrentValue);
            if (!IsPasswordReset() && Empty(oldPassword)) {
                OldPassword.AddErrorMessage(Language.Phrase("EnterOldPassword"));
                valid = false;
            }
            if (Empty(newPassword)) {
                NewPassword.AddErrorMessage(Language.Phrase("EnterNewPassword"));
                valid = false;
            }
            if (!NewPassword.Raw && Config.RemoveXss && CheckPassword(newPassword)) {
                NewPassword.AddErrorMessage(Language.Phrase("InvalidPasswordChars"));
                valid = false;
            }
            if (!SameString(newPassword, confirmPassword)) {
                ConfirmPassword.AddErrorMessage(Language.Phrase("MismatchPassword"));
                valid = false;
            }

            // Call Form CustomValidate event
            string formCustomError = "";
            valid = valid && FormCustomValidate(ref formCustomError);
            if (!Empty(formCustomError))
                FailureMessage = formCustomError;
            return valid;
        }
        #pragma warning restore 1998

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

        // Form Custom Validate event
        public bool FormCustomValidate(ref string customError) {
            //Return error message in customError
            return true;
        }

        // User ChangePassword event
        public virtual bool UserChangePassword(Dictionary<string, object> rs, string usr, string oldpwd, ref string newpwd) {
            // Return false to abort
            return true;
        }
    } // End page class
} // End Partial class
