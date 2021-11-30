using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TicketSystemDesign.Models
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            TicketTable = new HashSet<TicketTable>();
        }

        public long UserId { get; set; }
        public string UserName { get; set; }
        public byte Status { get; set; }
        public string Pwd { get; set; }
        public string Salt { get; set; }

        public virtual UserRole StatusNavigation { get; set; }
        public virtual ICollection<TicketTable> TicketTable { get; set; }
    }
}
