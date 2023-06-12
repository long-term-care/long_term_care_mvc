using long_term_care.Helpers;
using long_term_care.Models;
using long_term_care.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace long_term_care.Controllers
{
    public class MainController : Controller
    {
        private readonly longtermcareContext _context;

        public MainController(longtermcareContext context)
        {
            _context = context;
        }
        public IActionResult Caseinfor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Caseinfor([FromBody] MainViewModel model)
        {
            if (model.type == 1)
            {

                var member = new MemberInformation
                {
                    MemSid = model.MemSid,
                    MemAddress = model.MemAddress,
                    MemBd = model.MemBd,
                    MemCert = model.MemCert,
                    MemEdu = model.MemEdu,
                    MemExpr = model.MemExpr,
                    MemGender = model.MemGender,
                    MemIdent = model.MemIdent,
                    MemMovt = model.MemMovt,
                    MemMphone = model.MemMphone,
                    MemName = model.MemName,
                    MemPassword = PasswordHelper.ComputeSHA256Hash(model.MemPassword),
                    MemProf = model.MemProf,
                    MemPserv = model.MemPserv,
                    MemSerRec = model.MemSerRec,
                    MemSite = model.MemSite,
                    MemTphone = model.MemTphone,
                    MemTrans = model.MemTrans,
                    MemUid = model.MemUid,
                    MemUnitName = model.MemUnitName,
                    MemUnitNum = model.MemUnitNum
                };
                _context.MemberInformations.Add(member);
                _context.SaveChanges();
            }
            if (model.type == 2)
            {
                var entity = new CaseInfor
                {
                    CaseNo = model.CaseNo,
                    CaseUnitName = model.CaseUnitName,
                    CaseUnitNum = model.CaseUnitNum,
                    CaseName = model.CaseName,
                    CaseIdcard = model.CaseIdcard,
                    CasePassword = PasswordHelper.ComputeSHA256Hash(model.CasePassword),
                    CaseGender = model.CaseGender,
                    CaseRelig = model.CaseRelig,
                    CaseBd = model.CaseBd,
                    CaseLang = model.CaseLang,
                    CaseSource = model.CaseSource,
                    CaseWork = model.CaseWork,
                    CaseProf = model.CaseProf,
                    CaseEdu = model.CaseEdu,
                    CaseAddr = model.CaseAddr,
                    CaseHouse = model.CaseHouse,
                    CaseIdent = model.CaseIdent,
                    CaseFund = model.CaseFund,
                    CaseHealth = model.CaseHealth,
                    CaseActv = model.CaseActv,
                    CaseFactly = model.CaseFactly,
                    CaseMari = model.CaseMari,
                    CaseCnta = model.CaseCnta,
                    CaseCntTel = model.CaseCntTel,
                    CaseCntRel = model.CaseCntRel,
                    CaseCntAdd = model.CaseCntAdd,
                    CaseFami = model.CaseFami,
                    CaseQues = model.CaseQues,
                    CaseDesc = model.CaseDesc,
                    CaseRegName = model.CaseRegName,
                    CaseRegTime = model.CaseRegTime,
                };
                _context.CaseInfors.Add(entity);
                _context.SaveChanges();
            }
            return View();
        }
    }
}
