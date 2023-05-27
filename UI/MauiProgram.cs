﻿using CommunityToolkit.Mvvm.Messaging;
using EF.Repositories;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using UI.Views.Components;
using UI.Views.Pages.Distribution;
using UI.Views.Pages.Login;
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
                .UseUraniumUIMaterial()
                .ConfigureSyncfusionCore();

            //todo: Заменить на DI из communirt.mvvm.toolkit
            
            //ViewModels
            builder.Services.AddSingleton<InputMailMainViewModel>();
            builder.Services.AddTransient<MessageViewModel>();
            builder.Services.AddTransient<DistributionViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            
            //Repositories
            builder.Services.AddSingleton<PositionRepository>();
            builder.Services.AddSingleton<DepRepository>();
            builder.Services.AddSingleton<UserRepository>();
            
            //Messenger
            builder.Services.AddSingleton<IMessenger>(new StrongReferenceMessenger());

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}