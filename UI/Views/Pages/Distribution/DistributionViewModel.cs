using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using EF.Repositories;
using UI.Helpers;
using UI.Views.Pages.MainForms.Input;
using Microsoft.Maui.ApplicationModel.Communication;

namespace UI.Views.Pages.Distribution
{
    [QueryProperty("SelectedMail", "SelectedMail")]
    [QueryProperty("SelectedUserFrom", "SelectedUserFrom")]
    public partial class DistributionViewModel : ObservableObject
    {
        [ObservableProperty] public User selectedUserFrom;
        [ObservableProperty] DistributionItem selectedUser;
        [ObservableProperty] public bool isBusy;
        [ObservableProperty] public ObservableCollection<DistributionItem> userSource = new();
        [ObservableProperty] public ObservableCollection<DistributionItem> selectedUserSource = new();

        [RelayCommand]
        public async void GetAllUsers()
        {
            IsBusy = true;
            var uRep = ServiceHelper.GetService<UserRepository>();
            var users = await uRep.GetAllUsers();
            foreach (var user in users.Where(user => user != null))
            {
                UserSource.Add(new DistributionItem() { User = user });
            }
            IsBusy = false;
        }

        [RelayCommand]
        public async void Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public void CheckUser(DistributionItem user)
        {
            if(user.IsChecked)
            {
                SelectedUserSource.Add(user);
            }
            else
            {
                SelectedUserSource.Remove(user);
            }
        }

        public DistributionViewModel()
        {
            GetAllUsersCommand.Execute(null);
        }
    }
}
