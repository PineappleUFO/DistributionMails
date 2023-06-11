using CommunityToolkit.Mvvm.ComponentModel;
using Core.Models;

namespace UI.Helpers
{
    /// <summary>
    /// Обертка Mail в ObservableObject и добавления отслеживаемого свойства
    /// </summary>
    public partial class MailWrapper : ObservableObject
    {
        [ObservableProperty] public Mail mail;
        [ObservableProperty] public bool isSelected;
    }

    /// <summary>
    /// Обертка OutgoingMail в ObservableObject и добавления отслеживаемого свойства
    /// </summary>
    public partial class OMailWrapper : ObservableObject
    {
        [ObservableProperty] public OutgoingMail mail;
        [ObservableProperty] public bool isSelected;
    }
}
