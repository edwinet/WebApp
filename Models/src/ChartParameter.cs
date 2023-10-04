namespace WebApp.Models;

// Partial class
public partial class WebApp {
    /// <summary>
    /// Chart parameter class
    /// </summary>
    public class ChartParameter
    {
        public string Key = "";

        public object? Value = null;

        public bool Output = true;

        public ChartParameter(string k, object v, bool o = true)
        {
            Key = k;
            Value = v;
            Output = o;
        }
    }
} // End Partial class
