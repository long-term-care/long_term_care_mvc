using long_term_care.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace long_term_care.ViewModels
{
    public class DailyResultViewModel
    {
        [Display(Name = "姓名")]
        public string CaseName { get; set; }

        [Display(Name = "出生")]
        public string CaseBd { get; set; }
        [Display(Name = "性別")]
        public string CaseGender { get; set; }
        [Display(Name = "身分別")]
        public string CaseIdent { get; set; }

        [Display(Name = "常用語言")]
        public string CaseLang { get; set; }

        [Display(Name = "婚姻狀況")]
        public string CaseMari { get; set; }

        [Display(Name = "家庭狀況")]
        public string CaseFami { get; set; }

        [Display(Name = "現住地址")]
        public string CaseAddr { get; set; }

        [Display(Name = "緊急聯絡人")]
        public string CaseCnta { get; set; }

        [Display(Name = "電話")]
        public string CaseCntTel { get; set; }

        [Display(Name = "關係")]
        public string CaseCntRel { get; set; }

        [Display(Name = "問卷編號")]
        public string CaseContId { get; set; }
        [Display(Name = "個案編號")]

        public string CaseNo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MMM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "日期")]
        public DateTime Casedate { get; set; }
        [Display(Name = "體溫")]

        public string CaseTemp { get; set; }
        [Display(Name = "脈搏")]

        public int CasePluse { get; set; }
        [Display(Name = "血壓")]

        public string CaseBlood { get; set; }
        [Display(Name = "交通接送")]

        public string CasePick { get; set; }
    }
}
