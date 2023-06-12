using System;
using System.Collections.Generic;

#nullable disable

namespace long_term_care.Models
{
    public partial class CaseDailyRegistration
    {
        public CaseDailyRegistration()
        {
            CarPicks = new HashSet<CarPick>();
        }

        public string CaseContId { get; set; }
        public string CaseNo { get; set; }
        public DateTime CaseDailyTime1 { get; set; }
        public DateTime CaseDailyTime2 { get; set; }
        public int CaseTemp { get; set; }
        public int CasePluse { get; set; }
        public string CaseCont { get; set; }
        public string CaseIssue { get; set; }
        public string CaseRem { get; set; }
        public string MemSid { get; set; }
        public string CasePick { get; set; }
        public DateTime CaseRegTime { get; set; }

        public virtual CaseInfor CaseNoNavigation { get; set; }
        public virtual MemberInformation MemS { get; set; }
        public virtual ICollection<CarPick> CarPicks { get; set; }
    }
}
