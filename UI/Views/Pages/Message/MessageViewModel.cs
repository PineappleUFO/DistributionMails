using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using EF.Repositories;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;
using UI.Helpers;
using UI.Views.Components;
using UI.Views.Pages.Distribution;
using UI.Views.Pages.MainForms.Input;
using static iTextSharp.text.pdf.AcroFields;

namespace UI.Views.Pages.Message;

public partial class MessageViewModel : ObservableObject, IQueryAttributable
{ 
    [ObservableProperty]
    MailWrapper selectedMail;

    [ObservableProperty]
    User currentUser;


    [ObservableProperty]
    ObservableCollection<TreeItem> distributionTreeSource = new();


    /// <summary>
    /// Комманда открытия формы распределения
    /// </summary>
    /// <param name="mail">Модель письма</param>
    [RelayCommand]
    public  void OpenDistributionPage(MailWrapper mail)
    {
    }

    /// <summary>
    /// Удалить
    /// </summary>
    [RelayCommand]
    public void Remove(TreeItem item)
    {
     
        treeRep.DeleteUserFromTree(item.Id);
        LoadTree();
    }


    /// <summary>
    /// Перенести срок исполнителя
    /// </summary>
    [RelayCommand]
    public async void ChangeDeadline(TreeItem item)
    {
        await Shell.Current.GoToAsync(nameof(ChangeDateModal), new Dictionary<string, object>()
        {
            ["TreeItem"] = item,
            ["TreeRepository"] = treeRep,
            ["SelectedDate"] = item.Deadline
        }) ;
        LoadTree();
    }

    /// <summary>
    /// Назначить отвечающим
    /// </summary>
    [RelayCommand]
    public void GetReplying(TreeItem item)
    {
        treeRep.SetReplyingInTree(item.Id);
        LoadTree();
    }

    /// <summary>
    /// Назначить ответсвенным
    /// </summary>
    [RelayCommand]
    public void GetResponsible(TreeItem item) 
    {
        treeRep.SetResponibleInTree(item.Id);
        LoadTree();
    }

    /// <summary>
    /// Кнопка "выполнено"
    /// </summary>
    [RelayCommand]
    public void MyCompleted(TreeItem item)
    {
        treeRep.SetDone(item.Id);
        LoadTree();
    }

    /// <summary>
    /// Кнопка "принято"
    /// </summary>
    [RelayCommand]
    public void MyAccept(TreeItem item)
    {
        treeRep.SetAccept(item.Id);
        LoadTree();
    }

    /// <summary>
    /// Кнопка "Добавить исполнителя"
    /// </summary>
    [RelayCommand]
    public async void ChangeMyDistribution(TreeItem s)
    {
        //todo: service Current user
        await Shell.Current.GoToAsync($"{nameof(DistributionPage)}", new Dictionary<string, object>()
        {
            ["SelectedMail"] = SelectedMail.Mail,
            ["SelectedUserFrom"] = s.User,
            ["CurrentUser"] = CurrentUser,
            ["SelectedTreeItem"] = s
        }) ;
        LoadTree();
    }

    /// <summary>
    /// Кнопка "добавить распределение"
    /// </summary>
    [RelayCommand]
    public async void AddFirstLevel()
    {
       //todo: service Current user
         await Shell.Current.GoToAsync($"{nameof(DistributionPage)}", new Dictionary<string, object>()
        {
            ["SelectedMail"] = SelectedMail.Mail,
            ["CurrentUser"] = CurrentUser
         });

         LoadTree();
    }

    public MessageViewModel()
    {
       
    }
    TreeRepository treeRep;
    private async void LoadTree()
    {
        treeRep =new TreeRepository(TestHelper.GetConnectionSingltone());
        DistributionTreeSource = TreeHelper.GenerateTreeFromDbData(await  treeRep.GetTreeByMailId(SelectedMail.Mail));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        SelectedMail = query["SelectedMail"] as MailWrapper;
        CurrentUser = query["CurrentUser"] as User;
        LoadTree();
    }
}
