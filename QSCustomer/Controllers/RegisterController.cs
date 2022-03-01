using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using QSCustomer.Extensions;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSCustomer.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ILogger<RegisterController> _logger;
        private readonly IUnitOfWork _uow;

        public RegisterController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterController> logger,
            //RoleManager<ApplicationUser> roleManager,
            IUnitOfWork uow

            )
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            //_roleManager = roleManager;
            _uow = uow;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        [Route("signup")]
        public IActionResult Index()
        {/*
            Request;
            Microsoft.AspNetCore.Http.HttpRequest*/
            return View();
        }
        [HttpPost("signup")]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ApplicationUser user = new ApplicationUser();
            bool posted = true;
            //Input.IsCompany=Convert.ToBoolean(RegCheck) ;
            returnUrl = returnUrl ?? Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };


                var IsOperationArea = _uow.FabrikaTanimYetkili.GetFirstOrDefault(i => i.mail == Input.Email);

                var IsCustomer = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == Input.Email);
                if (IsOperationArea != null)
                {

                    if (IsOperationArea == null && IsCustomer==null)
                    {
                        ModelState.AddModelError(string.Empty, "You are not our Customers with this mail: " + user.Email);
                    }
                    else
                    {
                        var _user = new ApplicationUser
                        {
                            UserName = Input.Email,
                            Email = Input.Email,
                            Status = false,
                            EmailConfirmed = true,
                            DefinitionId=IsOperationArea.idFabrikaTanim,
                            UserTypeId = 1
                        };
                        user = _user;
                    }

                    //return View("Index");
                }
                else if(IsCustomer != null)
                {

                    var _user = new ApplicationUser
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        Status = false,
                        EmailConfirmed = true,
                        DefinitionId = IsCustomer.id,
                        UserTypeId = 2
                    };
                    user = _user;
                }
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Kullanıcı Kayıt işlemi yaptı. Email gönderiliyor..");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    EmailSenderExtension.SendEmail(Input.Email, callbackUrl);

                    /*Email Send*/

                    if (IsOperationArea != null)
                    {
                        var _definitionUserOperation = new DefinitionUsers()
                        {
                            UserId = user.Id,
                            DefinitionId = IsOperationArea.idFabrikaTanim
                        };
                        if (IsCustomer != null)
                        {
                            var _definitionUserCustomer = new DefinitionUsers()
                            {
                                UserId = user.Id,
                                DefinitionId = IsCustomer.idMusteriTanim
                            };
                            _uow.DefinitionUser.Add(_definitionUserCustomer);
                        }
                        _uow.DefinitionUser.Add(_definitionUserOperation);
                    }
                    else if (IsCustomer != null)
                    {

                        var _definitionUserCustomer = new DefinitionUsers()
                        {
                            UserId = user.Id,
                            DefinitionId = IsCustomer.idMusteriTanim
                        };
                        _uow.DefinitionUser.Add(_definitionUserCustomer);
                    }
                    _uow.Save();


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        //return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = "~/" });
                        return RedirectToAction("SuccessResult", posted);
                        //return Redirect("~/");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            // If we got this far, something failed, redisplay form

            return LocalRedirect(returnUrl);


        }
        //[HttpPost("signup")]
        public IActionResult SuccessResult(bool posted)
        {
            if (posted)
                return View();
            return NotFound();
        }
        public class InputModel
        {
            [Required]
            public string CompanyName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
        }




    }
}
