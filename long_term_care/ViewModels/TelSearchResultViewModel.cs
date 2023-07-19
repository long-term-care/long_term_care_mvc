using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace long_term_care.ViewModels
{
    public class TelSearchResultViewModel
    {
        [Display(Name = "出生")]
        public DateTime CaseBd { get; set; }
        [Display(Name = "健保")]
        public string CaseHealth { get; set; }
        [Display(Name = "身分別")]
        public string CaseIdent { get; set; }

        [Display(Name = "常用語言")]
        public string CaseLang { get; set; }
        [Display(Name = "表單編號")]
        public string CaseTelQaid { get; set; }

        [Display(Name = "個案編號")]
        public string CaseNo { get; set; }

        [Display(Name = "案主姓名")]
        public string CaseName { get; set; }

        [Display(Name = "案主性別")]
        public string CaseGender { get; set; }

        [Display(Name = "民國年月")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM}", ApplyFormatInEditMode = true)]
        public DateTime CaseRegTime { get; set; }

        [Display(Name = "故有疾病")]
        public string CaseSick { get; set; }

        [Display(Name = "日期")]
        public int CaseDay { get; set; }

        [Display(Name = "開始時分")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:H:mm}", ApplyFormatInEditMode = true)]
        public DateTime CaseTelTime1 { get; set; }

        [Display(Name = "結束時分")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:H:mm}", ApplyFormatInEditMode = true)]
        public DateTime CaseTelTime2 { get; set; }

        [Display(Name = "接聽情形")]
        public string CaseAns { get; set; }

        [Display(Name = "口頭表達")]
        public string CaseExp { get; set; }

        [Display(Name = "健康情況")]
        public string CaseHea { get; set; }

        [Display(Name = "生活狀況")]
        public string CaseLive { get; set; }

        [Display(Name = "親友互動")]
        public string CaseFam { get; set; }

        [Display(Name = "精神狀況")]
        public string CaseMental { get; set; }

        [Display(Name = "總評")]
        public string CaseCom { get; set; }

        [Display(Name = "服務人員")]
        public string MemSid { get; set; }
    }
}
