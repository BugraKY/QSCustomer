using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.DbModels
{
    public class qprojeDetay
    {
        [Key]
        public int id { get; set; }
        public int idProje { get; set; }
        public string referansPartNr { get; set; }
        public string deliveryNote { get; set; }
        public string hatalar { get; set; }
        public string hatalarReport { get; set; }
        public string lotNo { get; set; }
        public string seriNo { get; set; }
        public int idProjeDetayDurumu { get; set; }
        public DateTime kontrolTarihi { get; set; }
        public long tKontrolAdedi { get; set; }
        public long tHataAdetiReport { get; set; }
        public long tHataAdeti { get; set; }
        public long tTamirAdedi { get; set; }
        public int tOperatorSayisi { get; set; }
        public long toplamSuredk { get; set; }
        public double gerceklesenDk { get; set; }
        public double geceklesenDkMesai { get; set; }
        public double geceklesenDkMesaGirileni { get; set; }
        public double verimlilik { get; set; }
        public double birimZamandk { get; set; }
        public double saatUcreti { get; set; }
        public double anlasilanZaman { get; set; }
        public double harcananSaat { get; set; }
        public double harcananSaatMesai { get; set; }
        public double harcananGirilenMesai { get; set; }
        public bool mesaiHesapla { get; set; }
        public bool mesaiHesapla100 { get; set; }
        public double toplamUcret { get; set; }
        public double birimYemek { get; set; }
        public double birimOpretorYemek { get; set; }
        public double toplamYemek { get; set; }
        public double birimYol { get; set; }
        public double birimOpretorYol { get; set; }
        public double toplamYol { get; set; }
        public int toplamEkHizmekDk { get; set; }
        public double toplamEkHizmet { get; set; }
        public double toplam { get; set; }
        public double ppm { get; set; }
        public int idProforma { get; set; }
        public DateTime kayitTarihi { get; set; }
        public double arti { get; set; }
        public double eksi { get; set; }
        public double fix { get; set; }
        public string tidClsHata { get; set; }
        public int thataCLSAdet { get; set; }
        public double dkEkleme { get; set; }
    }
}
