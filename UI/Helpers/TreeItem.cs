using Core.Models;
using System.Collections.ObjectModel;

namespace UI.Helpers
{

    /// <summary>
    /// Модель дерева
    /// </summary>
    public class TreeItem
    {
        public TreeItem()
        {

        }

        public TreeItem(string name)
        {
            Name = name;
        }

        public virtual bool IsExpanded { get; set; } = true;
        public virtual DistributionTreeElement TreeElement { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<TreeItem> Children { get; set; } = new ObservableCollection<TreeItem>();
    }
}
