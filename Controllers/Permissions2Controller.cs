namespace WebApp.Controllers;

// Partial class
public partial class HomeController : Controller
{
    // list
    [Route("permissions2list/{entity?}/{role_id?}", Name = "permissions2list-permissions2-list")]
    [Route("home/permissions2list/{entity?}/{role_id?}", Name = "permissions2list-permissions2-list-2")]
    public async Task<IActionResult> Permissions2List()
    {
        // Create page object
        permissions2List = new GLOBALS.Permissions2List(this);
        permissions2List.Cache = _cache;

        // Run the page
        return await permissions2List.Run();
    }

    // edit
    [Route("permissions2edit/{entity?}/{role_id?}", Name = "permissions2edit-permissions2-edit")]
    [Route("home/permissions2edit/{entity?}/{role_id?}", Name = "permissions2edit-permissions2-edit-2")]
    public async Task<IActionResult> Permissions2Edit()
    {
        // Create page object
        permissions2Edit = new GLOBALS.Permissions2Edit(this);

        // Run the page
        return await permissions2Edit.Run();
    }

    // delete
    [Route("permissions2delete/{entity?}/{role_id?}", Name = "permissions2delete-permissions2-delete")]
    [Route("home/permissions2delete/{entity?}/{role_id?}", Name = "permissions2delete-permissions2-delete-2")]
    public async Task<IActionResult> Permissions2Delete()
    {
        // Create page object
        permissions2Delete = new GLOBALS.Permissions2Delete(this);

        // Run the page
        return await permissions2Delete.Run();
    }
}
