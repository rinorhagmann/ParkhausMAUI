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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); // Schriftart
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<ParkingService>(); // Anbindung Service

            builder.Services.AddTransient<MainViewModel>(); // Anbindung ViewModel MainPage

            builder.Services.AddTransient<MainPage>(); // Anbindung View MainPage

            builder.Services.AddTransient<ActiveParkingViewModel>(); // Anbindung ViewModel ActiveParkingPage

            builder.Services.AddTransient<ActiveParkingPage>(); // Anbindung View ActiveParkingPage

            builder.Services.AddTransient<HistoryViewModel>(); // Anbindung ViewModel HistoryPage

            builder.Services.AddTransient<HistoryPage>(); // Anbindung View HistoryPage

#if DEBUG
            builder.Logging.AddDebug(); // Debugging
#endif

            return builder.Build(); // Build der App
        }
    }
}