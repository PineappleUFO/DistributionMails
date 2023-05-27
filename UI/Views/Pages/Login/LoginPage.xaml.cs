using CommunityToolkit.Mvvm.Messaging;
using UI.Helpers;
using UI.Messengers;

namespace UI.Views.Pages.Login;

public partial class LoginPage : ContentPage
{
	private readonly IMessenger _messenger;
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = ServiceHelper.GetService<LoginViewModel>();
		_messenger = ServiceHelper.GetService<IMessenger>();
		_messenger.Register<DisplayAlertMessage>(this,OnDisplayAlertMessageRecieved);
	}

	private async void OnDisplayAlertMessageRecieved(object recipient, DisplayAlertMessage message)
	{
		await DisplayAlert(message.Title, message.Message, message.Cancel);
	}
}