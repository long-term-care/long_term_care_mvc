﻿using System;
using System.Collections.Generic;

#nullable disable

namespace long_term_care.Models
{
    public partial class CaseDailyRegistration
    {
        public string CaseContId { get; set; }
        public string CaseNo { get; set; }

        public DateTime Casedate { get; set; }
        public string CaseTemp { get; set; }
        public string CasePluse { get; set; }
        public string CaseBlood { get; set; }
        public string CasePick { get; set; }
        public string CaseSystolic { get; set; }
        public string CaseDiastolic { get; set; }



        public virtual CaseInfor CaseNoNavigation { get; set; }
       
    }
}
