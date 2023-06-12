using Core.Models;
using System.Reflection.Metadata.Ecma335;
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
        pickerModes.SelectedIndex = 0;
    }


    //private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
    //{

    //    // Проверяем, прошло ли время, достаточное для считывания двойного нажатия
    //    var currentTime = DateTime.Now;
    //    var elapsedTime = currentTime - _lastTapTime;
    //    _lastTapTime = currentTime;

    //    // Если предыдущий элемент и текущий элемент совпадают, и прошло мало времени,
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
      Previewer.FilePath = ((ImageButton)sender).CommandParameter.ToString();
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

        if (this.BindingContext is InputMailMainViewModel vm1)
        {
                Previewer.FilePath = vm1.CurrentFilePath;
        }

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


    private void Archive_Clicked(object sender, EventArgs e)
    {

    }

    private void DistributionToMe_Clicked(object sender, EventArgs e)
    {

    }

    private void All_Clicked(object sender, EventArgs e)
    {
        if(BindingContext is InputMailMainViewModel vm)
        {
            vm.LoadAllCommand.Execute(null);
        }
    }

    private  void Favorites_Clicked(object sender, EventArgs e)
    {
       
    }

    private async void Menu_Clicked(object sender, EventArgs e)
    {
        if (!brModesAccess.IsVisible)
        {
            brModesAccess.IsVisible = true;
            await brModesAccess.FadeTo(1, 250,Easing.CubicIn);
        }
        else
        {
            await brModesAccess.FadeTo(0, 250, Easing.CubicOut);
            brModesAccess.IsVisible = false;
        }
    }



    private void CV_Modes_Changed(object sender, SelectionChangedEventArgs e)
    {
        if(e.CurrentSelection[0] is MailType type)
        {
            if (BindingContext is InputMailMainViewModel vm)
            {
                vm.LoadMailByTypeCommand.Execute(type);
            }
        }
    }

    private void Search_TextChanged(object sender, TextChangedEventArgs e)
    {
        var mode = StringToModesEnum(pickerModes.SelectedItem.ToString());
        if (mode is null) return;
        if (BindingContext is InputMailMainViewModel vm)
        {
            vm.SearchModesEnum = mode.Value;
            vm.TextSearchCommand.Execute(((Entry)sender).Text);
        }
    }

    private SearchModesEnum? StringToModesEnum(string str)
    {
        switch (str) {
            case "Умный":
                return SearchModesEnum.Smart;
            case "По номеру письма":
                return SearchModesEnum.Number;
            case "По проекту":
                return SearchModesEnum.Project;
            case "По отправителю":
                return SearchModesEnum.Sender;
            case "По теме":
                return SearchModesEnum.Theme;
            case "По дате поступления":
                return SearchModesEnum.Date;
        }
        return null;
    }
}


