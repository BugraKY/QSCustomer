using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class MusteriTanimRepository : Repository<musteritanim>, IMusteriTanimRepository
    {
        private readonly SecondDbContext _db;

        public MusteriTanimRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(musteritanim musteritanim)
        {
            var data = _db.musteritanim.FirstOrDefault(i=>i.id== musteritanim.id);
        }
    }
}
