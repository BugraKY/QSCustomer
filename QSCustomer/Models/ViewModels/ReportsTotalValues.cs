using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.ViewModels
{
    public class ReportsTotalValues
    {

        public long qprojeDetayTKontrolAdedi { get; set; }
        public long qprojeDetayTHataAdeti { get; set; }
        public long qprojeDetayTTamirAdedi { get; set; }
        public int qprojeDetayTOperatorSayisi { get; set; }
        public long qprojeDetayToplamSuredk { get; set; }
        public double qprojeDetayGerceklesenDk { get; set; }
        public double qprojeDetayHarcananSaat { get; set; }
        public double qprojeDetayToplam { get; set; }
    }
}
