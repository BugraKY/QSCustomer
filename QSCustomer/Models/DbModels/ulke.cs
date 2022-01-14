using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.DbModels
{
    public class ulke
    {
        [Key]
        public int id { get; set; }
        public string ulkeAdi { get; set; }
    }
}
