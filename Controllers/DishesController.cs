namespace WebApp.Controllers;

// Partial class
public partial class HomeController : Controller
{
    // list
    [Route("disheslist/{id?}", Name = "disheslist-dishes-list")]
    [Route("home/disheslist/{id?}", Name = "disheslist-dishes-list-2")]
    public async Task<IActionResult> DishesList()
    {
        // Create page object
        dishesList = new GLOBALS.DishesList(this);
        dishesList.Cache = _cache;

        // Run the page
        return await dishesList.Run();
    }

    // add
    [Route("dishesadd/{id?}", Name = "dishesadd-dishes-add")]
    [Route("home/dishesadd/{id?}", Name = "dishesadd-dishes-add-2")]
    public async Task<IActionResult> DishesAdd()
    {
        // Create page object
        dishesAdd = new GLOBALS.DishesAdd(this);

        // Run the page
        return await dishesAdd.Run();
    }

    // view
    [Route("dishesview/{id?}", Name = "dishesview-dishes-view")]
    [Route("home/dishesview/{id?}", Name = "dishesview-dishes-view-2")]
    public async Task<IActionResult> DishesView()
    {
        // Create page object
        dishesView = new GLOBALS.DishesView(this);

        // Run the page
        return await dishesView.Run();
    }

    // edit
    [Route("dishesedit/{id?}", Name = "dishesedit-dishes-edit")]
    [Route("home/dishesedit/{id?}", Name = "dishesedit-dishes-edit-2")]
    public async Task<IActionResult> DishesEdit()
    {
        // Create page object
        dishesEdit = new GLOBALS.DishesEdit(this);

        // Run the page
        return await dishesEdit.Run();
    }

    // delete
    [Route("dishesdelete/{id?}", Name = "dishesdelete-dishes-delete")]
    [Route("home/dishesdelete/{id?}", Name = "dishesdelete-dishes-delete-2")]
    public async Task<IActionResult> DishesDelete()
    {
        // Create page object
        dishesDelete = new GLOBALS.DishesDelete(this);

        // Run the page
        return await dishesDelete.Run();
    }
}
