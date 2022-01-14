using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.DbModels
{
    public class qprojedurumu
    {
        [Key]
        public int id { get; set; }
        public string projeDurumu { get; set; }
        public int sıra { get; set; }
    }
}
