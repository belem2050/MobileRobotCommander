using Microsoft.Extensions.Logging;
using MobileRobotCommander.ViewModels;
using MobileRobotCommander.Views;
using CommunityToolkit.Maui;

namespace MobileRobotCommander
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPageVm>();
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddTransient<AboutPage>();
            builder.Services.AddTransient<AboutPageVm>();

            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsPageVm>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
