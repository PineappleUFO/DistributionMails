using UI.Helpers;

namespace UI.Views.Pages.Distribution;

public partial class DistributionPage : ContentPage
{
	public DistributionPage()
	{
		InitializeComponent();
		BindingContext = BindingContext = ServiceHelper.GetService<DistributionViewModel>();
    }
}