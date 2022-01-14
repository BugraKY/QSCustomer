using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class FabrikaTanimRepository : Repository<fabrikatanim>, IFabrikaTanimRepository
    {
        private readonly SecondDbContext _db;

        public FabrikaTanimRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(fabrikatanim fabrikatanim)
        {
            var data = _db.fabrikatanim.FirstOrDefault(i=>i.id== fabrikatanim.id);
        }
    }
}
