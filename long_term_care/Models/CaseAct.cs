﻿using System;
using System.Collections.Generic;

#nullable disable

namespace long_term_care.Models
{
    public partial class CaseAct
    {
     
        public string ActId { get; set; }
        public DateTime ActDate { get; set; }
        public string ActLec { get; set; }
        public string ActCourse { get; set; }
        public string ActLoc { get; set; }
       

    }
}
