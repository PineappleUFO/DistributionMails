using CommunityToolkit.Mvvm.ComponentModel;
using UI.Views.Pages.MainForms.Input;

namespace UI.Views.Pages.Message;

[QueryProperty("SelectedMail","SelectedMail")]
public partial class MessageViewModel : ObservableObject
{ 
    [ObservableProperty]
    MailModel selectedMail;

    public MessageViewModel()
    {
        
    }
}