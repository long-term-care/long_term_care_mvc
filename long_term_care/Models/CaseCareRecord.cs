using System;
using System.Collections.Generic;

#nullable disable

namespace long_term_care.Models
{
    public partial class CaseCareRecord
    {
        public string CaseQaid { get; set; }
        public string CaseNo { get; set; }
        public string CaseTel { get; set; }
        public string CaseHealth { get; set; }
        public string CaseHome { get; set; }
        public DateTime CaseTime1 { get; set; }
        public string CaseQ1 { get; set; }
        public string CaseQ1Other { get; set; }
        public string CaseQ2 { get; set; }
        public string CaseQ2Other { get; set; }
        public string CaseQ3 { get; set; }
        public string CaseQ3Other { get; set; }
        public string CaseQ4 { get; set; }
        public string CaseQ4Other { get; set; }
        public string MemSid { get; set; }

        public virtual CaseInfor CaseNoNavigation { get; set; }
        public virtual MemberInformation MemS { get; set; }
    }
}
