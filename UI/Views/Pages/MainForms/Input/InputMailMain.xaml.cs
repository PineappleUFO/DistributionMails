using Core.Models;
using UI.Helpers;

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


    //private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
    //{

    //    // ѕровер€ем, прошло ли врем€, достаточное дл€ считывани€ двойного нажати€
    //    var currentTime = DateTime.Now;
    //    var elapsedTime = currentTime - _lastTapTime;
    //    _lastTapTime = currentTime;

    //    // ≈сли предыдущий элемент и текущий элемент совпадают, и прошло мало времени,
    //    // считаем это двойным нажатием
    //    if (elapsedTime.TotalMilliseconds < 300)
    //    {
    //        if (this.BindingContext is InputMailMainViewModel vm)
    //        {
    //            vm.OpenMailCommand.Execute(vm.SelectedMail);
    //        }
    //    }

    //    if (this.BindingContext is InputMailMainViewModel vm1)
    //    {
    //        //todo сделать значок загрузки
    //        //todo иногда вылетает если быстро кликать
    //        Previewer.FilePath = vm1.CurrentFilePath;
    //    }

    //}



    private void Button_Clicked(object sender, EventArgs e)
    {

        if (this.BindingContext is InputMailMainViewModel vm1)
        {
            vm1.OpenPreviewFileCommand.Execute(((Button)sender).CommandParameter);
            Previewer.FilePath = vm1.CurrentFilePath;
        }
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {

        if (this.BindingContext is InputMailMainViewModel vm)
        {
            vm.OpenMailCommand.Execute(vm.SelectedMail);
        }
    }

    private void dgMails_ItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count== 0) return;

        if (e.CurrentSelection[0] is MailWrapper currentSelection)
        {
            currentSelection.IsSelected = true;
         
        }

        if (e.PreviousSelection.Count == 0) return;

        if (e.PreviousSelection[0] is MailWrapper prevSelection)
        {
            prevSelection.IsSelected = false;
        }
    }
}