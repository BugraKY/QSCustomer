using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class ProjeHataDetayRepository : Repository<qprojeHataDetay>, IProjeHataDetayRepository
    {
        private readonly SecondDbContext _db;

        public ProjeHataDetayRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(qprojeHataDetay qprojeHataDetay)
        {
            
            var data = _db.qprojeHataDetay.FirstOrDefault(i=>i.id== qprojeHataDetay.id);
        }
    }
}
