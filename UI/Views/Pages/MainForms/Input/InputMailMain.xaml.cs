using Core.Models;
using EF.Repositories;
using UI.Helpers;
using static iTextSharp.text.pdf.AcroFields;

namespace UI.Views.Pages.MainForms.Input;

public partial class InputMailMain : ContentPage
{
    private DateTime _lastTapTime;

    public User CurrentUser { get; set; }
    public InputMailMain()
	{
		InitializeComponent();
		BindingContext = ServiceHelper.GetService<InputMailMainViewModel>();

	}
	

	private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
	{
      
        // ���������, ������ �� �����, ����������� ��� ���������� �������� �������
        var currentTime = DateTime.Now;
        var elapsedTime = currentTime - _lastTapTime;
        _lastTapTime = currentTime;

        // ���� ���������� ������� � ������� ������� ���������, � ������ ���� �������,
        // ������� ��� ������� ��������
        if (elapsedTime.TotalMilliseconds < 300 )
        {
            if (this.BindingContext is InputMailMainViewModel vm)
            {
                vm.OpenMailCommand.Execute(vm.SelectedMail);
            }
        }

        if (this.BindingContext is InputMailMainViewModel vm1)
        {
            //todo ������� ������ ��������
            //todo ������ �������� ���� ������ �������
            Previewer.FilePath = vm1.CurrentFilePath;
        }


      
       
    }

 

    private void Button_Clicked(object sender, EventArgs e)
    {

        if (this.BindingContext is InputMailMainViewModel vm1)
        {
            vm1.OpenPreviewFileCommand.Execute(((Button)sender).CommandParameter);
            Previewer.FilePath = vm1.CurrentFilePath;
        }
    }
}