using UI.Helpers;

namespace UI.Views.Components;

public partial class ChangeDateModal : ContentPage
{


    public ChangeDateModal()
    {
        InitializeComponent();
        BindingContext = ServiceHelper.GetService<ChangeDataModalViewModel>();
    }



}