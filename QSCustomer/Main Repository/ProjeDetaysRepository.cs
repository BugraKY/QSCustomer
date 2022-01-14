using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class ProjeDetaysRepository : Repository<qprojeDetays>, IProjeDetaysRepository
    {
        private readonly SecondDbContext _db;

        public ProjeDetaysRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(qprojeDetays qprojedetays)
        {
            
            var data = _db.qprojedetays.FirstOrDefault(i=>i.id== qprojedetays.id);
        }
    }
}
