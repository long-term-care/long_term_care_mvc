using DocumentFormat.OpenXml.Office2010.ExcelAc;
using long_term_care.Models;
using System.Collections.Generic;

namespace long_term_care.ViewModels
{
    public class RolesetViewModel
    {
        public List<MemberInformation> Members { get; set; }

        public string RoleId { get; set; }

    }
}
