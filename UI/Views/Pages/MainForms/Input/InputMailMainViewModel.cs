using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Views.Pages.MainForms.Input
{
    public class MailModel
    {
        [DisplayName("Номер письма")]
        public string NumberMail { get; set; }
        [DisplayName("Дата получения")]
        public DateTime DateInput { get; set; }
        [DisplayName("Проект")]
        public string Project { get; set; }
        [DisplayName("Отправитель")]
        public string Sender { get; set; }
        [DisplayName("Тема")]
        public string Theme { get; set; }
        [DisplayName("Номер исх.")]
        public string NumberOut { get; set; }
        [DisplayName("Срок ответа")]
        public DateTime DateOut { get; set; }
    }
    [INotifyPropertyChanged]
    public partial class InputMailMainViewModel
    {
        [ObservableProperty]
        public ObservableCollection<MailModel> listSourceMail;

        [ObservableProperty]
        public MailModel selectedMail;

        public InputMailMainViewModel()
        {
            listSourceMail = new ObservableCollection<MailModel>();

            var m = new MailModel()
            {
                DateInput = DateTime.Now.AddDays(-4),
                DateOut = DateTime.Now.AddDays(-2),
                NumberMail = "1001",
                NumberOut = "10-0",
                Project = "project",
                Sender = "sender",
                Theme = "trheme"
            };
            for (int i = 0; i < 5; i++)
            {
                listSourceMail.Add(m);
            }
        }


    }
}
