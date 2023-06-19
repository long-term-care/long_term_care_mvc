using long_term_care.Helpers;
using long_term_care.Models;
using long_term_care.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;

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
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string id ,string password)
        {

            MemberInformation user = await _context.MemberInformations.FirstOrDefaultAsync(x => x.MemSid == id);

            if (user != null)
            {
                // 验证密码
                bool isPasswordValid = PasswordHelper.VerifyPassword(password, user.MemPassword);

                if (isPasswordValid)
                {
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name,id),
                    //new Claim(ClaimTypes.Role, "Administrator") // 如果要有「群組、角色、權限」，可以加入這一段  
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                       
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                        );

                    return LocalRedirect("~/Main/MemMainpage");
                }
                else
                {
                    // 密码验证失败
                    ModelState.AddModelError(string.Empty, "密碼錯誤");
                }
                }
                else
                {
                    // 用户不存在
                    ModelState.AddModelError(string.Empty, "找不到該用戶");
                }

                return View();
        }
        
        [Authorize]
        public IActionResult MemMainpage()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // 执行登出逻辑，例如清除用户的身份验证凭据
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // 返回重定向到登录页面或其他页面的响应
            return RedirectToAction("Login", "Main");
        }

    }
}
