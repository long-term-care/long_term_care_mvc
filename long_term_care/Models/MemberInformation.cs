using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

#nullable disable

namespace long_term_care.Models
{
    public partial class MemberInformation
    {
        public MemberInformation()
        {
            CarPicks = new HashSet<CarPick>();
            CaseCareRecords = new HashSet<CaseCareRecord>();
            
            CaseTelRecords = new HashSet<CaseTelRecord>();
            LectureTables = new HashSet<LectureTable>();
            MemSigns = new HashSet<MemSign>();
        }

        public string MemSid { get; set; }
        [Display(Name = "單位名稱")]
        public string MemUnitName { get; set; }
        [Display(Name = "單位編號")]
        public string MemUnitNum { get; set; }
        [Display(Name = "姓名")]
        public string MemName { get; set; }
        [Display(Name = "生日")]
        public DateTime MemBd { get; set; }
        [Display(Name = "身分證字號")]
        public string MemUid { get; set; }
        [Display(Name = "生日")]
        public string MemPassword { get; set; }
        [Display(Name = "性別")]
        public string MemGender { get; set; }
        [Display(Name = "電話")]
        public string? MemTphone { get; set; }
        [Display(Name = "手機")]
        public string? MemMphone { get; set; }
        [Display(Name = "通訊處")]
        public string? MemAddress { get; set; }
        [Display(Name = "服務地點")]
        public string? MemSite { get; set; }
        [Display(Name = "專長")]
        public string? MemProf { get; set; }
        [Display(Name = "證照")]
        public string? MemCert { get; set; }
        [Display(Name = "交通工具")]
        public string? MemTrans { get; set; }
        [Display(Name = "經驗")]
        public string? MemExpr { get; set; }
        [Display(Name = "加入動機")]
        public string? MemMovt { get; set; }
        [Display(Name = "提供服務")]
        public string? MemPserv { get; set; }
        [Display(Name = "身分別")]
        public string? MemIdent { get; set; }
        [Display(Name = "服務紀錄")]
        public string? MemSerRec { get; set; }
        [Display(Name = "教育程度")]
        public string? MemEdu { get; set; }
        public string? RoleId { get; set; }
        public string MemIcnum { get; set; }
        public virtual Roleset Role { get; set; }
        public virtual ICollection<CarPick> CarPicks { get; set; }
        public virtual ICollection<CaseCareRecord> CaseCareRecords { get; set; }
        public virtual ICollection<CaseTelRecord> CaseTelRecords { get; set; }
        public virtual ICollection<LectureTable> LectureTables { get; set; }
        public virtual ICollection<MemSign> MemSigns { get; set; }

        
    }
}
