using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class UlkeRepository : Repository<ulke>, IUlkeRepository
    {
        private readonly SecondDbContext _db;

        public UlkeRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(ulke ulke)
        {
            
            var data = _db.ulke.FirstOrDefault(i=>i.id== ulke.id);
        }
    }
}
