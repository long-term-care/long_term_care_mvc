using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.ComponentModel.DataAnnotations;

namespace long_term_care.ViewModels
{
    public class MemsignViewModel
    {
        
        public string MemSignQaid { get; set; }

        
        public string MemSid { get; set; }

        
        public DateTime MemTelTime1 { get; set; }

      
        public DateTime MemTelTime2 { get; set; }

        
        public string MemRecord { get; set; }
    }
}
