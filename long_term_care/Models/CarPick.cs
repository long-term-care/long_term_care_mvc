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
        public DateTime CarSearch { get; set; }
        public string CarType { get; set; }
        public string CaseNo { get; set; }

        public string CarNum { get; set; }
        [Required(ErrorMessage = "Please enter a valid date.")]
        [DataType(DataType.Date)]
        public DateTime CarMonth { get; set; }


        public string CarCaseAdr { get; set; }
        
        public string CarAgencyLoc { get; set; }

        public double CarL { get; set; }
        public double CarKm { get; set; }
        public decimal CarPrice { get; set; }
        public bool IsMonthEqualToSearch()
        {
            string searchYearMonth = CarSearch.ToString("yyyy/MM");
            string monthYear = CarMonth.ToString("yyyy/MM");
            return searchYearMonth == monthYear;
        }

        public virtual MemberInformation MemS { get; set; }
    }
}
