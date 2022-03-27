namespace CherieAppUploadZik.Services;

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
    private const string _apiUrl = "PUT_YOUR_API_URL_HERE";
    private APIService()
    {
        Client = new HttpClient
        {
            BaseAddress = new Uri(_apiUrl)
        };
    }
    
}