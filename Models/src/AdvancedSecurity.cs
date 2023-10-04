namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// Advanced Security class
    /// </summary>
    public class AdvancedSecurity : AdvancedSecurityBase
    {
        // Constructor
        public AdvancedSecurity() : base()
        {
            Security = this;
        }
    }
} // End Partial class
