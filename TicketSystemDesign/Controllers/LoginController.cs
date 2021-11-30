using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using TicketSystemDesign.Models;
using System.Text;

namespace TicketSystemDesign.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private TickerSystemContext _context;
        private TicketDac userInfoDac = new TicketDac();

        public LoginController(IConfiguration config , TickerSystemContext context)
        {
            _config = config;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Login Operation
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string Account, string pd, string ReturnUrl)
        {
            // Get which user want to logining 
            Models.UserInfo userInfo = TicketDac.GetUserInfo(Account);

            if (userInfo == null)
            {
                ViewBag.errMsg = "無該使用者";
                return View();
            }

            string hashText = CommonHelper.GetHastText(pd, userInfo.Salt);

            if (!(Account == userInfo.UserName && hashText == userInfo.Pwd))
            {
                ViewBag.errMsg = "帳號或密碼輸入錯誤";
                return View();
            }

            // Store the user infomation about stauts and user account
            Claim[] claims = new[] {  new Claim("UserName", Account),
                                      new Claim("RoleStatus", userInfo.Status.ToString()),
                                      new Claim("UserId", userInfo.UserId.ToString()),}; 
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            double loginExpireMinute = this._config.GetValue<double>("LoginExpireMinute");

            await HttpContext.SignInAsync(principal, 
                                            new AuthenticationProperties()
                                            {
                                                IsPersistent = false, // When close the browser immediately logout.
                                            });

            // Prevent open direct attacl
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Logout Operation
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }
    }
}
