namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// User level permisson handler
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected string LoginUrl = "";

        public PermissionHandler()
        {
            LoginUrl = GetPathByName("login") ?? "";
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            Language = ResolveLanguage();
            Profile = ResolveProfile();
            var security = ResolveSecurity();
            if (IsApi()) { // API
                if (!ValidApiRequest())
                    return Task.CompletedTask;
                var routeValues = Request?.RouteValues;
                if (routeValues?.TryGetValue("controller", out object? name) ?? false) { // Note: Check "controller", not "action"
                    string pageAction = ConvertToString(name).ToLower();
                    if (Config.ApiPageActions.Contains(pageAction, StringComparer.OrdinalIgnoreCase)) {
                        string tblVar = ConvertToString(routeValues["table"] ?? Post(Config.ApiObjectName));
                        if (!Empty(tblVar))
                            security.LoadTablePermissions(tblVar);
                        if (pageAction == Config.ApiListAction && security.CanList ||
                            pageAction == Config.ApiExportAction && (security.CanExport || Empty(tblVar)) ||
                            pageAction == Config.ApiViewAction && security.CanView ||
                            pageAction == Config.ApiAddAction && security.CanAdd ||
                            pageAction == Config.ApiEditAction && security.CanEdit ||
                            pageAction == Config.ApiDeleteAction && security.CanDelete ||
                            pageAction == Config.ApiFileAction && (security.CanList || security.CanView))
                            context.Succeed(requirement);
                    } else if (pageAction == Config.ApiLookupAction) { // Lookup
                        if (SameText(Request?.ContentType, "application/json")) { // Batch lookup, to be checked in controller
                            context.Succeed(requirement);
                        } else {
                            string pageName = Param(Config.ApiLookupPage); // Get lookup page
                            var t = Type.GetType(Config.ProjectClassName + "+" + pageName);
                            if (t != null) {
                                var page = Resolve(pageName);
                                if (page != null) {
                                    string fieldName = Post(Config.ApiFieldName); // Get field name
                                    var tbl = page.FieldByName(fieldName)?.Lookup?.GetTable();
                                    if (tbl != null) {
                                        security.LoadTablePermissions(tbl.TableVar);
                                        if (security.CanLookup)
                                            context.Succeed(requirement);
                                    }
                                }
                            }
                        }
                    } else if (pageAction == Config.ApiPushNotificationAction) { // Push notification
                        string action = ConvertToString(routeValues["action"]);
                        if (SameText(action, Config.ApiPushNotificationSubscribe) || SameText(action, Config.ApiPushNotificationDelete)) {
                            if (Config.PushAnonymous || security.IsLoggedIn)
                                context.Succeed(requirement);
                        } else if (SameText(action, Config.ApiPushNotificationSend)) {
                            security.LoadTablePermissions(Config.SubscriptionTableVar);
                            if (security.CanPush)
                                context.Succeed(requirement);
                        }
                    } else if (pageAction == "_" + Config.Api2FaAction) { // Two factor authentication // controller is prefixed with "_" // DN
                        string action = ConvertToString(routeValues["name"]);
                        if (action == Config.Api2FaShow) {
                            context.Succeed(requirement);
                        } else if (action == Config.Api2FaVerify) {
                            if (Config.ForceTwoFactorAuthentication && security.IsLoggingIn2FA || security.IsLoggedIn)
                                context.Succeed(requirement);
                        } else if (action == Config.Api2FaReset) {
                            if (security.IsLoggedIn)
                                context.Succeed(requirement);
                        } else if (action == Config.Api2FaBackupCodes || action == Config.Api2FaNewBackupCodes) {
                            if (security.IsLoggedIn && !security.IsSysAdmin)
                                context.Succeed(requirement);
                        } else if (action == Config.Api2FaSendOtp) {
                            if ((security.IsLoggingIn2FA || security.IsLoggedIn) && !security.IsSysAdmin)
                                context.Succeed(requirement);
                        }
                    } else if (pageAction == Config.ApiUploadAction) { // Upload
                        if (security.IsLoggedIn) // ValidApiRequest() should have logged in the user
                            context.Succeed(requirement);
                    } else if (pageAction == Config.ApiPermissionsAction) { // Permission
                        if (IsGet() || IsPost() && security.IsAdmin)
                            context.Succeed(requirement);
                    } else if (pageAction == Config.ApiJqueryUploadAction ||
                        pageAction == Config.ApiSessionAction ||
                        pageAction == Config.ApiExportChartAction) {
                        context.Succeed(requirement); // To be validated by [AutoValidateAntiforgeryToken]
                    }
                }
                return Task.CompletedTask;
            } else { // Non API
                string[] ar = new string[] {};
                if (!Empty(RouteName))
                    ar = RouteName.Split('-');
                string currentPageName = ar.Length >= 1 ? ar[0] : ""; // Get current page name
                string table = (ar.Length > 2) ? ar[1] : "";
                string pageAction = (ar.Length > 2) ? ar[2] : currentPageName;
                string redirectUrl = "";

                // Auto login
                if (!security.IsLoggedIn && !IsPasswordReset() && !IsPasswordExpired() && !IsLoggingIn2FA())
                    security.AutoLogin();

                // Validate security
                if (table != "") { // Table level
                    security.LoadTablePermissions(table);
                    if (pageAction == Config.ViewAction && !security.CanView ||
                        (pageAction == Config.EditAction || pageAction == Config.UpdateAction) && !security.CanEdit ||
                        pageAction == Config.AddAction && !security.CanAdd ||
                        pageAction == Config.DeleteAction && !security.CanDelete ||
                        pageAction == Config.SearchAction && !security.CanSearch) {
                        security.SaveLastUrl();
                        SetFailureMessage(DeniedMessage()); // Set no permission
                        if (security.CanList) { // Back to list
                            pageAction = Config.ListAction;
                            var tbl = Resolve(table);
                            if (tbl != null) {
                                string routeName = tbl.ListUrl;
                                if (GetPathByName(routeName, table, pageAction) is string url)
                                    redirectUrl = url;
                            }
                        } else {
                            redirectUrl = LoginUrl;
                        }
                    } else if (pageAction == Config.ListAction && !security.CanList || // List Permission
                        (new[] {
                            Config.CustomReportAction,
                            Config.SummaryReportAction,
                            Config.CrosstabReportAction,
                            Config.DashboardReportAction
                        }).Contains(pageAction) && !security.CanList) { // No permission
                        security.SaveLastUrl();
                        SetFailureMessage(DeniedMessage()); // Set no permission
                        redirectUrl = LoginUrl;
                    }
                } else { // Others
                    if (pageAction == "changepassword") { // Change password
                        if (!IsPasswordReset() && !IsPasswordExpired()) {
                            if (!security.IsLoggedIn || security.IsSysAdmin)
                                redirectUrl = LoginUrl;
                        }
                    } else if (pageAction == "personaldata") { // Personal data
                        if (!security.IsLoggedIn || security.IsSysAdmin) {
                            SetFailureMessage(DeniedMessage()); // Set no permission
                            redirectUrl = LoginUrl;
                        }
                    } else if (pageAction == "userpriv") { // User priv
                        table = "roles";
                        pageAction = Config.ListAction;
                        var tbl = Resolve(table);
                        if (tbl != null) {
                            string routeName = tbl.ListUrl;
                            security.LoadTablePermissions(table);
                            if (!security.IsLoggedIn || !security.IsAdmin) {
                                SetFailureMessage(DeniedMessage()); // Set no permission
                                if (GetPathByName(routeName, table, pageAction) is string url)
                                    redirectUrl = url; // Redirect to List page of User Level table
                            }
                        }
                    } else if (pageAction == "filemanager") { // File manager
                        if (!security.IsLoggedIn) {
                            Response?.OnStarting(async () => {
                                Response.StatusCode = 500;
                                await Response.WriteAsJsonAsync(DeniedMessage());
                            });
                            return Task.CompletedTask;
                        }
                    }
                }

                // Succeed
                if (Empty(redirectUrl)) {
                    context.Succeed(requirement);
                } else {
                    Response?.OnStarting(async () => {
                        if (Get<bool>("modal")) { // Modal page
                            if (redirectUrl == LoginUrl && Config.UseModalLogin) { // Redirect to login
                                Response.Redirect(redirectUrl);
                            } else {
                                Response.StatusCode = 200;
                                await Response.WriteAsJsonAsync(new { url = redirectUrl });
                            }
                        } else {
                            if (redirectUrl != LoginUrl)
                                Response.Redirect(redirectUrl);
                        }
                    });
                }

                // Return
                return Task.CompletedTask;
            }
        }
    }
} // End Partial class
