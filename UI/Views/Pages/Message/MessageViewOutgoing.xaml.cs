using UI.Helpers;

namespace UI.Views.Pages.Message;

public partial class MessageViewOutgoing : ContentPage
{
	public MessageViewOutgoing()
	{
		InitializeComponent();
        BindingContext = ServiceHelper.GetService<MessageViewOutgoingViewModel>();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
            Previewer.FilePath = ((Button)sender).CommandParameter.ToString();
    }


}