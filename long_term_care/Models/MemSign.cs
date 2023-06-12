using System;
using System.Collections.Generic;

#nullable disable

namespace long_term_care.Models
{
    public partial class MemSign
    {
        public string MemSignQaid { get; set; }
        public string MemSid { get; set; }
        public DateTime MemTelTime1 { get; set; }
        public DateTime MemTelTime2 { get; set; }
        public string MemRecord { get; set; }

        public virtual MemberInformation MemS { get; set; }
    }
}
