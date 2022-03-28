using Newtonsoft.Json.Linq;
using System.Reflection;

namespace MAUICherieApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
        setAPIUrlBySettings();
        DependencyService.Register<AndroidNotificationManager>();
        var app = builder.Build();
        return app;
    }

    private static void setAPIUrlBySettings()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resName = assembly.GetManifestResourceNames()
            ?.FirstOrDefault(r => r.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase));
        using var file = assembly.GetManifestResourceStream(resName);
        using var sr = new StreamReader(file);
        var json = sr.ReadToEnd();
        var j = JObject.Parse(json);
        Constants.API_Url = j["Settings"]["APIUrl"].Value<string>();
    }
}