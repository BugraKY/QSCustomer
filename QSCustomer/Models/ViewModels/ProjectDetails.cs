using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace QSCustomer.Models.ViewModels
{
    public class ProjectDetails
    {
        public int Id { get; set; }
        public string? KontrolTarihi { get; set; }
        public string? UretimTarihi { get; set; }
        public string PartNrTanimi { get; set; }
        public string IotNo { get; set; }
        public string SeriNo { get; set; }
        public double Harcanansaat { get; set; }
        public int Mesai50Hesapla { get; set; }
        public double Harcanangirilenmesai { get; set; }
        public long KontrolAdedi { get; set; }
        public long TamirAdedi { get; set; }
        public long HataAdeti { get; set; }
        //public FaultString FaultString { get; internal set; }
        //public List<qprojeHataDetay> FaultString { get; set; }
        public List<FaultString> Faults { get; set; }
    }
}
