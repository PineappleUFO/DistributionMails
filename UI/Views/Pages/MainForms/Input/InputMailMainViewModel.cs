using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using UI.Views.Pages.Message;
using UI.Extenstions;
using Microsoft.Maui.ApplicationModel.Communication;

namespace UI.Views.Pages.MainForms.Input
{
    [INotifyPropertyChanged]
    public partial class InputMailMainViewModel
    {
        /// <summary>
        /// Коллекция писем
        /// </summary>
        [ObservableProperty] public ObservableCollection<MailModel> listSourceMail;

        /// <summary>
        /// Текущее выбранное письмо
        /// </summary>
        [ObservableProperty] public MailModel selectedMail;

        /// <summary>
        /// Поток для предпросмоторщика вложений
        /// </summary>
        [ObservableProperty] public Stream documentStream;

        /// <summary>
        /// Путь на выбранный файл
        /// </summary>
        [ObservableProperty] public string currentFilePath;

        /// <summary>
        /// Все вложения выбранного письма
        /// </summary>
        [ObservableProperty] public List<string> currentFiles;


        /// <summary>
        /// Комманда открытия письма
        /// </summary>
        /// <param name="mail">Модель письма</param>
        [RelayCommand]
        public async void OpenMailCommand(MailModel mail)
        {
            await Shell.Current.GoToAsync($"{nameof(MessageView)}",new Dictionary<string, object>()
            {
                ["SelectedMail"] = mail
            });
        }

        /// <summary>
        /// Комманда открытия выбранного пользователем вложения
        /// </summary>
        [RelayCommand]
        public void OpenPreviewFileCommand(object filePath)
        {
                CurrentFilePath = filePath.ToString();
        }

        partial void OnSelectedMailChanged(MailModel value)
        {

            var pdfFiles = value.PathFolder.GetAllPdfInFolder();
            CurrentFilePath = null;
            //todo: Если нет файлов то показываем плашку
            if (pdfFiles.Count > 0)
            {
                CurrentFiles = pdfFiles.Select(a => a.FullName).ToList();
                CurrentFilePath = pdfFiles[0].FullName;
                OnPropertyChanged(nameof(CurrentFilePath));
            }
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
                    IdMail = i,
                    PathFolder = new DirectoryInfo(@"C:\Диплом\Test")
                };
                listSourceMail.Add(m);
            }
        }


      


    }
}
