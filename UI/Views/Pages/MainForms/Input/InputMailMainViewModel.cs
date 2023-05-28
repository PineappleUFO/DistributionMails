using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using EF.Repositories;
using UI.Views.Pages.Message;
using UI.Extenstions;
using Microsoft.Maui.ApplicationModel.Communication;
using PostgresRepository.Interfaces;
using UI.Helpers;

namespace UI.Views.Pages.MainForms.Input
{

    public partial class InputMailMainViewModel : ObservableObject,IQueryAttributable
    {
        /// <summary>
        /// Коллекция писем
        /// </summary>
        [ObservableProperty] public ObservableCollection<MailWrapper> listSourceMail;

        /// <summary>
        /// Текущее выбранное письмо
        /// </summary>
        [ObservableProperty] public MailWrapper selectedMail;

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

        [ObservableProperty] public User currentUser;


        /// <summary>
        /// Комманда открытия письма
        /// </summary>
        /// <param name="mail">Модель письма</param>
        [RelayCommand]
        public async void OpenMail(object mail)
        {
            await Shell.Current.GoToAsync($"{nameof(MessageView)}", new Dictionary<string, object>()
            {
                ["SelectedMail"] = mail
            });
        }

     

        /// <summary>
        /// Комманда открытия выбранного пользователем вложения
        /// </summary>
        [RelayCommand]
        public void OpenPreviewFile(object filePath)
        {
            CurrentFilePath = filePath.ToString();
        }

        partial void OnSelectedMailChanged(MailWrapper value)
        {
         //   if (value == null) return;
         ////   var pdfFiles = value.PathFolder.GetAllPdfInFolder();
         //   CurrentFilePath = null;
         //   //todo: Если нет файлов то показываем плашку
         //   if (pdfFiles.Count > 0)
         //   {
         //       CurrentFiles = pdfFiles.Select(a => a.FullName).ToList();
         //       CurrentFilePath = pdfFiles[0].FullName;
         //       OnPropertyChanged(nameof(CurrentFilePath));
         //   }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CurrentUser = query["CurrentUser"] as User;
            
        }

        public InputMailMainViewModel()
        {
            listSourceMail = new ObservableCollection<MailWrapper>();


            //todo: поменять на сервисы
            var mailsRep = ServiceHelper.GetService<MailRepository>();
            

            LoadCollection(mailsRep);
        }

        private async void LoadCollection(IMailRepository repository)
        {
            var mails = await repository.GetAllMails();
            foreach (Mail mail in mails)
            {
                ListSourceMail.Add(new MailWrapper() { Mail = mail,IsSelected=false});
            }
        }
    }
}