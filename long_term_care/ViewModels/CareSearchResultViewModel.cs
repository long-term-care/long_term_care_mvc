using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using long_term_care.Models;

namespace long_term_care.ViewModels
{
    public class CareSearchResultViewModel
    {
        [Display(Name = "姓名")]
        public string CaseName { get; set; }

        [Display(Name = "出生")]
        public string CaseBd { get; set; }
        [Display(Name = "性別")]
        public string CaseGender { get; set; }
        [Display(Name = "身分證統一編號")]
        public string CaseNo { get; set; }//x

        [Display(Name = "電話")]
        public string CasePhn { get; set; }
        [Display(Name = "健保")]
        public string CaseHealth { get; set; }
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

        public string CaseQaid { get; set; }


    }
}
