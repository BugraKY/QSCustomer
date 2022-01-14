using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.DbModels
{
    public class musteriYetkili 
    {
        [Key]
        public int id { get; set; }
        public int idMusteriTanim { get; set; }
        public string isim { get; set; }
        public string gorevi { get; set; }
        public string tel { get; set; }
        public string mail { get; set; }
        public string kullaniciadi { get; set; }
        public string sifre { get; set; }
    }
}
