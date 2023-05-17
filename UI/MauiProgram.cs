using Microsoft.Extensions.Logging;
using UI.Views.Pages.MainForms.Input;
using UI.Views.Pages.Message;
using UraniumUI;

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
                    fonts.AddFontAwesomeIconFonts();
                })
                .UseUraniumUI()
                .UseUraniumUIMaterial();

            builder.Services.AddSingleton<InputMailMainViewModel>();
            builder.Services.AddTransient<MessageViewModel>();
            

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}