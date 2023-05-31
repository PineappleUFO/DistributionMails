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

    private void ChangeMyDistribution_Clicked(object sender, EventArgs e)
    {
        if (((sender as MenuFlyoutItem).Parent as MenuFlyout).BindingContext is TreeItem treeItem)
        {
            if (BindingContext is MessageViewModel viewModel)
            {
                viewModel.ChangeMyDistributionCommand.Execute(treeItem);
            }
        }
    }

    private void MyAccept_Clicked(object sender, EventArgs e)
    {
        if (((sender as MenuFlyoutItem).Parent as MenuFlyout).BindingContext is TreeItem treeItem)
        {
            if (BindingContext is MessageViewModel viewModel)
            {
                viewModel.MyAcceptCommand.Execute(treeItem);
            }
        }
        
    }

    private void MyCompleted_Clicked(object sender, EventArgs e)
    {
        if (((sender as MenuFlyoutItem).Parent as MenuFlyout).BindingContext is TreeItem treeItem)
        {
            if (BindingContext is MessageViewModel viewModel)
            {
                viewModel.MyCompletedCommand.Execute(treeItem);
            }
        }
    }

    private void GetResponsible_Clicked(object sender, EventArgs e)
    {
        if (((sender as MenuFlyoutItem).Parent as MenuFlyout).BindingContext is TreeItem treeItem)
        {
            if (BindingContext is MessageViewModel viewModel)
            {
                viewModel.GetResponsibleCommand.Execute(treeItem);
            }
        }
    }

    private void GetReplying_Clicked(object sender, EventArgs e)
    {
        if (((sender as MenuFlyoutItem).Parent as MenuFlyout).BindingContext is TreeItem treeItem)
        {
            if (BindingContext is MessageViewModel viewModel)
            {
                viewModel.GetReplyingCommand.Execute(treeItem);
            }
        }
    }

    private void ChangeDeadline_Clicked(object sender, EventArgs e)
    {
        if (((sender as MenuFlyoutItem).Parent as MenuFlyout).BindingContext is TreeItem treeItem)
        {
            if (BindingContext is MessageViewModel viewModel)
            {
                viewModel.ChangeDeadlineCommand.Execute(treeItem);
            }
        }
    }

    private void Remove_Clicked(object sender, EventArgs e)
    {
        if (((sender as MenuFlyoutItem).Parent as MenuFlyout).BindingContext is TreeItem treeItem)
        {
            if (BindingContext is MessageViewModel viewModel)
            {
                viewModel.RemoveCommand.Execute(treeItem);
            }
        }
    }
}