using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class ProjePartNrTanimiRepository : Repository<qprojepartNrTanimi>, IProjePartNrTanimiRepository
    {
        private readonly SecondDbContext _db;

        public ProjePartNrTanimiRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(qprojepartNrTanimi qprojepartNrTanimi)
        {
            
            var data = _db.qprojetanim.FirstOrDefault(i=>i.id== qprojepartNrTanimi.id);
        }
    }
}
