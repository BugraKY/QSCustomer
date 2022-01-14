using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class ProjeDurumuRepository : Repository<qprojedurumu>, IProjeDurumuRepository
    {
        private readonly SecondDbContext _db;

        public ProjeDurumuRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(qprojedurumu qprojedurumu)
        {
            
            var data = _db.qprojedurumu.FirstOrDefault(i=>i.id== qprojedurumu.id);
        }
    }
}
