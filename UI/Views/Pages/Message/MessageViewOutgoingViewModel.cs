using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using UI.Extenstions;
using UI.Helpers;

namespace UI.Views.Pages.Message
{
    public partial class MessageViewOutgoingViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty] OutgoingMail selectedMail;
        [ObservableProperty] public string currentFilePath;
        /// <summary>
        /// Все вложения выбранного письма
        /// </summary>
        [ObservableProperty] public List<string> currentFiles;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            SelectedMail = query["SelectedMail"] as OutgoingMail;

            CurrentFilePath = null;
            CurrentFiles = null;
            OnPropertyChanged(nameof(CurrentFilePath));
            OnPropertyChanged(nameof(CurrentFiles));
            if (SelectedMail == null || SelectedMail.PathFolder == null) return;
            var pdfFiles = SelectedMail.PathFolder.GetAllPdfInFolder();

            //todo: Если нет файлов то показываем плашку
            if (pdfFiles.Count > 0)
            {
                CurrentFiles = pdfFiles.Select(a => a.FullName).ToList();
                CurrentFilePath = pdfFiles[0].FullName;
                OnPropertyChanged(nameof(CurrentFilePath));
            }
        }

      
    }
}
