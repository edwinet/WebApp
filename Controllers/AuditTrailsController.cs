namespace WebApp.Controllers;

// Partial class
public partial class HomeController : Controller
{
    // list
    [Route("audittrailslist/{id?}", Name = "audittrailslist-audit_trails-list")]
    [Route("home/audittrailslist/{id?}", Name = "audittrailslist-audit_trails-list-2")]
    public async Task<IActionResult> AuditTrailsList()
    {
        // Create page object
        auditTrailsList = new GLOBALS.AuditTrailsList(this);
        auditTrailsList.Cache = _cache;

        // Run the page
        return await auditTrailsList.Run();
    }

    // view
    [Route("audittrailsview/{id?}", Name = "audittrailsview-audit_trails-view")]
    [Route("home/audittrailsview/{id?}", Name = "audittrailsview-audit_trails-view-2")]
    public async Task<IActionResult> AuditTrailsView()
    {
        // Create page object
        auditTrailsView = new GLOBALS.AuditTrailsView(this);

        // Run the page
        return await auditTrailsView.Run();
    }

    // search
    [Route("audittrailssearch", Name = "audittrailssearch-audit_trails-search")]
    [Route("home/audittrailssearch", Name = "audittrailssearch-audit_trails-search-2")]
    public async Task<IActionResult> AuditTrailsSearch()
    {
        // Create page object
        auditTrailsSearch = new GLOBALS.AuditTrailsSearch(this);

        // Run the page
        return await auditTrailsSearch.Run();
    }
}
