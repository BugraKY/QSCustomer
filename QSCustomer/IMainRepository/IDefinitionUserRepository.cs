using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.IMainRepository
{
    public interface IDefinitionUserRepository : IRepository<DefinitionUsers>
    {
        void Update(DefinitionUsers definitionUsers);
    }
}
