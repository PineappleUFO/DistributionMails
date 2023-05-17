using UI.Helpers;

namespace UI.Views.Pages.MainForms.Input;

public partial class InputMailMain : ContentPage
{
	public InputMailMain()
	{
		InitializeComponent();
		BindingContext = ServiceHelper.GetService<InputMailMainViewModel>();

	}
	

	private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
	{
		if (this.BindingContext is InputMailMainViewModel vm)
		{
			vm.OpenMailCommandCommand.Execute(vm.SelectedMail);
		}
	}
}