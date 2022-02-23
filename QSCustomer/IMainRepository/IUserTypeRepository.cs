using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.IMainRepository
{
    public interface IUserTypeRepository : IRepository<UserType>
    {
        void Update(UserType userType);

    }
}
