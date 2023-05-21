using UI.Helpers;
using static iTextSharp.text.pdf.AcroFields;

namespace UI.Views.Pages.MainForms.Input;

public partial class InputMailMain : ContentPage
{
    private DateTime _lastTapTime;

    public InputMailMain()
	{
		InitializeComponent();
		BindingContext = ServiceHelper.GetService<InputMailMainViewModel>();
		Previewer.FilePath = @"C:\Games\Zakharov_Z8431.pdf";
	}
	

	private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
	{
        // ѕровер€ем, прошло ли врем€, достаточное дл€ считывани€ двойного нажати€
        var currentTime = DateTime.Now;
        var elapsedTime = currentTime - _lastTapTime;
        _lastTapTime = currentTime;

        // ≈сли предыдущий элемент и текущий элемент совпадают, и прошло мало времени,
        // считаем это двойным нажатием
        if (elapsedTime.TotalMilliseconds < 300 )
        {
            if (this.BindingContext is InputMailMainViewModel vm)
            {
                vm.OpenMailCommandCommand.Execute(vm.SelectedMail);
            }
        }

    }
	
	
}