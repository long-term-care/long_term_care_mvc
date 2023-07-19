using System;
using System.Collections.Generic;

#nullable disable

namespace long_term_care.Models
{
    public partial class CaseNeed
    {
        public string CaseNeedId { get; set; }
        public string CaseNo { get; set; }
        public string CaseRead { get; set; }
        public string CaseFami { get; set; }
        public string CaseCons { get; set; }
        public string CaseSpeak { get; set; }
        public string CaseAct { get; set; }
        public string CaseMed { get; set; }
        public string CaseSee { get; set; }
        public string CaseHear { get; set; }
        public string CaseEat { get; set; }
        public string CaseCare { get; set; }
        public string CaseView1 { get; set; }
        public string CaseView2 { get; set; }
        public string CaseView3 { get; set; }
        public string CaseView4 { get; set; }
        public string CaseView5 { get; set; }
        public string CaseView6 { get; set; }
        public string CaseView7 { get; set; }
        public string CaseView8 { get; set; }
        public string CaseView9 { get; set; }

        public virtual CaseInfor CaseNoNavigation { get; set; }
    }
}
