using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace long_term_care.ViewModels
{
    public class PhysicalSearchResultViewModel
    {
        [Display(Name = "受訪者 : ")]
        public string CaseName { get; set; }

        [Display(Name = "性別 : ")]
        public string CaseGender { get; set; }

        [Display(Name = "年齡 : ")]
        public string CaseOld { get; set; }

        [Display(Name = "婚姻 : ")]
        public string CaseMari { get; set; }

        [Display(Name = "教育程度 : ")]
        public string CaseEdu { get; set; }

        [Display(Name = "以上居住情形:")]
        public string CaseLive { get; set; }

        [Display(Name = "您多久來關懷站一次: ")]
        public string CaseFre { get; set; }

        [Display(Name = "一、參加C 單位巷弄長照站辦理的活動之後，精神是否較好? ")]
        public string CaseContent1 { get; set; }

        [Display(Name = "二、定期檢測血壓、體溫、體重，是否對您有幫助?")]
        public string CaseContent2 { get; set; }

        [Display(Name = "三、您來C 單位巷弄長照站之後，經常參與的活動有哪些(可複選)?")]
        public string CaseContent3 { get; set; }

        [Display(Name = "四、您來C 單位巷弄長照站之後，是否學到新東西(可複選)?")]
        public string CaseContent4 { get; set; }

        [Display(Name = "五、您參加C 單位巷弄長照站的活動後，是否有多結交一些朋友?")]
        public string CaseContent5 { get; set; }

        [Display(Name = "六、您參加C 單位巷弄長照站的活動後，心情是否改變?")]
        public string CaseContent6 { get; set; }

        [Display(Name = "七、您對C 單位巷弄長照站的「場地規劃與設備提供」情形是否滿意? ")]
        public string CaseContent7 { get; set; }

        [Display(Name = "八、您對C 單位巷弄長照站的「志工的服務態度」情形是否滿意?")]
        public string CaseContent8 { get; set; }

        [Display(Name = "九、您對C 單位巷弄長照站所辦理的「健康促進活動」是否滿意? ")]
        public string CaseContent9 { get; set; }

        [Display(Name = "十、您對C 單位巷弄長照站所辦理的「餐飲服務」情形是否滿意? ")]
        public string CaseContent10 { get; set; }

        [Display(Name = "十一、整體而言，您是否喜歡到C 單位巷弄長照站活動? ")]
        public string CaseContent11 { get; set; }

        [Display(Name = "十二、您覺得這裡所辦理的活動是否適合您，您有什麼建議?")]
        public string CaseContent12 { get; set; }

        [Display(Name = "十三、您參加C 單位巷弄長照站的活動之後，對您生活有什麼影響(改變)?")]
        public string CaseContent13 { get; set; }
        public string CaseQaid { get; set; }


    }
}
