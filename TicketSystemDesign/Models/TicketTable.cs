using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TicketSystemDesign.Models
{
    public partial class TicketTable
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public int Severity { get; set; }
        public int Priority { get; set; }
        public int TicketTypeId { get; set; }
        public long UserId { get; set; }
        public bool? Resolved { get; set; }
        public long TicketId { get; set; }
        public DateTime? DateTime { get; set; }

        public virtual TicketProp TicketType { get; set; }
        public virtual UserInfo User { get; set; }
    }
}
