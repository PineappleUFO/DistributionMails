using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Helpers;

namespace UI.Views.Pages.Message;

public partial class MessageView : ContentPage
{
    public MessageView()
    {
        InitializeComponent();
        BindingContext = ServiceHelper.GetService<MessageViewModel>();
    }

    private void MenuFlyoutItem_Clicked(object sender, EventArgs e)
    {
       if(((sender as MenuFlyoutItem).Parent as MenuFlyout).BindingContext is TreeItem treeItem )
        {
            if(BindingContext is MessageViewModel viewModel)
            {
                viewModel.AddFirstLevelCommand.Execute(treeItem);
            }
        }
    }
}