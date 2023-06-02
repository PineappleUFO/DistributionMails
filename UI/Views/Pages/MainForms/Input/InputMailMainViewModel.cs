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
using EF.Interfaces;
using PostgresRepository.PostgresCommon;
using PostgresRepository.Repositories;

namespace UI.Views.Pages.MainForms.Input
{

    public partial class InputMailMainViewModel : ObservableObject,IQueryAttributable
    {
        [ObservableProperty] public ObservableCollection<MailWrapper> filteredSourceMail = new();

        /// <summary>
        /// Коллекция писем
        /// </summary>
        [ObservableProperty] public ObservableCollection<MailWrapper> listSourceMail = new();

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

        /// <summary>
        /// Текущий режим
        /// </summary>
        [ObservableProperty] public EnumModes currentMode;

        /// <summary>
        /// Идет ли загрузка
        /// </summary>
        [ObservableProperty] public bool isLoading;

        /// <summary>
        /// Текущий пользователь  todo:заменить на сервис
        /// </summary>
        [ObservableProperty] public User currentUser;

        /// <summary>
        /// Список прав на конф. разделы
        /// </summary>
        [ObservableProperty] public List<MailType> userAccessMailTypeList;

        public SearchModesEnum SearchModesEnum { get; set; }

        /// <summary>
        /// Комманда открытия письма
        /// </summary>
        /// <param name="mail">Модель письма</param>
        [RelayCommand]
        public async void OpenMail(MailWrapper mail)
        {
            await Shell.Current.GoToAsync($"{nameof(MessageView)}", new Dictionary<string, object>()
            {
                ["SelectedMail"] = mail
            }); 
        }

        MailRepository mailsRep;
        public InputMailMainViewModel()
        {
             mailsRep = new MailRepository(TestHelper.GetConnectionSingltone());
         

        }

        private async void Init()
        {
            var mailTypeRep = new MailTypeRepository(TestHelper.GetConnectionSingltone());
            UserAccessMailTypeList = await mailTypeRep.GetTypesAccessByUser(CurrentUser);
           
        }

        /// <summary>
        /// Комманда загрузки писем в зависимости от выбранной конф. папки
        /// </summary>
        /// <param name="mailType"></param>
        [RelayCommand]
        public async void LoadMailByType(MailType mailType)
        {
            ListSourceMail.Clear();
            IsLoading = true;
            await Task.Delay(200);
            CurrentMode = EnumModes.None;
            //todo: user service
            foreach (Mail mail in await mailsRep.GetMailsByType(mailType))
            {
                ListSourceMail.Add(new MailWrapper() { Mail = mail, IsSelected = false });
            }
            IsLoading = false;
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
            Init();
        }


        /// <summary>
        /// Загрузить избранные письма пользователя
        /// </summary>
        [RelayCommand]
        public async void LoadFavorite()
        {
            ListSourceMail.Clear();
            IsLoading = true;
            await Task.Delay(200);
            CurrentMode = EnumModes.Favorite;
            //todo: user service
            foreach (Mail mail in await mailsRep.GetFavoriteUser(new User() { Id = 157 }))
            {
                ListSourceMail.Add(new MailWrapper() { Mail = mail, IsSelected = false });
                FilteredSourceMail = ListSourceMail;
            }
            IsLoading = false;
        }

        /// <summary>
        /// Загрузить архив писем пользователя
        /// </summary>
        [RelayCommand]
        public async void LoadArchive()
        {
            ListSourceMail.Clear();
            IsLoading = true;
            await Task.Delay(200);
            CurrentMode = EnumModes.Archive;
            //todo: user service
            foreach (Mail mail in await mailsRep.GetArchiveUser(new User() { Id = 157 }))
            {
                ListSourceMail.Add(new MailWrapper() { Mail = mail, IsSelected = false });
                FilteredSourceMail = ListSourceMail;
            }
            IsLoading = false;
        }

        /// <summary>
        /// Загрузить все распределенные пользователю письма
        /// </summary>
        [RelayCommand]
        public async void LoadDistibutinToMe()
        {
            ListSourceMail.Clear();
            IsLoading = true;
            await Task.Delay(200);
            CurrentMode = EnumModes.DistributedToMe;
            //todo: user service
            foreach (Mail mail in await mailsRep.GetDistributedToUser(new User() { Id=157}))
            {
                ListSourceMail.Add(new MailWrapper() { Mail = mail, IsSelected = false });
                FilteredSourceMail = ListSourceMail;
            }
            IsLoading = false;
        }

        /// <summary>
        /// Загрузить все письма
        /// </summary>
        [RelayCommand]
        public async void LoadAll()
        {
            ListSourceMail.Clear();
            IsLoading = true;
            await Task.Delay(200);
            CurrentMode = EnumModes.All;
            //todo: поменять на сервисы
            foreach (Mail mail in await mailsRep.GetAllMails())
            {
                ListSourceMail.Add(new MailWrapper() { Mail = mail, IsSelected = false });
                FilteredSourceMail = ListSourceMail;
            }
            IsLoading = false;

        }
        /// <summary>
        /// Команда поиска
        /// </summary>
        /// <param name="text"></param>
        [RelayCommand]
        public async void TextSearch(string text)
        {
            //todo: оптимизировать


            if(text.Length > 0)
            {
                switch (SearchModesEnum)
                {
                    case SearchModesEnum.Smart:
                        FilteredSourceMail = new ObservableCollection<MailWrapper>(
                            ListSourceMail.Where(a => a.Mail.Theme.Contains(text) ||
                             a.Mail.Number.Contains(text) ||
                             a.Mail.Sender.Name.Contains(text) ||
                             a.Mail.Project.Name.Contains(text) ||
                             a.Mail.DateInput.ToString("dd.MM.yyyy").Contains(text) ||
                             a.Mail.Theme.Contains(text) 
                        ).ToList());
                        break;
                    case SearchModesEnum.Theme:
                        FilteredSourceMail = new ObservableCollection<MailWrapper>(
                           ListSourceMail.Where(a => a.Mail.Theme.Contains(text)).ToList());
                        break;
                    case SearchModesEnum.Number:
                        FilteredSourceMail = new ObservableCollection<MailWrapper>(
                        ListSourceMail.Where(a => a.Mail.Number.Contains(text)).ToList());
                        break;
                    case SearchModesEnum.Sender:
                        FilteredSourceMail = new ObservableCollection<MailWrapper>(
                        ListSourceMail.Where(a => a.Mail.Sender.Name.Contains(text)).ToList());
                        break;
                    case SearchModesEnum.Project:
                        FilteredSourceMail = new ObservableCollection<MailWrapper>(
                        ListSourceMail.Where(a => a.Mail.Project.Name.Contains(text)).ToList());
                        break;
                    case SearchModesEnum.Date:
                        FilteredSourceMail = new ObservableCollection<MailWrapper>(
                        ListSourceMail.Where(a => a.Mail.DateInput.ToString("dd.MM.yyyy").Contains(text)).ToList());
                        break;
                    default:
                        break;
                }
               
            }
            else
            {
                FilteredSourceMail = ListSourceMail;
            }
        }
     
    }
}