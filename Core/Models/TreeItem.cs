using System.Collections.ObjectModel;

namespace Core.Models
{
    public class TreeItem : Entity
    {
        public TreeItem()
        {

        }

        public TreeItem(string name)
        {
            Name = name;
        }
        public  int Id { get; set; }
        public  bool IsExpanded { get; set; } = true;
        public  DateTime? Deadline { get; set; }
        public  string Name { get; set; }
        public  User User { get; set; }
        public  Status Status { get; set; }
        /// <summary>
        /// статус *(ответсвенный) или $(отвечающий)
        /// </summary>
        public string PrefixStatus { get; set; }
        public string PrefixStatusColor { get; set; }
        public  IList<TreeItem> Children { get; set; } = new ObservableCollection<TreeItem>();
        public int MailId { get; set; }
        public int UpId { get; set; }
        public bool IsResponsible { get; set; }
        public bool IsReplying { get; set; }
        public string? Resolution { get; set; }
        public string? Log { get; set; }
        public DateTime? DateAdd { get; set; }
    }
}
