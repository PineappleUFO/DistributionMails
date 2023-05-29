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
    /// Обертка User в ObservableObject и добавления отслеживаемого свойства
    /// </summary>
    public partial class DistributionItem:ObservableObject
    {
        [ObservableProperty] public User user;
        [ObservableProperty] public DateTime deadline;
        [ObservableProperty] public string resolution;
        [ObservableProperty] public bool isResponsible;
        [ObservableProperty] public bool isReplying;
        [ObservableProperty] public bool isSelected;
        [ObservableProperty] public bool isChecked = false;
    }
}
