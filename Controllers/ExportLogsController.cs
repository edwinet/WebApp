namespace WebApp.Controllers;

// Partial class
public partial class HomeController : Controller
{
    // list
    [Route("exportlogslist/{file_id?}", Name = "exportlogslist-export_logs-list")]
    [Route("home/exportlogslist/{file_id?}", Name = "exportlogslist-export_logs-list-2")]
    public async Task<IActionResult> ExportLogsList()
    {
        // Create page object
        exportLogsList = new GLOBALS.ExportLogsList(this);
        exportLogsList.Cache = _cache;

        // Run the page
        return await exportLogsList.Run();
    }

    // view
    [Route("exportlogsview/{file_id?}", Name = "exportlogsview-export_logs-view")]
    [Route("home/exportlogsview/{file_id?}", Name = "exportlogsview-export_logs-view-2")]
    public async Task<IActionResult> ExportLogsView()
    {
        // Create page object
        exportLogsView = new GLOBALS.ExportLogsView(this);

        // Run the page
        return await exportLogsView.Run();
    }

    // search
    [Route("exportlogssearch", Name = "exportlogssearch-export_logs-search")]
    [Route("home/exportlogssearch", Name = "exportlogssearch-export_logs-search-2")]
    public async Task<IActionResult> ExportLogsSearch()
    {
        // Create page object
        exportLogsSearch = new GLOBALS.ExportLogsSearch(this);

        // Run the page
        return await exportLogsSearch.Run();
    }
}
