using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using UI.Views.Pages.Message;

namespace UI.Views.Pages.MainForms.Input
{
    [INotifyPropertyChanged]
    public partial class InputMailMainViewModel
    {
        [ObservableProperty]
        public ObservableCollection<MailModel> listSourceMail;

        [ObservableProperty]
        public MailModel selectedMail;

        [RelayCommand]
        public async void OpenMailCommand(MailModel mail)
        {
            await Shell.Current.GoToAsync($"{nameof(MessageView)}",new Dictionary<string, object>()
            {
                ["SelectedMail"] = mail
            });
        }

        public InputMailMainViewModel()
        {
            listSourceMail = new ObservableCollection<MailModel>();

          
            for (int i = 0; i < 5; i++)
            {
                var m = new MailModel()
                {
                    DateInput = DateTime.Now.AddDays(-4),
                    DateOut = DateTime.Now.AddDays(-2),
                    NumberMail = "1001",
                    NumberOut = "10-0",
                    Project = "project",
                    Sender = "sender",
                    Theme = "trheme",
                    IdMail = i
                };
                listSourceMail.Add(m);
            }
        }


    }
}
