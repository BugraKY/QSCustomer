using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QSCustomer.IMainRepository;
using QSCustomer.Models.ViewModels;
using static QSCustomer.Utility.ProjectConstant;

namespace QSCustomer.ViewComponents
{
    public class GetUser : ViewComponent
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _hostEnvironment;

        public GetUser(IUnitOfWork uow, IWebHostEnvironment hostEnvironment)
        {
            _uow = uow;
            _hostEnvironment = hostEnvironment;
        }
        public IViewComponentResult Invoke()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (Claims != null)
            {
                var AppUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value,includeProperties: "UserTypes");
                if (AppUser.UserTypes.Name == UserTypeConst.Customer)
                {
                    var firstdef = _uow.DefinitionUser.GetFirstOrDefault(i => i.UserId == AppUser.Id);
                    var _customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == firstdef.DefinitionId);
                    var _user = new User()
                    {
                        Email = AppUser.Email,
                        NameField = _customer.musteriAdi,
                        UserType= UserTypeConst.Customer

                    };
                    return View("default", _user);
                }
                if (AppUser.UserTypes.Name == UserTypeConst.Operation_Area)
                {
                    var _operation = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == AppUser.DefinitionId);
                    var _user = new User()
                    {
                        Email = AppUser.Email,
                        NameField = _operation.fabrikaAdi,
                        UserType = UserTypeConst.Operation_Area

                    };
                    return View("default", _user);
                }
            }
            User _userNull = null;
            return View("default", _userNull);
        }
    }
}
