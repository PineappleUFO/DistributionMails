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
}