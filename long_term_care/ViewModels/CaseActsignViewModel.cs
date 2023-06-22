using System;

namespace long_term_care.ViewModels
{
    public class CaseActsignViewModel
    {
        public int type {get;set;} 
        public string ActId { get; set; }
        public DateTime ActDate { get; set; }
        public string ActLec { get; set; }
        public string ActCourse { get; set; }
        public string ActLoc { get; set; }
        public string? CaseNo { get; set; }
        public string? CaseName { get; set; }
        public string ActSer { get; set; }

    }

}
