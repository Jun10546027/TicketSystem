using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TicketSystemDesign.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            UserInfo = new HashSet<UserInfo>();
        }

        public string Role { get; set; }
        public byte Status { get; set; }

        public virtual ICollection<UserInfo> UserInfo { get; set; }
    }
}
