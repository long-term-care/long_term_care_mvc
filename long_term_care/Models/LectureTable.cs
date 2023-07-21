using System;
using System.Collections.Generic;

#nullable disable

namespace long_term_care.Models
{
    public partial class LectureTable
    {
        public LectureTable()
        {
            CaseActContents = new HashSet<CaseActContent>();
        }
        public string LecId { get; set; }
        public string MemSid { get; set; }
        public string LecTheme { get; set; }
        public string LecClass { get; set; }
        public string LecAim { get; set; }
        public DateTime LecDate { get; set; }
        public string LecLeader { get; set; }
        public string LecPla { get; set; }
        public string LecTool { get; set; }
        public string LecStep { get; set; }

        public virtual MemberInformation MemS { get; set; }

        public virtual ICollection<CaseActContent> CaseActContents { get; set; }
    }
}
