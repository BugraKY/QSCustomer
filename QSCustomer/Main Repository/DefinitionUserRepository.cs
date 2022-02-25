using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.MainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class DefinitionUserRepository : Repository<DefinitionUsers>, IDefinitionUserRepository
    {
        private readonly ApplicationDbContext _db;

        public DefinitionUserRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(DefinitionUsers definitionUsers)
        {
            _db.Update(definitionUsers);
        }
    }
}
