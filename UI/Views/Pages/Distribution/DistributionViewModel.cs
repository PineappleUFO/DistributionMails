using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using EF.Repositories;
using PostgresRepository.Interfaces;
using System.Collections.ObjectModel;
using UI.Helpers;
using UI.Views.Components;

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
        //общая резолюция
        [ObservableProperty] public string allResolution;
        //общий срок
        [ObservableProperty] public DateTime? allDeadline;
        //Выбран ли режим с общей резолюцией и сроком
        [ObservableProperty] public bool isGeneral;
        //число писем в работе у выбранного пользователя
        [ObservableProperty] public int countMailInWork;
        [ObservableProperty] public string selectedResolution;
        [ObservableProperty] public List<string> allResolutionList;
        [ObservableProperty] public bool isFastResolutionVisible = false;

        partial void OnSelectedUserChanged(DistributionItem value)
        {
            if (value == null) return;
            CountMailInWork = mRep.GetMailsInWork(value.User.Id);

        }
        partial void OnAllResolutionChanged(string value)
        {
            if (IsGeneral)
            {
                foreach (var item in SelectedUserSource)
                {
                    if (item != null)
                    {
                        item.Resolution = value;
                    }
                }
            }
            else
            {
                AllResolution = string.Empty;
            }

        }
        partial void OnAllDeadlineChanged(DateTime? value)
        {
            if (IsGeneral)
            {
                foreach (var item in SelectedUserSource)
                {
                    if (item != null)
                    {
                        item.Deadline = value.Value;
                    }
                }
            }
            else
            {
                AllDeadline = null;
            }
        }

        IUserRepository uRep = new UserRepository(TestHelper.GetConnectionSingltone());
        IMailRepository mRep = new MailRepository(TestHelper.GetConnectionSingltone());

        [RelayCommand]
        public async void SetResolution()
        {
           if(IsGeneral)
            {
                AllResolution = SelectedResolution;
            }
            IsFastResolutionVisible = false;
        }

        [RelayCommand]
        public async void GetAllUsers()
        {

            UserSource.Clear();
            IsBusy = true;
            await Task.Delay(200);
            var users = await uRep.GetAllUsers();
            foreach (var user in users.Where(user => user != null))
            {
                UserSource.Add(new DistributionItem() { User = user });
            }
            IsBusy = false;
        }
        TreeRepository treeRep = new TreeRepository(TestHelper.GetConnectionSingltone());

        [RelayCommand]
        public async void GetUsersFromCounter()
        {
            UserSource.Clear();
            IsBusy = true;
            await Task.Delay(200);
            var users = await uRep.GetUserByCount(currentUser.Id);
            foreach (var user in users.Where(user => user != null))
            {
                UserSource.Add(new DistributionItem() { User = user });
            }
            IsBusy = false;
        }

        [RelayCommand]
        public async void GetUsersFromDep()
        {
            UserSource.Clear();
            IsBusy = true;
            await Task.Delay(200);
            var users = await uRep.GetUserFromDep(currentUser.Department.Id);
            foreach (var user in users.Where(user => user != null))
            {
                UserSource.Add(new DistributionItem() { User = user });
            }
            IsBusy = false;
        }
        [RelayCommand]
        public async void GetUsersFromReplacement()
        {
            UserSource.Clear();
            IsBusy = true;
            await Task.Delay(200);
            var users = await uRep.GetUsersFromReplacement(currentUser.Id);
            foreach (var user in users.Where(user => user != null))
            {
                UserSource.Add(new DistributionItem() { User = user });
            }
            IsBusy = false;
        }

        [RelayCommand]
        public async void Save()
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
                //добавляем в счетчик распределения
                treeRep.UpdateCounterDistibution(currentUser.Id, item.User.Id);
            }



            await Shell.Current.GoToAsync("..");

        }

      

        [RelayCommand]
        public async void Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async void ShowFastResolutionList()
        {
            IsFastResolutionVisible = true;
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

        public DistributionViewModel()
        {
            Init();
        }
        private async void Init()
        {
            AllResolutionList =await mRep.GetFastResolution();
        }
        /// <summary>
        /// Установка быстрой резолюции в режиме "общий срок и резолюция"
        /// </summary>
        /// <param name="resolution"></param>
        private void setAllFastResolution(string resolution)
        {
            AllResolution = resolution;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("SelectedMail"))
                selectedMail = query["SelectedMail"] as Mail;

            if (query.ContainsKey("SelectedUserFrom"))
                selectedUserFrom = query["SelectedUserFrom"] as User;

            if (query.ContainsKey("CurrentUser"))
                currentUser = query["CurrentUser"] as User;

            if (query.ContainsKey("SelectedTreeItem"))
                selectedTreeItem = query["SelectedTreeItem"] as TreeItem;

              

            GetUsersFromCounterCommand.Execute(null);
        }


    }
}
