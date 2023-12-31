﻿using long_term_care.Helpers;
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
using Newtonsoft.Json.Linq;
using System;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using SkiaSharp;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Net.Http;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using static long_term_care.Models.CaseInfor;

namespace long_term_care.Controllers
{
    public class MainController : Controller
    {
        private readonly longtermcareContext _context;
        private HttpClient httpClient;


        public MainController(longtermcareContext context)
        {
            _context = context;
        }
        public IActionResult ElderRegistration()
        {
            string nextFormNumber = "";

            var lastForm = _context.CaseInfors.OrderByDescending(f => f.CaseNo).FirstOrDefault();
            if (lastForm != null)
            {
                int lastFormNumber;
                if (int.TryParse(lastForm.CaseNo.Substring(1), out lastFormNumber))
                {
                    int nextFormNumberInt = lastFormNumber + 1;
                    nextFormNumber = $"C{nextFormNumberInt.ToString("0000")}"; // 在流水号前添加字母前缀 "M"
                }
                else
                {
                    // 处理解析失败的情况
                    // 可以抛出异常、使用默认值或采取其他适当的操作
                }
            }
            else
            {
                nextFormNumber = "C0001"; // 初始流水号添加字母前缀 "M"
            }

            ViewData["CaseNo"] = nextFormNumber;
            return View();
        }
        [HttpPost]
        public IActionResult ElderRegistration([FromBody] MainViewModel model)
        {
            string userName = User.Identity.Name;
            var memdName = _context.MemberInformations.FirstOrDefault(x => x.MemSid == userName);

            var entity = new CaseInfor
            {
                CaseNo = model.CaseNo,
                CaseUnitName = model.CaseUnitName,
                CaseUnitNum = model.CaseUnitNum,
                CaseName = model.CaseName,
                CaseIdcard = model.CaseIdcard,
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
                CaseRegName = memdName.MemName,
                CaseRegTime = model.CaseRegTime,
                CaseIcnum = model.CaseIcnum,
            };
            _context.CaseInfors.Add(entity);
            _context.SaveChanges();
            return View();
        }
        public IActionResult MemAdd()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://www.ris.gov.tw/rs-opendata/api/v1/datastore/ODRP049/111");
            string nextFormNumber_Mem = "";
            var lastForm_Mem = _context.MemberInformations.OrderByDescending(f => f.MemSid).FirstOrDefault();
            if (lastForm_Mem != null)
            {
                int lastFormNumber_Mem;
                if (int.TryParse(lastForm_Mem.MemSid.Substring(1), out lastFormNumber_Mem))
                {
                    int nextFormNumberInt_Mem = lastFormNumber_Mem + 1;
                    nextFormNumber_Mem = $"M{nextFormNumberInt_Mem:D4}"; // 添加字母前缀 "M" 并格式化流水号
                }
                else
                {
                    // 处理解析失败的情况
                    // 可以抛出异常、使用默认值或采取其他适当的操作
                }
            }
            else
            {
                nextFormNumber_Mem = "M0001"; // 初始流水号添加字母前缀 "M"
            }

            ViewData["MemSid"] = nextFormNumber_Mem;

            return View();
        }
     


        

        [HttpPost]
        public IActionResult MemAdd([FromBody] MainViewModel model)
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
                    MemUnitNum = model.MemUnitNum,
                    MemIcnum = model.MemIcnum,

                };
                _context.MemberInformations.Add(member);
                _context.SaveChanges();
            }
           
            return View();
        }
        private string GenerateVerificationCode(int length = 6)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public ActionResult VerificationCodeImage()
        {
            string verificationCode = HttpContext.Session.GetString("VerificationCode") as string;
            if (string.IsNullOrEmpty(verificationCode))
            {
                verificationCode = GenerateVerificationCode();
                HttpContext.Session.SetString("VerificationCode", verificationCode);
            }

            using (SKBitmap bitmap = new SKBitmap(150, 60))
            {
                using (SKCanvas canvas = new SKCanvas(bitmap))
                {
                    // 設定背景顏色
                    canvas.Clear(SKColors.White);

                    // 設定字體和顏色
                    using (SKPaint paint = new SKPaint())
                    {
                        paint.TextSize = 30;
                        paint.Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold);
                        paint.Color = SKColors.Black;

                        // 繪製驗證碼文字
                        SKRect textBounds = new SKRect();
                        paint.MeasureText(verificationCode, ref textBounds);

                        float x = (bitmap.Width - textBounds.Width) / 2;
                        float y = (bitmap.Height - textBounds.Height) / 2 + textBounds.Height;

                        canvas.DrawText(verificationCode, x, y, paint);
                    }

                    // 儲存圖片到記憶體串流
                    using (SKData data = bitmap.Encode(SKEncodedImageFormat.Jpeg, 100))
                    {
                        // 將記憶體串流轉換為位元組陣列
                        byte[] imageData = data.ToArray();

                        // 返回圖片的檔案結果
                        return File(imageData, "image/jpeg");
                    }
                }
            }



        }

        public IActionResult Login()
        {
            string verificationCode = GenerateVerificationCode();
            HttpContext.Session.SetString("VerificationCode", verificationCode);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string id, string password, string verificationCode)
        {
            MemberInformation user = await _context.MemberInformations.FirstOrDefaultAsync(x => x.MemSid == id);
            string serverVerificationCode = HttpContext.Session.GetString("VerificationCode");

            if (user != null)
            {
                // 验证密码
                bool isPasswordValid = PasswordHelper.VerifyPassword(password, user.MemPassword);

                if (isPasswordValid)
                {
                    if (string.Equals(verificationCode, serverVerificationCode, StringComparison.OrdinalIgnoreCase))
                    {
                        var claims = new List<Claim>
                        {
                          new Claim(ClaimTypes.Name,user.MemSid)
                        };

                        if (user.RoleId == "1")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "管理員"));

                        }
                        else if (user.RoleId == "2")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "社工"));

                        }
                        else if (user.RoleId == "3")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "志工"));
                        }
                        else if (user.RoleId == "4")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "志工簽到負責人"));
                        }
                        else if (user.RoleId == "5")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "課表負責人"));
                        }
                        else if (user.RoleId == "6")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "活動設計負責人"));
                        }


                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties()
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
                        // 验证码不正确
                        ModelState.AddModelError(string.Empty, "驗證碼錯誤");
                    }
                }
                else
                {
                    // 密码不正确
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








        /* [Authorize]*/
        public IActionResult MemMainpage()
        {
            /*string userName = User.Identity.Name;
            var MemName = _context.MemberInformations.FirstOrDefault(x => x.MemSid == userName);
            var name = MemName.MemName;
            ViewData["name"] = name;*/
            return View();
        }
        [Authorize]
        public IActionResult Memprofile()
        {
            string userName = User.Identity.Name;
            var MemName = _context.MemberInformations.FirstOrDefault(x => x.MemSid == userName);
            return View(MemName);
        }
        [Authorize]
        public IActionResult FixMemprofile()
        {
            string userName = User.Identity.Name;
            var MemName = _context.MemberInformations.FirstOrDefault(x => x.MemSid == userName);
            return View(MemName);
        }
        [HttpPost]
        public async Task<IActionResult> FixMemprofileAsync(string id, [Bind("MemSid,MemUnitName,MemUnitNum,MemName,MemBd,MemUid,MemPassword,MemGender,MemTphone,MemMphone,MemAddress,MemSite,MemProf,MemCert,MemTrans,MemExpr,MemMovt,MemPserv,MemIdent,MemSerRec,MemEdu,RoleId")] MemberInformation memberInformation)
        {

            memberInformation.MemTphone = memberInformation.MemTphone ?? "";
            memberInformation.MemMphone = memberInformation.MemMphone ?? "";
            memberInformation.MemAddress = memberInformation.MemAddress ?? "";
            memberInformation.MemSite = memberInformation.MemSite ?? "";
            memberInformation.MemProf = memberInformation.MemProf ?? "";
            memberInformation.MemCert = memberInformation.MemCert ?? "";
            memberInformation.MemTrans = memberInformation.MemTrans ?? "";
            memberInformation.MemExpr = memberInformation.MemExpr ?? "";
            memberInformation.MemMovt = memberInformation.MemMovt ?? "";
            memberInformation.MemPserv = memberInformation.MemPserv ?? "";
            memberInformation.MemIdent = memberInformation.MemIdent ?? "";
            memberInformation.MemSerRec = memberInformation.MemSerRec ?? "";
            memberInformation.MemEdu = memberInformation.MemEdu ?? "";
            memberInformation.MemIcnum = memberInformation.MemIcnum ?? "";

            _context.Update(memberInformation);
            await _context.SaveChangesAsync();
            return View(memberInformation);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // 执行登出逻辑，例如清除用户的身份验证凭据
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // 返回重定向到登录页面或其他页面的响应
            return RedirectToAction("Login", "Main");
        }
        [Authorize(Roles = "管理員")]

        public IActionResult Roleset()
        {
            var members = _context.MemberInformations.Where(x => x.RoleId != "1").ToList();

            var ViewModel = new RolesetViewModel
            {
                Members = members
            };
            return View(ViewModel);
        }
        [HttpPost]
        public IActionResult Roleset(string id, string roleid)
        {
            var member = _context.MemberInformations.FirstOrDefault(x => x.MemSid == id);
            member.RoleId = roleid;
            _context.SaveChanges();

            var members = _context.MemberInformations.Where(x => x.RoleId != "1").ToList();

            var ViewModel = new RolesetViewModel
            {
                Members = members
            };
            TempData["SuccessMessage"] = "Success";
            return View(ViewModel);
        }
        [HttpPost]
        public ActionResult ButtonClick(string buttonId)
        {
            switch (buttonId)
            {
                case "Button1":
                    return RedirectToAction("Index", "MemSigns");
                case "Button2":
                    return RedirectToAction("Index", "LectureTables");
                case "Button3":
                    return RedirectToAction("Index", "LectureClasses");
                case "Button4":
                    return RedirectToAction("Index", "CaseActsign");
                case "Button5":
                    return RedirectToAction("Index", "CasePhysicalMentals");
                case "Button6":
                    return RedirectToAction("Index", "CaseTelRecords");
                case "Button7":
                    return RedirectToAction("Index", "CaseCareRecords");
                case "Button8":
                    return RedirectToAction("Index", "CarPicks");
                case "Button9":
                    return RedirectToAction("Index", "CaseDailyRegistrations");
                case "Button10":
                    return RedirectToAction("Index", "CaseNeeds");
                default:
                    // 如果按鈕ID無效，返回控制器A的視圖或顯示錯誤信息
                    return RedirectToAction("ActionName", "ControllerA");
            }
        }

    }
}