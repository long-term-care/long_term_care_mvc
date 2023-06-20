using long_term_care.Models;
using System.ComponentModel.DataAnnotations;
namespace long_term_care.ViewModels
{
    public class NeedSearchResultViewModal
    {
        [Display(Name = "問卷編號 : ")]
        public string CaseNeedId { get; set; }
        [Display(Name = "個案編號 : ")]
        public string CaseNo { get; set; }
        [Display(Name = "識字 : ")]
        public string CaseRead { get; set; }
        [Display(Name = "居住情形 : ")]
        public string CaseFami { get; set; }

        [Display(Name = "意識清楚 : ")]
        public string CaseCons { get; set; }
        [Display(Name = "說話清楚 : ")]
        public string CaseSpeak { get; set; }
        [Display(Name = "協助行動 : ")]
        public string CaseAct { get; set; }
        [Display(Name = "服藥情形 : ")]
        public string CaseMed { get; set; }
        [Display(Name = "視力 : ")]
        public string CaseSee { get; set; }
        [Display(Name = "聽力 : ")]
        public string CaseHear { get; set; }
        [Display(Name = "進食 : ")]
        public string CaseEat { get; set; }
        [Display(Name = "自我照顧 : ")]
        public string CaseCare { get; set; }
        [Display(Name = "一、如果○○C單位巷弄長照站，您贊成嗎？ ")]
        public string CaseView1 { get; set; }
        [Display(Name = "二、是否喜歡與其他老人一起聊天、活動？ ")]
        public string CaseView2 { get; set; }
        [Display(Name = "三、每天在家中生活情形？ ")]
        public string CaseView3 { get; set; }
        [Display(Name = "四、在家裡如何吃中餐？ ")]

        public string CaseView4 { get; set; }
        [Display(Name = "五、您可以自行前往活動地點嗎？ ")]
        public string CaseView5 { get; set; }
        [Display(Name = "六、您喜歡的活動 : ")]
        public string CaseView6 { get; set; }
        [Display(Name = "七、您個人會的傳統技藝有:  ")]
        public string CaseView7 { get; set; }
        [Display(Name = "八、如果需繳交活動費、伙食費（每週2個上午含點心或午餐），您認為每個月多少錢可接受? ")]
        public string CaseView8 { get; set; }
        [Display(Name = "九、您本人會來參加C單位巷弄長照站的活動嗎？ ")]
        public string CaseView9 { get; set; }

        public virtual CaseInfor CaseNoNavigation { get; set; }
    }
}
