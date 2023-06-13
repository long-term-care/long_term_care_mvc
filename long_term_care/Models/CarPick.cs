using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace long_term_care.Models
{
    public partial class CarPick
    {
        public string CarId { get; set; }
        public string MemSid { get; set; }
        public DateTime CarDate { get; set; }
        public string CarType { get; set; }
        public enum CarEnumType
        {
            [Display(Name = "小巴")]
            SmallBus = 1,

            [Display(Name = "客車")]
            Coach = 2,

            [Display(Name = "計程車")]
            Taxi = 3
        }
        public string CarNum { get; set; }
        public DateTime CarMonth { get; set; }

        public string CarCaseAdr { get; set; }
        public double CarL { get; set; }
        public double CarKm { get; set; }
        public decimal CarPrice { get; set; }
        public string CaseContId { get; set; }

        public virtual CaseDailyRegistration CaseCont { get; set; }
        public virtual MemberInformation MemS { get; set; }
    }
}
