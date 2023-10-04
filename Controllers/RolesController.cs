namespace WebApp.Controllers;

// Partial class
public partial class HomeController : Controller
{
    // list
    [Route("roleslist/{id?}", Name = "roleslist-roles-list")]
    [Route("home/roleslist/{id?}", Name = "roleslist-roles-list-2")]
    public async Task<IActionResult> RolesList()
    {
        // Create page object
        rolesList = new GLOBALS.RolesList(this);
        rolesList.Cache = _cache;

        // Run the page
        return await rolesList.Run();
    }

    // add
    [Route("rolesadd/{id?}", Name = "rolesadd-roles-add")]
    [Route("home/rolesadd/{id?}", Name = "rolesadd-roles-add-2")]
    public async Task<IActionResult> RolesAdd()
    {
        // Create page object
        rolesAdd = new GLOBALS.RolesAdd(this);

        // Run the page
        return await rolesAdd.Run();
    }

    // edit
    [Route("rolesedit/{id?}", Name = "rolesedit-roles-edit")]
    [Route("home/rolesedit/{id?}", Name = "rolesedit-roles-edit-2")]
    public async Task<IActionResult> RolesEdit()
    {
        // Create page object
        rolesEdit = new GLOBALS.RolesEdit(this);

        // Run the page
        return await rolesEdit.Run();
    }

    // delete
    [Route("rolesdelete/{id?}", Name = "rolesdelete-roles-delete")]
    [Route("home/rolesdelete/{id?}", Name = "rolesdelete-roles-delete-2")]
    public async Task<IActionResult> RolesDelete()
    {
        // Create page object
        rolesDelete = new GLOBALS.RolesDelete(this);

        // Run the page
        return await rolesDelete.Run();
    }
}
