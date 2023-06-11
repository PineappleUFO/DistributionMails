using Core.Models;
using UI.Extenstions;
using UI.Helpers;

namespace UI.Views.Pages.Distribution;

public partial class DistributionPage : ContentPage
{
	public DistributionPage()
	{
		InitializeComponent();
		BindingContext = ServiceHelper.GetService<DistributionViewModel>();
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if(BindingContext is DistributionViewModel distributionViewModel)
        {
            distributionViewModel.CheckUser(dgUsers.SelectedItem as DistributionItem);
        }
    }

    private void dgUsers_ItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;

        if (e.CurrentSelection[0] is DistributionItem currentSelection)
        {
            currentSelection.IsSelected = true;

        }

        if (e.PreviousSelection.Count == 0) return;

        if (e.PreviousSelection[0] is DistributionItem prevSelection)
        {
            if(!prevSelection.IsChecked)
            {
                prevSelection.IsSelected = false;
            }

        }



    }
}