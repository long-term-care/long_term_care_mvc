namespace long_term_care.Models
{
    public class CaseActContent
    {
        public string LecId { get; set; }
        public string CaseNo { get; set; }
        public string ActSer { get; set; }

        public virtual LectureTable Table { get; set; }
        public virtual CaseInfor CaseNoNavigation { get; set; }
    }
}
