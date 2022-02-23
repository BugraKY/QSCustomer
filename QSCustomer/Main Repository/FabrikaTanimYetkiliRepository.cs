using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.MainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class FabrikaTanimYetkiliRepository : Repository<fabrikatanimyetkili>, IFabrikaTanimYetkiliRepository
    {
        private readonly SecondDbContext _db;

        public FabrikaTanimYetkiliRepository(SecondDbContext db)
            : base(db)
        {
            _db = db;
        }

        public void Update(fabrikatanimyetkili fabrikatanimyetkili)
        {
            _db.Update(fabrikatanimyetkili);
        }
    }
}
