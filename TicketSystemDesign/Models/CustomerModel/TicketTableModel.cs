using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystemDesign
{
    public class TicketTableModel
    {
        [Required(ErrorMessage = "This column is required")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "This column is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This column is required")]
        public int Severity { get; set; }

        [Required(ErrorMessage = "This column is required")]
        public int Priority { get; set; }

        [Required(ErrorMessage = "This column is required")]
        public int TicketTypeId { get; set; }

        public long TicketId { get; set; }

        public long UserId { get; set; }

        public bool Resolved { get; set; }

        public string TicketType { get; set; }

    }
}
