using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class UserTypeRepository : Repository<UserType>, IUserTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public UserTypeRepository(ApplicationDbContext db)
    : base(db)
        {
            _db = db;
        }

        public void Update(UserType userType)
        {
            _db.Update(userType);
        }
    }
}
