using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UI.Views.Pages.Distribution;
using UI.Views.Pages.MainForms.Input;

namespace UI.Views.Pages.Message;

[QueryProperty("SelectedMail","SelectedMail")]
public partial class MessageViewModel : ObservableObject
{ 
    [ObservableProperty]
    MailModel selectedMail;


    /// <summary>
    /// Комманда открытия формы распределения
    /// </summary>
    /// <param name="mail">Модель письма</param>
    [RelayCommand]
    public  void OpenDistributionPage(MailModel mail)
    {
         Shell.Current.GoToAsync($"{nameof(DistributionPage)}", new Dictionary<string, object>()
        {
            ["SelectedMail"] = mail
        });
    }

    public MessageViewModel()
    {
        
    }
}