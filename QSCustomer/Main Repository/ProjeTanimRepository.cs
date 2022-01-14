using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class ProjeTanimRepository : Repository<qprojetanim>, IProjeTanimRepository
    {
        private readonly SecondDbContext _db;

        public ProjeTanimRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(qprojetanim qprojetanim)
        {
            
            var data = _db.qprojepartNrTanimi.FirstOrDefault(i=>i.id== qprojetanim.id);
        }
    }
}
