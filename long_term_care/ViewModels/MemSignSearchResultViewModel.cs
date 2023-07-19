using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace long_term_care.ViewModels
{
    public class MemSignSearchResultViewModel
    {
        [Display(Name = "表單編號")]
        public string MemSignQaid { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM}", ApplyFormatInEditMode = true)]
        [Display(Name = "年月")]
        public DateTime MemYM { get; set; }

        public string MemSid { get; set; }
        [Display(Name = "服務員姓名 : ")]
        public string MemName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "月/日")]
        public DateTime MemDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "簽到時間")]
        public DateTime MemTelTime1 { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "簽退時間")]
        public DateTime MemTelTime2 { get; set; }

        [Display(Name = "工作日誌")]
        public string MemRecord { get; set; }
    }
}
