

using UI.Views.Components;
using UI.Views.Pages.Distribution;
using UI.Views.Pages.MainForms.Input;
using UI.Views.Pages.Message;

namespace UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

             Routing.RegisterRoute(nameof(MessageView),typeof(MessageView));
            Routing.RegisterRoute(nameof(DistributionPage), typeof(DistributionPage));
            Routing.RegisterRoute(nameof(InputMailMain), typeof(InputMailMain));
            Routing.RegisterRoute(nameof(ChangeDateModal), typeof(ChangeDateModal));
        

        }
    }
}