using Android.Content;
using MAUICherieApp.Platforms.Android;
using AndroidApp = Android.App.Application;

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
        DependencyService.Register<AndroidNotificationManager>();
        return builder.Build();
    }
}