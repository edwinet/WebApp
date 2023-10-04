namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// User level permission class
    /// </summary>
    public class UserLevelPermission
    {
        // Table name
        [SqlKata.Column("entity")]
        public string Table { set; get; } = "";

        // User level ID
        [SqlKata.Column("role_id")]
        public int Id { set; get; }

        // Permission
        [SqlKata.Column("permission")]
        public int Permission { set; get; } = 0;
    }
} // End Partial class
