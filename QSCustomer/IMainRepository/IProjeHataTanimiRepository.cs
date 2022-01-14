using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.IMainRepository
{
    public interface IProjeHataTanimiRepository : IRepository<qprojehataTanimi>
    {
        void Update(qprojehataTanimi qprojehataTanimi);
    }
}
