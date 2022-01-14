using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class ProjeDetayRepository : Repository<qprojeDetay>, IProjeDetayRepository
    {
        private readonly SecondDbContext _db;

        public ProjeDetayRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(qprojeDetay qprojedetay)
        {
            
            var data = _db.qprojedetay.FirstOrDefault(i=>i.id== qprojedetay.id);
        }
    }
}
