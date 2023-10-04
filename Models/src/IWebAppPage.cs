namespace WebApp.Models;

// Partial class
public partial class WebApp {
    // IWebAppPage interface // DN
    public interface IWebAppPage
    {
        Task<IActionResult> Run();
        IActionResult Terminate(string url = "");
    }
} // End Partial class
