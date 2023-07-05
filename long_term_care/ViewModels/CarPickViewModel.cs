using System;
using System.ComponentModel.DataAnnotations;

namespace long_term_care.ViewModels
{
    public class CarPickViewModel
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

        public string CarId { get; set; }

        [Display(Name = "個案編號")]
        public string CaseNo { get; set; }

        public string MemSid { get; set; }

        
        public DateTime CarSearch { get; set; }

        public string CarType { get; set; }

        public string CarNum { get; set; }
        public string CarAgencyLoc { get; set; }

        

        public DateTime CarMonth { get; set; }
        
        public string CarCaseAdr { get; set; }

        public double CarL { get; set; }
        public double CarKm { get; set; }
        public decimal CarPrice { get; set; }
        public string CaseContId { get; set; }

    }
}
