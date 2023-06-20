using System;
using System.ComponentModel.DataAnnotations;

namespace long_term_care.ViewModels
{
    public class CarPickViewModel
    {
        public string CarId { get; set; }
        public string MemSid { get; set; }
        public int CarSearchY { get; set; }
        public int CarSearchM { get; set; }
        public string CarType { get; set; }
        public enum CarEnumType
        {
            [Display(Name = "小巴")]
            小巴 = 1,

            [Display(Name = "客車")]
            客車 = 2,

            [Display(Name = "計程車")]
            計程車 = 3
        }
        public string CarNum { get; set; }
        public DateTime CarMonth { get; set; }

        public string CarCaseAdr { get; set; }
        public double CarL { get; set; }
        public double CarKm { get; set; }
        public decimal CarPrice { get; set; }
        public string CaseContId { get; set; }

    }
}
