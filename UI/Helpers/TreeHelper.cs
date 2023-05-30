using Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Helpers
{
    public static class TreeHelper
    {
        public static ObservableCollection<TreeItem> GenerateTreeFromDbData(IEnumerable<DistributionTreeElement> elements, int parentId = 0)
        {
            ObservableCollection<TreeItem> treeItems = new ObservableCollection<TreeItem>();

            var filteredElements = elements.Where(e => e.UpId == parentId);

            foreach (var element in filteredElements)
            {
                TreeItem treeItem = new TreeItem();
                
                string RowName = $"{element.User?.Family} {element.User?.Inicials} (Срок до:{element.DeadLine:d};Резолюция:{element.Resolution})";
                treeItem.TreeElement = element;
                treeItem.Name = RowName;
                treeItem.User = element.User;
                treeItem.Children = GenerateTreeFromDbData(elements, element.Id);

                treeItems.Add(treeItem);
            }

            return treeItems;
        }
    }
    }

