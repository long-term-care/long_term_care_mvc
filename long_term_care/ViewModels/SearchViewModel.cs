using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using long_term_care.Models;



namespace long_term_care.ViewModels
{
    public class SearchViewModel
    {
        public string CaseNo { get; set; }
        public string CaseName { get; set; }
        public string CaseIDCard { get; set; }
    }

}
