namespace WebApp.Controllers;

// Partial class
public partial class HomeController : Controller
{
    // list
    [Route("userslist/{id?}", Name = "userslist-users-list")]
    [Route("home/userslist/{id?}", Name = "userslist-users-list-2")]
    public async Task<IActionResult> UsersList()
    {
        // Create page object
        usersList = new GLOBALS.UsersList(this);
        usersList.Cache = _cache;

        // Run the page
        return await usersList.Run();
    }

    // add
    [Route("usersadd/{id?}", Name = "usersadd-users-add")]
    [Route("home/usersadd/{id?}", Name = "usersadd-users-add-2")]
    public async Task<IActionResult> UsersAdd()
    {
        // Create page object
        usersAdd = new GLOBALS.UsersAdd(this);

        // Run the page
        return await usersAdd.Run();
    }

    // view
    [Route("usersview/{id?}", Name = "usersview-users-view")]
    [Route("home/usersview/{id?}", Name = "usersview-users-view-2")]
    public async Task<IActionResult> UsersView()
    {
        // Create page object
        usersView = new GLOBALS.UsersView(this);

        // Run the page
        return await usersView.Run();
    }

    // edit
    [Route("usersedit/{id?}", Name = "usersedit-users-edit")]
    [Route("home/usersedit/{id?}", Name = "usersedit-users-edit-2")]
    public async Task<IActionResult> UsersEdit()
    {
        // Create page object
        usersEdit = new GLOBALS.UsersEdit(this);

        // Run the page
        return await usersEdit.Run();
    }

    // delete
    [Route("usersdelete/{id?}", Name = "usersdelete-users-delete")]
    [Route("home/usersdelete/{id?}", Name = "usersdelete-users-delete-2")]
    public async Task<IActionResult> UsersDelete()
    {
        // Create page object
        usersDelete = new GLOBALS.UsersDelete(this);

        // Run the page
        return await usersDelete.Run();
    }

    // search
    [Route("userssearch", Name = "userssearch-users-search")]
    [Route("home/userssearch", Name = "userssearch-users-search-2")]
    public async Task<IActionResult> UsersSearch()
    {
        // Create page object
        usersSearch = new GLOBALS.UsersSearch(this);

        // Run the page
        return await usersSearch.Run();
    }
}
