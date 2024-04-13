using Microsoft.Extensions.Http;
 
using Syncfusion.Maui.Core.Hosting;
using mobilewakala.Shared;
using mobilewakala.Services;
using Microsoft.Extensions.Logging;
namespace mobilewakala
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                    .ConfigureSyncfusionCore()

                 .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "Material");
                    fonts.AddFont("Roboto-Black.ttf", "FontBlack");
                    fonts.AddFont("Roboto-BlackItalic.ttf", "FontBlackItalic");
                    fonts.AddFont("Roboto-Bold.ttf", "FontBold");
                    fonts.AddFont("Roboto-BoldItalic.ttf", "FontBoldItalic");
                    fonts.AddFont("Ruboto-Italic.ttf", "FontItalic");
                    fonts.AddFont("Ruboto-Light.ttf", "FontLight");
                    fonts.AddFont("Ruboto-LightItalic.ttf", "FontLightItalic");
                    fonts.AddFont("Roboto-Medium.ttf", "FontMedium");
                    fonts.AddFont("Roboto-MediumItalic.ttf", "FontMediumItalic");
                    fonts.AddFont("Roboto-Regular.ttf", "FontRegular");
                    fonts.AddFont("Roboto-Thin.ttf", "FontThin");
                    fonts.AddFont("Roboto-ThinItalic.ttf", "FontThinItalic");
                })
                 .UseMauiMaps();

            builder.Services.AddHttpClient("api", httpClient =>
            {
                httpClient.BaseAddress = new Uri("http://localhost:5217/WeatherForecast");
            });
            builder.Services.AddSingleton<IAgentServices, AgentServices>();
            builder.Services.AddSingleton<HttpClient>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
