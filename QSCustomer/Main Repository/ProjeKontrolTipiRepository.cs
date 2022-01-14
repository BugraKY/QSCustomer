using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class ProjeKontrolTipiRepository : Repository<qprojecontroltipi>, IProjeKontrolTipiRepository
    {
        private readonly SecondDbContext _db;

        public ProjeKontrolTipiRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(qprojecontroltipi qprojecontroltipi)
        {
            
            var data = _db.qprojecontroltipi.FirstOrDefault(i=>i.id== qprojecontroltipi.id);
        }
    }
}
