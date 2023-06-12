using System;
using System.Collections.Generic;

#nullable disable

namespace long_term_care.Models
{
    public partial class LectureClass
    {
        public string SchWeek { get; set; }
        public string SchA { get; set; }
        public string SchB { get; set; }
        public string SchC { get; set; }
        public string SchD { get; set; }
        public int? Weeknum { get; set; }
    }
}
