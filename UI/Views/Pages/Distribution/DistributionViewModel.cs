using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Views.Pages.MainForms.Input;

namespace UI.Views.Pages.Distribution
{
    [QueryProperty("SelectedMail", "SelectedMail")]
    public partial class DistributionViewModel : ObservableObject
    {
        [ObservableProperty]
        MailModel selectedMail;
    }
}
