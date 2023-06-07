using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EF.Repositories;
using UI.Helpers;

namespace UI.Views.Components
{

    public partial class ChangeDataModalViewModel : ObservableObject, IQueryAttributable
    {
        private TreeItem treeItem;
        private TreeRepository treeRepository;
        [ObservableProperty] public DateTime? selectedDate = DateTime.Now;

        public ChangeDataModalViewModel()
        {

        }

        [RelayCommand]
        public void Cancel()
        {
            Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public void Accept()
        {
            treeRepository.ChangeDeadline(treeItem.Id, SelectedDate.Value);
            Shell.Current.GoToAsync("..");
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            treeItem = query["TreeItem"] as TreeItem;
            treeRepository = query["TreeRepository"] as TreeRepository;
            SelectedDate = query["SelectedDate"] as DateTime?;

        }
    }
}
