using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace long_term_care.Models
{
    public class Roleset
    {

        public Roleset()
        {
            MemberInformations = new HashSet<MemberInformation>();
        }

        public string Id { get; set; }
        public string RoleName { get; set; }
        public string Permissions { get; set; }

        public virtual ICollection<MemberInformation> MemberInformations { get; set; }

    }
}
