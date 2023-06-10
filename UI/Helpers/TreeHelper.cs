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
        /// <summary>
        /// генерация ветвей 
        /// </summary>
        public static ObservableCollection<TreeItem> GenerateTreeFromDbData(IEnumerable<TreeItem> elements, int parentId = 0)
        {
            ObservableCollection<TreeItem> treeItems = new ObservableCollection<TreeItem>();

            var filteredElements = elements.Where(e => e.UpId == parentId);

            foreach (var element in filteredElements)
            {
              
                string status= string.Empty;

                if(element.IsResponsible)
                {
                    element.PrefixStatus = "*(Ответственный)";
                    element.PrefixStatusColor = $"#305adc";
                }
                else if(element.IsReplying)
                {
                    element.PrefixStatus = "$(Отвечающий)";
                    element.PrefixStatusColor = $"#ffda84";
                }
                else if (element.IsReplying && element.IsResponsible)
                {
                    element.PrefixStatus = "$(Ответсвенный и Отвечающий)";
                    element.PrefixStatusColor = $"#305adc";
                }

                string RowName = $"{element.User?.Family} {element.User?.Inicials}  (Срок до:{element.Deadline:d};Резолюция:{element.Resolution})";
                element.Id = element.Id;
                element.Name = RowName;
                element.User = element.User;
                element.Children = GenerateTreeFromDbData(elements, element.Id);

                treeItems.Add(element);
            }

            return treeItems;
        }
    }
    }

