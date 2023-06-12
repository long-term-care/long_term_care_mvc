using System;
using System.Collections.Generic;

#nullable disable

namespace long_term_care.Models
{
    public partial class CasePhysicalMental
    {
        public string CaseQaid { get; set; }
        public string CaseNo { get; set; }
        public string CaseLive { get; set; }
        public string CaseFre { get; set; }
        public string CaseContent1 { get; set; }
        public string CaseContent2 { get; set; }
        public string CaseContent3 { get; set; }
        public string CaseContent4 { get; set; }
        public string CaseContent5 { get; set; }
        public string CaseContent6 { get; set; }
        public string CaseContent7 { get; set; }
        public string CaseContent8 { get; set; }
        public string CaseContent9 { get; set; }
        public string CaseContent10 { get; set; }
        public string CaseContent11 { get; set; }
        public string CaseContent12 { get; set; }
        public string CaseContent13 { get; set; }

        public virtual CaseInfor CaseNoNavigation { get; set; }
    }
}
