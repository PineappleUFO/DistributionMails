using CommunityToolkit.Mvvm.ComponentModel;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Helpers
{
    /// <summary>
    /// Обертка Mail в ObservableObject и добавления отслеживаемого свойства
    /// </summary>
    public partial class MailWrapper:ObservableObject
    {
        [ObservableProperty] public Mail mail;
        [ObservableProperty] public bool isSelected;
    }
}
