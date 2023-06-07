using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using EF.Repositories;
using System.Collections.ObjectModel;
using UI.Helpers;

namespace UI.Views.Pages.Distribution
{
    public partial class DistributionViewModel : ObservableObject, IQueryAttributable
    {
        private User currentUser;
        private User selectedUserFrom;
        private Mail selectedMail;
        private TreeItem selectedTreeItem;
        [ObservableProperty] DistributionItem selectedUser;
        [ObservableProperty] public bool isBusy;
        [ObservableProperty] public ObservableCollection<DistributionItem> userSource = new();
        [ObservableProperty] public ObservableCollection<DistributionItem> selectedUserSource = new();

        [RelayCommand]
        public async void GetAllUsers()
        {
            IsBusy = true;
            var uRep = new UserRepository(TestHelper.GetConnectionSingltone());
            var users = await uRep.GetAllUsers();
            foreach (var user in users.Where(user => user != null))
            {
                UserSource.Add(new DistributionItem() { User = user });
            }
            IsBusy = false;
        }
        TreeRepository treeRep = new TreeRepository(TestHelper.GetConnectionSingltone());

        [RelayCommand]
        public void Save()
        {
            //todo:message back notification
            if (SelectedUserSource.Count == 0) return;
            
            foreach (var item in SelectedUserSource)
            {
                if (selectedUserFrom != null)
                {
                    treeRep.AddDistributionInMail(selectedMail, selectedTreeItem.Id, item.User, item.Deadline, item.Resolution, item.IsResponsible, item.IsReplying);

                }
                else
                {
                    treeRep.AddOneLevelDistributionInMail(selectedMail, item.User, item.Deadline, item.Resolution, item.IsResponsible, item.IsReplying);
                }
            }



        }




        [RelayCommand]
        public async void Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public void CheckUser(DistributionItem user)
        {
            if (user.IsChecked)
            {
                if (!SelectedUserSource.Contains(user))
                    SelectedUserSource.Add(user);
            }
            else
            {
                SelectedUserSource.Remove(user);
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            selectedMail = query["SelectedMail"] as Mail;
            selectedUserFrom = query["SelectedUserFrom"] as User;
            currentUser = query["CurrentUser"] as User;
            selectedTreeItem = query["SelectedTreeItem"] as TreeItem;
        }

        public DistributionViewModel()
        {
            GetAllUsersCommand.Execute(null);
        }
    }
}
