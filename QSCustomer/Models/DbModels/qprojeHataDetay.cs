using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.DbModels
{
    public class qprojeHataDetay
    {
        [Key]
        public int id { get; set; }
        public int idProje { get; set; }
        public int idProjeDetay { get; set; }
        public int idProjeDetays { get; set; }
        public int idHataTanimi { get; set; }
        public int Adet { get; set; }
    }
}
