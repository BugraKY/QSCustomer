using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class ProjeHataTanimiRepository : Repository<qprojehataTanimi>, IProjeHataTanimiRepository
    {
        private readonly SecondDbContext _db;

        public ProjeHataTanimiRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(qprojehataTanimi qprojehataTanimi)
        {
            
            var data = _db.qprojehataTanimi.FirstOrDefault(i=>i.id== qprojehataTanimi.id);
        }
    }
}
