using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.DbModels
{
    public class musteritanim
    {
        [Key]
        public int id { get; set; }
        public string musteriAdi { get; set; }
        public string aktif { get; set; }
        public string sehir { get; set; }
        public string ulke { get; set; }
        public int idSirket { get; set; }
    }
}
