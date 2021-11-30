using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TicketSystemDesign.Models
{
    public partial class TicketProp
    {
        public TicketProp()
        {
            TicketTable = new HashSet<TicketTable>();
        }

        public string TicketType { get; set; }
        public int TicketTypeId { get; set; }
        public string RolePermission { get; set; }

        public virtual ICollection<TicketTable> TicketTable { get; set; }
    }
}
