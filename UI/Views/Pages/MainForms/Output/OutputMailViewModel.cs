using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using PostgresRepository.Interfaces;
using PostgresRepository.Repositories;
using System.Collections.ObjectModel;
using UI.Extenstions;
using UI.Helpers;
using UI.Views.Pages.Message;

namespace UI.Views.Pages.MainForms.Output
{
    public partial class OutputMailViewModel : ObservableObject
    {
        IOutgoingRepository oRep = new OutgoingMailRepository(TestHelper.GetConnectionSingltone());
        [ObservableProperty] public ObservableCollection<OMailWrapper> filteredSourceMail = new();
        [ObservableProperty] ObservableCollection<OMailWrapper> outgoingMailList = new();
        [ObservableProperty] bool isLoading;
        [ObservableProperty] OMailWrapper selectedMail;
        public SearchModesEnum SearchModesEnum { get; set; }
        /// <summary>
        /// Путь на выбранный файл
        /// </summary>
        [ObservableProperty] public string currentFilePath;
        /// <summary>
        /// Все вложения выбранного письма
        /// </summary>
        [ObservableProperty] public List<string> currentFiles;
        public OutputMailViewModel()
        {
            Init();
        }

        partial void OnSelectedMailChanged(OMailWrapper value)
        {
            CurrentFilePath = null;
            CurrentFiles = null;
            OnPropertyChanged(nameof(CurrentFilePath));
            OnPropertyChanged(nameof(CurrentFiles));
            if (value == null || value.Mail.PathFolder == null) return;
            var pdfFiles = value.Mail.PathFolder.GetAllPdfInFolder();

            //todo: Если нет файлов то показываем плашку
            if (pdfFiles.Count > 0)
            {
                CurrentFiles = pdfFiles.Select(a => a.FullName).ToList();
                CurrentFilePath = pdfFiles[0].FullName;
                OnPropertyChanged(nameof(CurrentFilePath));
            }
        }
        /// <summary>
        /// Комманда открытия письма
        /// </summary>
        /// <param name="mail">Модель письма</param>
        [RelayCommand]
        public async void OpenMail()
        {
            await Shell.Current.GoToAsync($"{nameof(MessageViewOutgoing)}", new Dictionary<string, object>()
            {
                ["SelectedMail"] = SelectedMail.Mail,
            });
        }
        public async void Init()
        {
            IsLoading = true;
            await Task.Delay(200);
            //todo: user service
            foreach (OutgoingMail mail in await oRep.GetAllOutputMail())
            {
                OutgoingMailList.Add(new OMailWrapper() { Mail = mail, IsSelected = false });
            }
            FilteredSourceMail = OutgoingMailList;
            IsLoading = false;
        }

        [RelayCommand]
        public async void OpenInputMail()
        {
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Команда поиска
        /// </summary>
        /// <param name="text"></param>
        [RelayCommand]
        public async void TextSearch(string text)
        {
            //todo: оптимизировать


            if (text.Length > 0)
            {
                switch (SearchModesEnum)
                {
                    case SearchModesEnum.Smart:
                        FilteredSourceMail = new ObservableCollection<OMailWrapper>(
                            OutgoingMailList.Where(a => a.Mail.Theme.ToLower().Contains(text.ToLower()) ||
                             a.Mail.Number.ToLower().Contains(text.ToLower()) ||
                             a.Mail.Sender.Name.ToLower().Contains(text.ToLower()) ||
                             a.Mail.Project.Name.ToLower().Contains(text.ToLower()) ||
                             a.Mail.DateExport.ToString("dd.MM.yyyy").ToLower().Contains(text.ToLower()) ||
                             a.Mail.Theme.ToLower().Contains(text.ToLower())
                        ).ToList());
                        break;
                    case SearchModesEnum.Theme:
                        FilteredSourceMail = new ObservableCollection<OMailWrapper>(
                           OutgoingMailList.Where(a => a.Mail.Theme.ToLower().Contains(text.ToLower())).ToList());
                        break;
                    case SearchModesEnum.Number:
                        FilteredSourceMail = new ObservableCollection<OMailWrapper>(
                        OutgoingMailList.Where(a => a.Mail.Number.ToLower().Contains(text.ToLower())).ToList());
                        break;
                    case SearchModesEnum.Sender:
                        FilteredSourceMail = new ObservableCollection<OMailWrapper>(
                        OutgoingMailList.Where(a => a.Mail.Sender.Name.ToLower().Contains(text.ToLower())).ToList());
                        break;
                    case SearchModesEnum.Project:
                        FilteredSourceMail = new ObservableCollection<OMailWrapper>(
                        OutgoingMailList.Where(a => a.Mail.Project.Name.ToLower().Contains(text.ToLower())).ToList());
                        break;
                    case SearchModesEnum.Date:
                        FilteredSourceMail = new ObservableCollection<OMailWrapper>(
                        OutgoingMailList.Where(a => a.Mail.DateExport.ToString("dd.MM.yyyy").ToLower().Contains(text.ToLower())).ToList());
                        break;
                    default:
                        break;
                }

            }
            else
            {
                FilteredSourceMail = OutgoingMailList;
            }
        }


    }
}
