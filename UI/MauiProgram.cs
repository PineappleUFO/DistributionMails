using Microsoft.Extensions.Logging;

namespace UI
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
                    fonts.AddFont("RobotoSlab-Regular.ttf", "RobotoSlabRegular");
                    fonts.AddFont("RobotoSlab-Light.ttf", "RobotoSlabLight");
                    fonts.AddFont("RobotoSlab-Bold.ttf", "RobotoSlabBold");
                    fonts.AddFont("FontAwesomeRegular.otf", "FontRegular");
                    fonts.AddFont("FontAwesomeBrands.otf", "FontBrands");
                    fonts.AddFont("FontAwesomeSolid.otf", "FontSolid");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}