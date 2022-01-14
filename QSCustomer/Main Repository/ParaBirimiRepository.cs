using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class ParaBirimiRepository : Repository<paraBirimi>, IParaBirimiRepository
    {
        private readonly SecondDbContext _db;

        public ParaBirimiRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(paraBirimi paraBirimi)
        {
            
            var data = _db.paraBirimi.FirstOrDefault(i=>i.id== paraBirimi.id);
        }
    }
}
