using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork _uow;
        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IUnitOfWork uow
            )
        {
            _signInManager = signInManager;
            _logger = logger;
            _uow = uow;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
