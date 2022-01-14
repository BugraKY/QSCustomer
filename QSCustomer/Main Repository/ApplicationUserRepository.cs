using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(ApplicationUser applicationUser)
        {
            var data = _db.ApplicationUsers.FirstOrDefault(i=>i.Id == applicationUser.Id);
        }
    }
}
