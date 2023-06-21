using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace long_term_care.Models
{
    public partial class longtermcareContext : DbContext
    {
        public longtermcareContext()
        {
        }

        public longtermcareContext(DbContextOptions<longtermcareContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CarPick> CarPicks { get; set; }
        public virtual DbSet<CaseAct> CaseActs { get; set; }
        public virtual DbSet<CaseActContent> CaseActContents { get; set; }
        public virtual DbSet<CaseCareRecord> CaseCareRecords { get; set; }
        public virtual DbSet<CaseDailyRegistration> CaseDailyRegistrations { get; set; }
        public virtual DbSet<CaseInfor> CaseInfors { get; set; }
        public virtual DbSet<CaseNeed> CaseNeeds { get; set; }
        public virtual DbSet<CasePhysicalMental> CasePhysicalMentals { get; set; }
        public virtual DbSet<CaseTelRecord> CaseTelRecords { get; set; }
        public virtual DbSet<LectureClass> LectureClasses { get; set; }
        public virtual DbSet<LectureTable> LectureTables { get; set; }
        public virtual DbSet<MemSign> MemSigns { get; set; }
        public virtual DbSet<MemberInformation> MemberInformations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Database=long-term-care;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CarPick>(entity =>
            {
                entity.HasKey(e => e.CarId)
                    .HasName("PK__car_pick__523653D98367FCC5");

                entity.ToTable("car_pick");

                entity.Property(e => e.CarId)
                    .HasMaxLength(30)
                    .HasColumnName("Car_ID");

                entity.Property(e => e.CaseNo)
                    .HasMaxLength(8)
                    .HasColumnName("Case_No");

                entity.Property(e => e.CarCaseAdr)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Car_CaseAdr");  
                
                entity.Property(e => e.CarAgencyLoc)
                    .HasMaxLength(100)
                    .HasColumnName("Car_AgencyLoc");


                entity.Property(e => e.CarSearch)
                  .HasColumnType("datetime")
                  .HasColumnName("Car_Search");

                entity.Property(e => e.CarKm).HasColumnName("Car_Km");

                entity.Property(e => e.CarL).HasColumnName("Car_L");

                entity.Property(e => e.CarMonth)
                    .HasColumnType("date")
                    .HasColumnName("Car_Month");

                entity.Property(e => e.CarNum)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Car_Num");

                entity.Property(e => e.CarPrice)
                    .HasColumnType("money")
                    .HasColumnName("Car_Price");

                entity.Property(e => e.CarType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Car_Type");

               

                entity.Property(e => e.MemSid)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Mem_SID");

             

                entity.HasOne(d => d.MemS)
                    .WithMany(p => p.CarPicks)
                    .HasForeignKey(d => d.MemSid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__car_pick__Mem_SI__44FF419A");
            });

            modelBuilder.Entity<CaseAct>(entity =>
            {
                entity.HasKey(e => e.ActId)
                    .HasName("PK__case_act__6564ADDDE6E88FEE");

                entity.ToTable("case_act");

                entity.Property(e => e.ActId)
                    .HasMaxLength(8)
                    .HasColumnName("Act_ID");

                entity.Property(e => e.ActCourse)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("Act_Course");

                entity.Property(e => e.ActDate)
                    .HasColumnType("date")
                    .HasColumnName("Act_Date");

                entity.Property(e => e.ActLec)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Act_Lec");

                entity.Property(e => e.ActLoc)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("Act_Loc");
            });

            modelBuilder.Entity<CaseActContent>(entity =>
            {
                entity.HasKey(e => new { e.ActId, e.CaseNo })
                    .HasName("PK__Case_Act__5862BDFE9B8D3FC1");

                entity.ToTable("Case_Act_Content");

                entity.Property(e => e.ActId)
                    .HasMaxLength(8)
                    .HasColumnName("Act_ID");

                entity.Property(e => e.CaseNo)
                    .HasMaxLength(8)
                    .HasColumnName("Case_No");

                entity.Property(e => e.ActSer)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Act_Ser");

                entity.HasOne(d => d.Act)
                    .WithMany(p => p.CaseActContents)
                    .HasForeignKey(d => d.ActId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Case_Act___Act_I__498EEC8D");

                entity.HasOne(d => d.CaseNoNavigation)
                    .WithMany(p => p.CaseActContents)
                    .HasForeignKey(d => d.CaseNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Case_Act___Case___4A8310C6");
            });

            modelBuilder.Entity<CaseCareRecord>(entity =>
            {
                entity.HasKey(e => e.CaseQaid)
                    .HasName("PK__Case_Car__21B54A279265BE84");

                entity.ToTable("Case_Care_Records");

                entity.Property(e => e.CaseQaid)
                    .HasMaxLength(8)
                    .HasColumnName("Case_QAID");

                entity.Property(e => e.CaseHealth)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("Case_Health");

                entity.Property(e => e.CaseHome)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("Case_Home");

                entity.Property(e => e.CaseNo)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_No");

                entity.Property(e => e.CaseQ1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_q1");

                entity.Property(e => e.CaseQ1Other)
                    .HasMaxLength(100)
                    .HasColumnName("Case_q1_other");

                entity.Property(e => e.CaseQ2)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_q2");

                entity.Property(e => e.CaseQ2Other)
                    .HasMaxLength(100)
                    .HasColumnName("Case_q2_other");

                entity.Property(e => e.CaseQ3)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_q3");

                entity.Property(e => e.CaseQ3Other)
                    .HasMaxLength(100)
                    .HasColumnName("Case_q3_other");

                entity.Property(e => e.CaseQ4)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_q4");

                entity.Property(e => e.CaseQ4Other)
                    .HasMaxLength(100)
                    .HasColumnName("Case_q4_other");

                entity.Property(e => e.CaseTel)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("Case_Tel");

                entity.Property(e => e.CaseTime1)
                    .HasColumnType("datetime")
                    .HasColumnName("Case_Time1");

                entity.Property(e => e.MemSid)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Mem_SID");

                entity.HasOne(d => d.CaseNoNavigation)
                    .WithMany(p => p.CaseCareRecords)
                    .HasForeignKey(d => d.CaseNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Case_Care__Case___29221CFB");

                entity.HasOne(d => d.MemS)
                    .WithMany(p => p.CaseCareRecords)
                    .HasForeignKey(d => d.MemSid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Case_Care__Mem_S__2A164134");
            });

            modelBuilder.Entity<CaseDailyRegistration>(entity =>
            {
                entity.HasKey(e => e.CaseContId)
                    .HasName("PK__case_dai__0D7D6BC7A01B2B94");

                entity.ToTable("case_daily_registration");

                entity.Property(e => e.CaseContId)
                    .HasMaxLength(8)
                    .HasColumnName("Case_ContID");

                entity.Property(e => e.CaseNo)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_No");

                entity.Property(e => e.CasePick)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Case_Pick");

                entity.Property(e => e.CasePluse)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("Case_Pluse");
                entity.Property(e => e.CaseBlood)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("Case_Blood");

                entity.Property(e => e.CaseTemp)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("Case_Temp");

                entity.Property(e => e.Casedate)
                   .HasColumnType("datetime")
                   .HasColumnName("Case_date");

                entity.HasOne(d => d.CaseNoNavigation)
                    .WithMany(p => p.CaseDailyRegistrations)
                    .HasForeignKey(d => d.CaseNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__case_dail__Case___32E0915F");                
            });

            modelBuilder.Entity<CaseInfor>(entity =>
            {
                entity.HasKey(e => e.CaseNo)
                    .HasName("PK__case_inf__D0610232CD08B242");

                entity.ToTable("case_infor");

                entity.Property(e => e.CaseNo)
                    .HasMaxLength(8)
                    .HasColumnName("Case_No");

                entity.Property(e => e.CaseActv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Actv");

                entity.Property(e => e.CaseAddr)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Addr");

                entity.Property(e => e.CaseBd)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_BD");

                entity.Property(e => e.CaseCntAdd)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_CntAdd");

                entity.Property(e => e.CaseCntRel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_CntRel");

                entity.Property(e => e.CaseCntTel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_CntTel");

                entity.Property(e => e.CaseCnta)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Cnta");

                entity.Property(e => e.CaseDesc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Desc");

                entity.Property(e => e.CaseEdu)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Edu");

                entity.Property(e => e.CaseFactly)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Factly");

                entity.Property(e => e.CaseFami)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Fami");

                entity.Property(e => e.CaseFund)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Fund");

                entity.Property(e => e.CaseGender)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Gender");

                entity.Property(e => e.CaseHealth)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Health");

                entity.Property(e => e.CaseHouse)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_House");

                entity.Property(e => e.CaseIdcard)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("Case_IDcard");

                entity.Property(e => e.CaseIdent)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Ident");

                entity.Property(e => e.CaseLang)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Lang");

                entity.Property(e => e.CaseMari)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Mari");

                entity.Property(e => e.CaseName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Name");

                entity.Property(e => e.CasePassword)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Password");

                entity.Property(e => e.CaseProf)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Prof");

                entity.Property(e => e.CaseQues)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Ques");

                entity.Property(e => e.CaseRegName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_RegName");

                entity.Property(e => e.CaseRegTime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_RegTime");

                entity.Property(e => e.CaseRelig)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Relig");

                entity.Property(e => e.CaseSource)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Source");

                entity.Property(e => e.CaseUnitName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Case_UnitName");

                entity.Property(e => e.CaseUnitNum)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_UnitNum");

                entity.Property(e => e.CaseWork)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_Work");
            });

            modelBuilder.Entity<CaseNeed>(entity =>
            {
                entity.ToTable("Case_Need");

                entity.Property(e => e.CaseNeedId)
                    .HasMaxLength(8)
                    .HasColumnName("Case_NeedID");

                entity.Property(e => e.CaseAct)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Act");

                entity.Property(e => e.CaseCare)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Care");

                entity.Property(e => e.CaseCons)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Cons");

                entity.Property(e => e.CaseEat)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Eat");

                entity.Property(e => e.CaseFami)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Fami");

                entity.Property(e => e.CaseHear)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Hear");

                entity.Property(e => e.CaseMed)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Med");

                entity.Property(e => e.CaseNo)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_No");

                entity.Property(e => e.CaseRead)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Read");

                entity.Property(e => e.CaseSee)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_See");

                entity.Property(e => e.CaseSpeak)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Speak");

                entity.Property(e => e.CaseView1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_View1");

                entity.Property(e => e.CaseView2)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_View2");

                entity.Property(e => e.CaseView3)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_View3");

                entity.Property(e => e.CaseView4)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_View4");

                entity.Property(e => e.CaseView5)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_View5");

                entity.Property(e => e.CaseView6)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_View6");

                entity.Property(e => e.CaseView7)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_View7");

                entity.Property(e => e.CaseView8)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_View8");

                entity.Property(e => e.CaseView9)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Case_View9");

                entity.HasOne(d => d.CaseNoNavigation)
                    .WithMany(p => p.CaseNeeds)
                    .HasForeignKey(d => d.CaseNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Case_Need__Case___36B12243");
            });

            modelBuilder.Entity<CasePhysicalMental>(entity =>
            {
                entity.HasKey(e => e.CaseQaid)
                    .HasName("PK__case_phy__21B54A278516737A");

                entity.ToTable("case_physical_mental");

                entity.Property(e => e.CaseQaid)
                    .HasMaxLength(8)
                    .HasColumnName("Case_QAID");

                entity.Property(e => e.CaseContent1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content1");

                entity.Property(e => e.CaseContent10)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content10");

                entity.Property(e => e.CaseContent11)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content11");

                entity.Property(e => e.CaseContent12)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content12");

                entity.Property(e => e.CaseContent13)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content13");

                entity.Property(e => e.CaseContent2)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content2");

                entity.Property(e => e.CaseContent3)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content3");

                entity.Property(e => e.CaseContent4)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content4");

                entity.Property(e => e.CaseContent5)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content5");

                entity.Property(e => e.CaseContent6)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content6");

                entity.Property(e => e.CaseContent7)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content7");

                entity.Property(e => e.CaseContent8)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content8");

                entity.Property(e => e.CaseContent9)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Case_Content9");

                entity.Property(e => e.CaseFre)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Case_Fre");

                entity.Property(e => e.CaseLive)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Case_Live");

                entity.Property(e => e.CaseNo)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_No");

                entity.HasOne(d => d.CaseNoNavigation)
                    .WithMany(p => p.CasePhysicalMentals)
                    .HasForeignKey(d => d.CaseNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__case_phys__Case___2CF2ADDF");
            });

            modelBuilder.Entity<CaseTelRecord>(entity =>
            {
                entity.HasKey(e => e.CaseTelQaid)
                    .HasName("PK__Case_Tel__8C289C6576F0578D");

                entity.ToTable("Case_Tel_Records");

                entity.Property(e => e.CaseTelQaid)
                    .HasMaxLength(8)
                    .HasColumnName("Case_TelQAID");

                entity.Property(e => e.CaseAns)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Ans");

                entity.Property(e => e.CaseCom)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Case_Com");

                entity.Property(e => e.CaseExp)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Exp");

                entity.Property(e => e.CaseFam)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Fam");

                entity.Property(e => e.CaseHea)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Hea");

                entity.Property(e => e.CaseLive)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Live");

                entity.Property(e => e.CaseMental)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_Mental");

                entity.Property(e => e.CaseNo)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Case_No");

                entity.Property(e => e.CaseRegTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Case_RegTime");

                entity.Property(e => e.CaseSick)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Case_Sick");

                entity.Property(e => e.CaseTelTime1)
                    .HasColumnType("datetime")
                    .HasColumnName("Case_TelTime1");

                entity.Property(e => e.CaseTelTime2)
                    .HasColumnType("datetime")
                    .HasColumnName("Case_TelTime2");

                entity.Property(e => e.MemSid)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Mem_SID");

                entity.HasOne(d => d.CaseNoNavigation)
                    .WithMany(p => p.CaseTelRecords)
                    .HasForeignKey(d => d.CaseNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Case_Tel___Case___2FCF1A8A");

                entity.HasOne(d => d.MemS)
                    .WithMany(p => p.CaseTelRecords)
                    .HasForeignKey(d => d.MemSid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Case_Tel___Mem_S__30C33EC3");
            });

            modelBuilder.Entity<LectureClass>(entity =>
            {
                entity.HasKey(e => e.SchWeek)
                    .HasName("PK__lecture___CE91D8F57C0189BF");

                entity.ToTable("lecture_class");

                entity.Property(e => e.SchWeek)
                    .HasMaxLength(10)
                    .HasColumnName("Sch_Week");

                entity.Property(e => e.SchA)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Sch_A");

                entity.Property(e => e.SchB)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Sch_B");

                entity.Property(e => e.SchC)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Sch_C");

                entity.Property(e => e.SchD)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Sch_D");

                entity.Property(e => e.Weeknum).HasColumnName("weeknum");
            });

            modelBuilder.Entity<LectureTable>(entity =>
            {
                entity.HasKey(e => e.LecId)
                    .HasName("PK__lecture___2E5B8E905793C8AC");

                entity.ToTable("lecture_table");

                entity.Property(e => e.LecId)
                    .HasMaxLength(8)
                    .HasColumnName("Lec_ID");

                entity.Property(e => e.LecAim)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Lec_Aim");

                entity.Property(e => e.LecClass)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Lec_Class");

                entity.Property(e => e.LecDate)
                    .HasColumnType("date")
                    .HasColumnName("Lec_Date");

                entity.Property(e => e.LecLeader)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Lec_Leader");

                entity.Property(e => e.LecPla)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Lec_Pla");

                entity.Property(e => e.LecStep)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Lec_Step");

                entity.Property(e => e.LecTheme)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Lec_Theme");

                entity.Property(e => e.LecTool)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Lec_Tool");

                entity.Property(e => e.MemSid)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Mem_SID");

                entity.HasOne(d => d.MemS)
                    .WithMany(p => p.LectureTables)
                    .HasForeignKey(d => d.MemSid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__lecture_t__Mem_S__4222D4EF");
            });

            modelBuilder.Entity<MemSign>(entity =>
            {
                entity.HasKey(e => e.MemSignQaid)
                    .HasName("PK__Mem_Sign__5DC3E46BF2F8F753");

                entity.ToTable("Mem_Sign");

                entity.Property(e => e.MemSignQaid)
                    .HasMaxLength(8)
                    .HasColumnName("Mem_SignQAID");

                entity.Property(e => e.MemRecord)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("Mem_Record");

                entity.Property(e => e.MemSid)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Mem_SID");

                entity.Property(e => e.MemTelTime1)
                    .HasColumnType("datetime")
                    .HasColumnName("Mem_TelTime1");

                entity.Property(e => e.MemTelTime2)
                    .HasColumnType("datetime")
                    .HasColumnName("Mem_TelTime2");

                entity.HasOne(d => d.MemS)
                    .WithMany(p => p.MemSigns)
                    .HasForeignKey(d => d.MemSid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Mem_Sign__Mem_SI__339FAB6E");
            });

            modelBuilder.Entity<MemberInformation>(entity =>
            {
                entity.HasKey(e => e.MemSid)
                    .HasName("PK__Member_I__47D0834AC937CAE3");

                entity.ToTable("Member_Information");

                entity.Property(e => e.MemSid)
                    .HasMaxLength(8)
                    .HasColumnName("Mem_SID");

                entity.Property(e => e.MemAddress)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Mem_Address");

                entity.Property(e => e.MemBd)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Mem_BD");

                entity.Property(e => e.MemCert)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Mem_Cert");

                entity.Property(e => e.MemEdu)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Mem_Edu");

                entity.Property(e => e.MemExpr)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Mem_Expr");

                entity.Property(e => e.MemGender)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Mem_Gender");

                entity.Property(e => e.MemIdent)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Mem_Ident");

                entity.Property(e => e.MemMovt)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Mem_Movt");

                entity.Property(e => e.MemMphone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Mem_MPhone");

                entity.Property(e => e.MemName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Mem_Name");

                entity.Property(e => e.MemPassword)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Mem_Password");

                entity.Property(e => e.MemProf)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Mem_Prof");

                entity.Property(e => e.MemPserv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Mem_PServ");

                entity.Property(e => e.MemSerRec)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Mem_SerRec");

                entity.Property(e => e.MemSite)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Mem_Site");

                entity.Property(e => e.MemTphone)
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasColumnName("Mem_Tphone");

                entity.Property(e => e.MemTrans)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Mem_Trans");

                entity.Property(e => e.MemUid)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Mem_UID");

                entity.Property(e => e.MemUnitName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Mem_UnitName");

                entity.Property(e => e.MemUnitNum)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Mem_UnitNum");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
