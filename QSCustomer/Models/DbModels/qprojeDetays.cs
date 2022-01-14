using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.DbModels
{
    public class qprojeDetays
    {
        [Key]
        public int id { get; set; }
        public int idProjeDetay { get; set; }
        public int? idReferansPartNr { get; set; }
        public string idDeliveryNote { get; set; }
        public int idHata { get; set; }
        public DateTime uretimTarihi { get; set; }
        public string lotNo { get; set; }
        public string seriNo { get; set; }
        public long KontrolAdedi { get; set; }
        public long HataAdeti { get; set; }
        public long TamirAdedi { get; set; }
        public int OperatorSayisi { get; set; }
        public long toplamSuredk { get; set; }
        public long geceklesenDkMesaGirileni { get; set; }
        public long harcananGirilenMesai { get; set; }
        //public double harcananGirilenMesai { get; set; }
        public long hataAdediReport { get; set; }
        public int idHataReport { get; set; }
        public string note { get; set; }
        public string basSaat { get; set; }
        public string bitSaat { get; set; }
        public int idClsHata { get; set; }
        public int hataCLSAdet { get; set; }
        public int mesai50Hesapla { get; set; }
        public double SaatUcreti { get; set; }
        public double LabCost { get; set; }
    }
}
