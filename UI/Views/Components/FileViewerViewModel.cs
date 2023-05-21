using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UI.Extenstions;
using UI.Helpers;

namespace UI.Views.Components;

public partial class FileViewerViewModel : ObservableObject
{
    [ObservableProperty] public string filePath;

    /// <summary>
    /// Текущая страница PDF
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
    [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
    public int currentPage = 1;

    [ObservableProperty] public HtmlWebViewSource htmlContent;


    public FileViewerViewModel(string path)
    {
        FilePath = path;
        HtmlContent = new HtmlWebViewSource();
        LoadPdf();
    }

    private void LoadPdf()
    {
        if (!FilePath.CheckFilePath()) return;

        // Получение содержимого выбранной страницы
        var pageContent = PdfFileHelper.GetPageContent(FilePath, CurrentPage);
        var base64String = Convert.ToBase64String(pageContent);
        HtmlContent.Html =$"<html><body><object width='100%' height='100%' data='data:application/pdf;base64,{base64String}'></object></body></html>";
    }

    public bool CanNavigateBack()
    {
        return CurrentPage > 1;
    }
    
    public bool CanNavigateNext()
    {
        if (!FilePath.CheckFilePath()) return false;
        return CurrentPage < PdfFileHelper.GetPageCount(FilePath);
    }

    /// <summary>
    /// Предыдущая страница
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanNavigateBack))]
    public void PreviousPage()
    {

        if (CurrentPage > 1)
        {
            CurrentPage--;
            LoadPdf();
        }
    }

    /// <summary>
    /// Следующая страница
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanNavigateNext))]
    public void NextPage()
    {

        if (CurrentPage < PdfFileHelper.GetPageCount(FilePath))
        {
            CurrentPage++;
            LoadPdf();
        }
        
    }

  
}