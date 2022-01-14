using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class MusteriYetkiliRepository : Repository<musteriYetkili>, IMusteriYetkiliRepository
    {
        private readonly SecondDbContext _db;

        public MusteriYetkiliRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(musteriYetkili musteriYetkili)
        {
            
            var data = _db.musteriYetkili.FirstOrDefault(i=>i.id==musteriYetkili.id);
        }
    }
}
