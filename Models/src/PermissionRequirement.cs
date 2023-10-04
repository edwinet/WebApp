namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// User level requirement
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement()
        {
        }
    }
} // End Partial class
