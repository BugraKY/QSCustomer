using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.ViewModels
{
    public class FaultString
    {
        //public List<string> FaultStringItem { get; set; } 
        //public List<qprojeHataDetay> Faults { get; set; }
        [Key]
        public int id { get; set; }
        public int idProje { get; set; }
        public int idProjeDetay { get; set; }
        public int idProjeDetays { get; set; }
        public int idHataTanimi { get; set; }
        public int Adet { get; set; }
        //public qprojehataTanimi FaultStringItem { get; set; }
        public string hataTanimi { get; set; }
    }
}
