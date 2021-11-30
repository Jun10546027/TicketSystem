using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystemDesign
{
    public class TicketInfoModel
    {
        public string Summary { get; set; }

        public string Description { get; set; }

        public int Severity { get; set; }

        public int Priority { get; set; }

        public int TicketTypeId { get; set; }

        public string TicketType { get; set; }

        public int TicketId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public bool Resolved { get; set; }

        public DateTime DateTime { get; set; }

    }
}
