using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.DbModels
{
    public class fabrikatanim
    {
        [Key]
        public int id { get; set; }
        public string fabrikaAdi { get; set; }
        public string kod { get; set; }
        public string sayac { get; set; }
        public string aktif { get; set; }
        public string sehir { get; set; }
        public int idUlke { get; set; }
    }
}
