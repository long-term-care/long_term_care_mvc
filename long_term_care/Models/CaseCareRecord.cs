using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

#nullable disable

namespace long_term_care.Models
{
    public partial class CaseCareRecord
    {
        [Display(Name = "表單編號")]
        public string CaseQaid { get; set; }

        [Display(Name = "身分證統一編號")]
        public string CaseNo { get; set; }

        [Display(Name = "電話")]
        public string CaseTel { get; set; }

        [Display(Name = "健保")]
        public string CaseHealth { get; set; }

        [Display(Name = "住宅情形")]
        public string CaseHome { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MMM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "日期")]
        public DateTime CaseTime1 { get; set; }
        [Display(Name = "居家環境")]
        public string CaseQ1 { get; set; }
        [Display(Name = "居家環境(其他)")]
        public string CaseQ1Other { get; set; }
        [Display(Name = "健康狀況")]
        public string CaseQ2 { get; set; }
        [Display(Name = "健康狀況(其他)")]
        public string CaseQ2Other { get; set; }
        [Display(Name = "就醫情形")]
        public string CaseQ3 { get; set; }
        [Display(Name = "就醫情形(其他)")]
        public string CaseQ3Other { get; set; }
        [Display(Name = "提供服務")]
        public string CaseQ4 { get; set; }
        [Display(Name = "提供服務(其他)")]
        public string CaseQ4Other { get; set; }
        [Display(Name = "訪視者")]
        public string MemSid { get; set; }

        [Display(Name = "個案編號")]
        public virtual CaseInfor CaseNoNavigation { get; set; }
        [Display(Name = "志工編號")]
        public virtual MemberInformation MemS { get; set; }

        
    }
}
