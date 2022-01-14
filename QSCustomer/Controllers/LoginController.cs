using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using QSCustomer.Extensions;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSCustomer.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ILogger<LoginController> _logger;
        private readonly IUnitOfWork _uow;

        public musteritanim CustomerUser;
        public LoginController(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginController> logger,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork uow
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _uow = uow;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [Route("login")]
        public IActionResult Index()
        {
            
            #region Test Section
            EmailSenderExtension.TestRun("bugrakaya16@gmail.com", "/");
            #endregion Test Section
            return View();
        }
        /*
        [HttpPost("loginpost")]
        public IActionResult Loginpost(InputModel Input)
        {

            if (ModelState.IsValid)
            {
                CustomerUser = _uow.MusteriTanim.GetFirstOrDefault(i => i.musteriAdi == Input.musteriAdi);
            }

            return RedirectToAction("Index", "Reports", new { id = CustomerUser.id });
        }*/
        [HttpPost("login")]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = _uow.ApplicationUser.GetFirstOrDefault(u => u.Email == Input.Email);
                    _logger.LogInformation("Kullanıcı giriş yaptı." + "Kullanıcı: "+user.Email);
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Kullanıcı hesabı kilitlendi!");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    _logger.LogInformation("Kullanıcı bilgisi yalnış. - Girilen Email :"+Input.Email);
                    ModelState.AddModelError(string.Empty, "Sorry, Your e-mail address or password is incorrect. Please check your e-mail and password carefully..");
                    return View("Index");
                    //return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return View("Index", model: ModelState.Values);
        }
        
        public class InputModel
        {
            public string musteriAdi { get; set; }
            
            
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
}
