using UI.Helpers;

namespace UI.Views.Pages.MainForms.Output;

public partial class OutputMail : ContentPage
{
	public OutputMail()
	{
		InitializeComponent();
        BindingContext = ServiceHelper.GetService<OutputMailViewModel>();
        pickerModes.SelectedIndex = 0;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Previewer.FilePath = ((Button)sender).CommandParameter.ToString();
    }

    private void Search_TextChanged(object sender, TextChangedEventArgs e)
    {
        var mode = StringToModesEnum(pickerModes.SelectedItem.ToString());
        if (mode is null) return;
        if (BindingContext is OutputMailViewModel vm)
        {
            vm.SearchModesEnum = mode.Value;
            vm.TextSearchCommand.Execute(((Entry)sender).Text);
        }
    }
    private SearchModesEnum? StringToModesEnum(string str)
    {
        switch (str)
        {
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
    private void dgMails_ItemSelected(object sender, SelectionChangedEventArgs e)
    {

        if (this.BindingContext is OutputMailViewModel vm1)
        {
            Previewer.FilePath = vm1.CurrentFilePath;
        }

        if (e.CurrentSelection.Count == 0) return;

        if (e.CurrentSelection[0] is OMailWrapper currentSelection)
        {
            currentSelection.IsSelected = true;

        }

        if (e.PreviousSelection.Count == 0) return;

        if (e.PreviousSelection[0] is OMailWrapper prevSelection)
        {
            prevSelection.IsSelected = false;
        }
    }
    private void Button_Clicked_1(object sender, EventArgs e)
    {
        if (this.BindingContext is OutputMailViewModel vm)
        {
            vm.OpenMailCommand.Execute(vm.SelectedMail);
        }
        
    }
}