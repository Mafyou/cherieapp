using System.Reflection;

namespace MAUICherieApp.Services;

public sealed class APIService
{
    #region Singleton
    private static readonly Lazy<APIService> lazy = new Lazy<APIService>(() => new APIService());
    public static APIService Instance
    {
        get
        {
            return lazy.Value;
        }
    }
    #endregion
    public HttpClient Client { get; set; }
    private APIService()
    {
        Client = new HttpClient
        {
            BaseAddress = new Uri(Constants.API_Url)
        };
    }
}