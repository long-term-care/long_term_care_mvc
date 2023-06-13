using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

#nullable disable

namespace long_term_care.Models
{
    public partial class MemSign
    {
        [Display(Name = "表單編號")]
        public string MemSignQaid { get; set; }

        [Display(Name = "志工id")]
        public string MemSid { get; set; }

        [Display(Name = "簽到時間")]
        public DateTime MemTelTime1 { get; set; }

        [Display(Name = "簽退時間")]
        public DateTime MemTelTime2 { get; set; }

        [Display(Name = "工作日誌")]
        public string MemRecord { get; set; }

        public virtual MemberInformation MemS { get; set; }
    }
}
