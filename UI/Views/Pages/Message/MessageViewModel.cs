using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using EF.Repositories;
using System.Collections.ObjectModel;
using UI.Helpers;
using UI.Views.Pages.Distribution;
using UI.Views.Pages.MainForms.Input;
using static iTextSharp.text.pdf.AcroFields;

namespace UI.Views.Pages.Message;

public partial class MessageViewModel : ObservableObject, IQueryAttributable
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

    /// <summary>
    /// Удалить
    /// </summary>
    [RelayCommand]
    public void Remove(TreeItem item)
    {
        //todo: service Current user
    }


    /// <summary>
    /// Перенести срок исполнителя
    /// </summary>
    [RelayCommand]
    public void ChangeDeadline(TreeItem item)
    {
        //todo: service Current user
    }

    /// <summary>
    /// Назначить отвечающим
    /// </summary>
    [RelayCommand]
    public void GetReplying(TreeItem item)
    {
        //todo: service Current user
    }

    /// <summary>
    /// Назначить ответсвенным
    /// </summary>
    [RelayCommand]
    public void GetResponsible(TreeItem item) 
    {
        //todo: service Current user
    }

    /// <summary>
    /// Кнопка "выполнено"
    /// </summary>
    [RelayCommand]
    public void MyCompleted(TreeItem item)
    {
        //todo: service Current user
    }

    /// <summary>
    /// Кнопка "принято"
    /// </summary>
    [RelayCommand]
    public void MyAccept(TreeItem item)
    {
        //todo: service Current user

    }

    /// <summary>
    /// Кнопка "изменить распределение"
    /// </summary>
    /// <param name="s"></param>
    [RelayCommand]
    public void ChangeMyDistribution(TreeItem s)
    {
        //todo: service Current user
        Shell.Current.GoToAsync($"{nameof(DistributionPage)}", new Dictionary<string, object>()
        {
            ["SelectedMail"] = SelectedMail,
            ["SelectedUserForm"] = s.User
        });
    }

    /// <summary>
    /// Кнопка "добавить распределение 1 уровня"
    /// </summary>
    [RelayCommand]
    public void AddFirstLevel(TreeItem s)
    {
       //todo: service Current user
        Shell.Current.GoToAsync($"{nameof(DistributionPage)}", new Dictionary<string, object>()
        {
            ["SelectedMail"] = SelectedMail,
            ["SelectedUserForm"] = s.User
        });
    }

    public MessageViewModel()
    {
       
    }

    private async void LoadTree()
    {
        var treeRep =new TreeRepository(TestHelper.GetConnectionSingltone());
        DistributionTreeSource = TreeHelper.GenerateTreeFromDbData(await  treeRep.GetTreeByMailId(SelectedMail.Mail));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        SelectedMail = query["SelectedMail"] as MailWrapper;
        LoadTree();
    }
}
