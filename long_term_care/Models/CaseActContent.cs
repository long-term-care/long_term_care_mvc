namespace long_term_care.Models
{
    public class CaseActContent
    {
        public string ActId { get; set; }
        public string CaseNo { get; set; }
        public string ActSer { get; set; }

        public virtual CaseAct Act { get; set; }
        public virtual CaseInfor CaseNoNavigation { get; set; }
    }
}
