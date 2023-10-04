namespace WebApp.Models;

// Partial class
public partial class WebApp {
    // Configuration
    public static partial class Config
    {
        //
        // User level settings
        //

        // User level info
        public static List<UserLevel> UserLevels = new ()
        {
            new () { Id = -2, Name = "Anonymous" },
            new () { Id = 0, Name = "Default" },
            new () { Id = 1, Name = "Editor" }
        };

        // User level priv info
        public static List<UserLevelPermission> UserLevelPermissions = new ()
        {
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}restaurants", Id = -2, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}restaurants", Id = 0, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}restaurants", Id = 1, Permission = 367 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}dishes", Id = -2, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}dishes", Id = 0, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}dishes", Id = 1, Permission = 367 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}users", Id = -2, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}users", Id = 0, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}users", Id = 1, Permission = 367 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}roles", Id = -2, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}roles", Id = 0, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}roles", Id = 1, Permission = 367 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}permissions", Id = -2, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}permissions", Id = 0, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}permissions", Id = 1, Permission = 367 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}audit_trails", Id = -2, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}audit_trails", Id = 0, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}audit_trails", Id = 1, Permission = 367 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}export_logs", Id = -2, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}export_logs", Id = 0, Permission = 0 },
            new () { Table = "{89456192-A767-4B60-B043-591A4AA6A5D7}export_logs", Id = 1, Permission = 367 }
        };

        // User level table info // DN
        public static List<UserLevelTablePermission> UserLevelTablePermissions = new ()
        {
            new () { TableName = "restaurants", TableVar = "restaurants", Caption = "Restaurante", Allowed = true, ProjectId = "{89456192-A767-4B60-B043-591A4AA6A5D7}", Url = "restaurantslist" },
            new () { TableName = "dishes", TableVar = "dishes", Caption = "Plato", Allowed = true, ProjectId = "{89456192-A767-4B60-B043-591A4AA6A5D7}", Url = "disheslist" },
            new () { TableName = "users", TableVar = "users", Caption = "Usuario", Allowed = true, ProjectId = "{89456192-A767-4B60-B043-591A4AA6A5D7}", Url = "userslist" },
            new () { TableName = "roles", TableVar = "roles", Caption = "Role", Allowed = true, ProjectId = "{89456192-A767-4B60-B043-591A4AA6A5D7}", Url = "roleslist" },
            new () { TableName = "permissions", TableVar = "permissions2", Caption = "Permiso", Allowed = true, ProjectId = "{89456192-A767-4B60-B043-591A4AA6A5D7}", Url = "permissions2list" },
            new () { TableName = "audit_trails", TableVar = "audit_trails", Caption = "Auditoria", Allowed = true, ProjectId = "{89456192-A767-4B60-B043-591A4AA6A5D7}", Url = "audittrailslist" },
            new () { TableName = "export_logs", TableVar = "export_logs", Caption = "Log de Exportación", Allowed = true, ProjectId = "{89456192-A767-4B60-B043-591A4AA6A5D7}", Url = "exportlogslist" }
        };
    }
} // End Partial class
