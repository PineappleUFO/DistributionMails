using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EF.Repositories;
using System.Collections.ObjectModel;
using UI.Helpers;
using UI.Views.Pages.Distribution;
using UI.Views.Pages.MainForms.Input;
using static iTextSharp.text.pdf.AcroFields;

namespace UI.Views.Pages.Message;

[QueryProperty("SelectedMail","SelectedMail")]
public partial class MessageViewModel : ObservableObject
{ 
    [ObservableProperty]
    MailWrapper selectedMail;

    [ObservableProperty]
    ObservableCollection<TreeItem> distributionTreeSource = new();

    /// <summary>
    /// Комманда открытия формы распределения
    /// </summary>
    /// <param name="mail">Модель письма</param>
    [RelayCommand]
    public  void OpenDistributionPage(MailWrapper mail)
    {
         Shell.Current.GoToAsync($"{nameof(DistributionPage)}", new Dictionary<string, object>()
        {
            ["SelectedMail"] = mail
        });
    }

    public MessageViewModel()
    {
        LoadTree();
    }

    private async void LoadTree()
    {
        var treeRep =ServiceHelper.GetService<TreeRepository>();
        DistributionTreeSource = TreeHelper.GenerateTreeFromDbData(await  treeRep.GetTreeByMailId(2));
    }
}
