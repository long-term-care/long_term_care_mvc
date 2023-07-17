using System;
using System.ComponentModel.DataAnnotations;

namespace long_term_care.ViewModels
{
    public class MainViewModel
    {
        public int type { get; set; }
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
        public string CaseHouse { get; set; }
        public string CaseIdent { get; set; }
        public string CaseFund { get; set; }
        public string CaseHealth { get; set; }
        public string CaseActv { get; set; }
        public string CaseFactly { get; set; }
        public string CaseMari { get; set; }
        public string CaseCnta { get; set; }
        public string CaseCntTel { get; set; }
        public string CaseCntRel { get; set; }
        public string CaseCntAdd { get; set; }
        public string CaseFami { get; set; }
        public string CaseQues { get; set; }
        public string CaseDesc { get; set; }
        public string CaseRegName { get; set; }
        public string CaseRegTime { get; set; }
        [RegularExpression(@"^[A-Z]{2}\d{10}$", ErrorMessage = "健保卡IC码格式不正确")]
        public string CaseIcnum { get; set; }
        //-----------------------------------------------------------------------------------

        public string MemSid { get; set; }
        public string MemUnitName { get; set; }
        public string MemUnitNum { get; set; }
        public string MemName { get; set; }
        public DateTime MemBd { get; set; }
        public string MemUid { get; set; }
        public string MemPassword { get; set; }
        public string MemGender { get; set; }
        public string MemTphone { get; set; }
        public string MemMphone { get; set; }
        public string MemAddress { get; set; }
        public string MemSite { get; set; }
        public string MemProf { get; set; }
        public string MemCert { get; set; }
        public string MemTrans { get; set; }
        public string MemExpr { get; set; }
        public string MemMovt { get; set; }
        public string MemPserv { get; set; }
        public string MemIdent { get; set; }
        public string MemSerRec { get; set; }
        public string MemEdu { get; set; }

        public string MemIcnum { get; set; }
    }
}
