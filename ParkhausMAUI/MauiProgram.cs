using Microsoft.Extensions.Logging;
using ParkhausMAUI.Views;
using ParkhausMAUI.ViewModels;
using ParkhausMAUI.Services;

namespace ParkhausMAUI
{
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<ParkingService>(); // Anbindung Service

            builder.Services.AddTransient<MainViewModel>(); // Anbindung ViewModel

            builder.Services.AddTransient<MainPage>(); // Anbindung View

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}