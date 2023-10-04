namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// User level class
    /// </summary>
    public class UserLevel
    {
        // User level ID
        [SqlKata.Column("id")]
        public int Id { set; get; }

        // Name
        [SqlKata.Column("role_name")]
        public string Name { set; get; } = "";
    }
} // End Partial class
