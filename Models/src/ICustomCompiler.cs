namespace WebApp.Models;

// Partial class
public partial class WebApp {

    public interface ICustomCompiler
    {
        string Output { get; set; }
    }
} // End Partial class
