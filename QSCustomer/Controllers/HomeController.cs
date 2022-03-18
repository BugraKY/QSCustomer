using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QSCustomer.IMainRepository;
using QSCustomer.Models;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using QSCustomer.Extensions;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.IO;

namespace QSCustomer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _uow;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork uow, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _uow = uow;
            _userManager = userManager;

        }

        public IActionResult Index()
        {


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                var Company = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == ApplicationUser.Email);
                //var CustomerInfo = _uow.MusteriTanim.GetFirstOrDefault(i=>i.)
                if (ApplicationUser != null && ApplicationUser.EmailConfirmed == true)
                {
                    //return Redirect("/raporlar/" + Company.idMusteriTanim);
                    return RedirectToAction("Index", "Reports");
                }
                else if (ApplicationUser != null && ApplicationUser.EmailConfirmed == false)
                    return View("Unconfirmed");//2 Faktörlü onaylama tamamlanmalı. 1. Email onayı.  2. Telefon Onayı. -- Hesap onayı bekleniyor..
            }
            return Redirect("/login");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Route("/")]
        public IActionResult Unconfirmed()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                var Company = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == ApplicationUser.Email);
                //var CustomerInfo = _uow.MusteriTanim.GetFirstOrDefault(i=>i.)
                if (ApplicationUser != null && ApplicationUser.EmailConfirmed == true)
                {
                    //return Redirect("/raporlar/" + Company.idMusteriTanim);
                    return RedirectToAction("Index", "Reports");
                }
                else if (ApplicationUser != null && ApplicationUser.EmailConfirmed == false)
                    return View();//onaylanmamış hesap.
            }
            return Redirect("/login");
        }
        [HttpGet("sendmail")]
        public async Task<IActionResult> SendMailAction(string returnUrl)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                //var Company = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == ApplicationUser.Email);
                if (ApplicationUser != null && ApplicationUser.EmailConfirmed == false)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(ApplicationUser);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = ApplicationUser.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    EmailSenderExtension.SendEmail(ApplicationUser.Email, callbackUrl);
                }
                else
                    return Redirect("/");
            }
            else
                return NotFound();

            


            return RedirectToAction("MailSended", new { sendStatus = true });
            
            /*
            return Content(
                @"<form action='/sended' method='post' id='sendStatus'>
                    <input type='checkbox' name='SendStatus' checked>
                  </form>
                  <script src='/lib/jquery/dist/jquery.min.js'></script>
                  <script>
                        $(document).ready(function() {
                            $('#sendStatus').submit();
                        });
                  </script>
", "text/html"


                );*/
            

        }
        [Route("sended")]
        public IActionResult MailSended(bool SendStatus)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                if (ApplicationUser != null && ApplicationUser.EmailConfirmed == false)
                {
                    if (SendStatus)
                        return View();
                }
            }
            return NotFound();
        }
    }
}
