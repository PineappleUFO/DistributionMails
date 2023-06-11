using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Models;
using EF.Interfaces;
using EF.Repositories;
using PostgresRepository.PostgresCommon;
using UI.Helpers;
using UI.Messengers;
using UI.Views.Pages.MainForms.Input;

namespace UI.Views.Pages.Login;

[INotifyPropertyChanged]
public partial class LoginViewModel
{
    /// <summary>
    /// Логин
    /// </summary>
    [ObservableProperty] public string login = "zakharovdb";
    
    /// <summary>
    /// Пароль
    /// </summary>
    [ObservableProperty] public string password="zakharovdb";
    
    /// <summary>
    /// Идет ли операция обращения к бд
    /// </summary>
    [ObservableProperty] public bool isBusy;

    private readonly IMessenger _messenger;

    public LoginViewModel()
    {
        _messenger = ServiceHelper.GetService<IMessenger>();
    }

    [RelayCommand]
    async void TryLogin()
    {
        IsBusy = true;
        await Task.Delay(200);
        var userRepository = new UserRepository(TestHelper.GetConnectionSingltone());

     

        User user = await userRepository.TryGetUserByLogin(Login, Password);
        
        if (user != null)
        {
             await Shell.Current.GoToAsync($"{nameof(InputMailMain)}",new Dictionary<string, object>()
            {
                ["CurrentUser"] = user
            });
        }
        else
        {
            var message = new DisplayAlertMessage()
            {
                Title = "Ошибка - Не найден пользователь в бд",
                Message = "Неверный логин или пароль",
                Cancel = "Ок"
            };
            _messenger.Send(message);
        }

        IsBusy = false;
    }

}