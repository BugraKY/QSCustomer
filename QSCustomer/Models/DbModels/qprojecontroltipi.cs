using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.DbModels
{
    public class qprojecontroltipi
    {
        [Key]
        public int id { get; set; }
        public string controlType { get; set; }
        public decimal yurticiFiyat { get; set; }
        public decimal yurtdisiFiyat { get; set; }
    }
}
