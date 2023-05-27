using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Models;
using EF.Repositories;
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
        var userRepository = ServiceHelper.GetService<UserRepository>();
        var depRepository = ServiceHelper.GetService<DepRepository>();
        var positionRepository = ServiceHelper.GetService<PositionRepository>();

        User user = await userRepository.TryGetUserByLogin(Login, Password, positionRepository, depRepository);

        
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