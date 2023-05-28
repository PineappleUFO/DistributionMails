using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class DistributionTreeElement:Entity
    {
        public int MailId { get; set; }
        public User User { get; set; }
        //todo status
        public int UpId { get; set; }
        public DateTime? DeadLine { get; set; }
        public string Resolution { get; set; }
        public bool IsResponsible { get; set; }
        public bool IsReplying { get; set; }
        public DateTime? DateAdd { get; set; }
        public string Log { get; set; }
    }
}
