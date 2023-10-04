namespace WebApp.Controllers;

// Partial class
public partial class HomeController : Controller
{
    // list
    [Route("restaurantslist/{id?}", Name = "restaurantslist-restaurants-list")]
    [Route("home/restaurantslist/{id?}", Name = "restaurantslist-restaurants-list-2")]
    public async Task<IActionResult> RestaurantsList()
    {
        // Create page object
        restaurantsList = new GLOBALS.RestaurantsList(this);
        restaurantsList.Cache = _cache;

        // Run the page
        return await restaurantsList.Run();
    }

    // add
    [Route("restaurantsadd/{id?}", Name = "restaurantsadd-restaurants-add")]
    [Route("home/restaurantsadd/{id?}", Name = "restaurantsadd-restaurants-add-2")]
    public async Task<IActionResult> RestaurantsAdd()
    {
        // Create page object
        restaurantsAdd = new GLOBALS.RestaurantsAdd(this);

        // Run the page
        return await restaurantsAdd.Run();
    }

    // view
    [Route("restaurantsview/{id?}", Name = "restaurantsview-restaurants-view")]
    [Route("home/restaurantsview/{id?}", Name = "restaurantsview-restaurants-view-2")]
    public async Task<IActionResult> RestaurantsView()
    {
        // Create page object
        restaurantsView = new GLOBALS.RestaurantsView(this);

        // Run the page
        return await restaurantsView.Run();
    }

    // edit
    [Route("restaurantsedit/{id?}", Name = "restaurantsedit-restaurants-edit")]
    [Route("home/restaurantsedit/{id?}", Name = "restaurantsedit-restaurants-edit-2")]
    public async Task<IActionResult> RestaurantsEdit()
    {
        // Create page object
        restaurantsEdit = new GLOBALS.RestaurantsEdit(this);

        // Run the page
        return await restaurantsEdit.Run();
    }

    // delete
    [Route("restaurantsdelete/{id?}", Name = "restaurantsdelete-restaurants-delete")]
    [Route("home/restaurantsdelete/{id?}", Name = "restaurantsdelete-restaurants-delete-2")]
    public async Task<IActionResult> RestaurantsDelete()
    {
        // Create page object
        restaurantsDelete = new GLOBALS.RestaurantsDelete(this);

        // Run the page
        return await restaurantsDelete.Run();
    }
}
