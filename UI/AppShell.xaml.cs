using UI.Views.Components;

namespace UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("TestPage", typeof(Test));
             
        }
    }
}