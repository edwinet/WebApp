namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// Authentication provider
    /// </summary>
    public class AuthenticationProvider
    {
        public bool Enabled;

        public string Id = "";

        public string Color = "";

        public string Secret = "";
    }
} // End Partial class
