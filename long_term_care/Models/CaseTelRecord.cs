using System;
using System.Collections.Generic;

#nullable disable

namespace long_term_care.Models
{
    public partial class CaseTelRecord
    {
        public string CaseTelQaid { get; set; }
        public string CaseNo { get; set; }
        public DateTime CaseRegTime { get; set; }
        public string CaseSick { get; set; }
        public int CaseDay { get; set; }
        public DateTime CaseTelTime1 { get; set; }
        public DateTime CaseTelTime2 { get; set; }
        public string CaseAns { get; set; }
        public string CaseExp { get; set; }
        public string CaseHea { get; set; }
        public string CaseLive { get; set; }
        public string CaseFam { get; set; }
        public string CaseMental { get; set; }
        public string CaseCom { get; set; }
        public string MemSid { get; set; }

        public virtual CaseInfor CaseNoNavigation { get; set; }
        public virtual MemberInformation MemS { get; set; }
    }
}
