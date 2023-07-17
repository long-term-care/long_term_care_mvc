using System;

namespace long_term_care.Models
{
    public class ChangeLog
    {
        public int ChangeId { get; set; }
        public string TableName { get; set; }
        public string ActionType { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionBy { get; set; }
        public string Details { get; set; }
    }
}
