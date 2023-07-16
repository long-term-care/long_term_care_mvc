using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace long_term_care.Models
{
    public partial class CaseInfor
    {
        public CaseInfor()
        {
            
            CaseActContents = new HashSet<CaseActContent>();
            CaseCareRecords = new HashSet<CaseCareRecord>();
            CaseDailyRegistrations = new HashSet<CaseDailyRegistration>();
            CaseNeeds = new HashSet<CaseNeed>();
            CasePhysicalMentals = new HashSet<CasePhysicalMental>();
            CaseTelRecords = new HashSet<CaseTelRecord>();
        }
       

        public string CaseNo { get; set; }
        public string CaseUnitName { get; set; }
        public string CaseUnitNum { get; set; }
        public string CaseName { get; set; }
        [RegularExpression(@"^[A-Z]\d{9}$", ErrorMessage = "身份证号码格式不正确")]
        public string CaseIdcard { get; set; }
       
        public string CaseGender { get; set; }
        public string CaseRelig { get; set; }
        [RegularExpression(@"\d{4}-\d{2}-\d{2}", ErrorMessage = "日期格式必须为yyyy-mm-dd")]
        public DateTime CaseBd { get; set; }
        public string CaseLang { get; set; }
        public string CaseSource { get; set; }
        public string CaseWork { get; set; }
        public string CaseProf { get; set; }
        public string CaseEdu { get; set; }
        public string CaseAddr { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string CaseHouse { get; set; }
        public string CaseIdent { get; set; }
        public string CaseFund { get; set; }
        public string CaseHealth { get; set; }
        public string CaseActv { get; set; }
        public string CaseFactly { get; set; }
        public string CaseMari { get; set; }
        public string CaseCnta { get; set; }
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "手机号码格式不正确")]
        public string CaseCntTel { get; set; }
        public string CaseCntRel { get; set; }
        public string CaseCntAdd { get; set; }
        public string CaseFami { get; set; }
        public string CaseQues { get; set; }
        public string CaseDesc { get; set; }
        public string CaseRegName { get; set; }
        public string CaseRegTime { get; set; }

        public string CaseIcnum { get; set; }


        public virtual ICollection<CaseActContent> CaseActContents { get; set; }
        public virtual ICollection<CaseCareRecord> CaseCareRecords { get; set; }
        public virtual ICollection<CaseDailyRegistration> CaseDailyRegistrations { get; set; }
        public virtual ICollection<CaseNeed> CaseNeeds { get; set; }
        public virtual ICollection<CasePhysicalMental> CasePhysicalMentals { get; set; }
        public virtual ICollection<CaseTelRecord> CaseTelRecords { get; set; }
      
    }
}
