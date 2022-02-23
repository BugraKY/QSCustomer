using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QSCustomer.Models.DbModels
{
    public class fabrikatanimyetkili
    {
        [Key]
        public int id { get; set; }
        public int idFabrikaTanim { get; set; }
        public string isim { get; set; }
        public string gorevi { get; set; }
        public string tel { get; set; }
        public string mail { get; set; }
        public string kullaniciAdi { get; set; }
        public string sifre { get; set; }
    }
}
