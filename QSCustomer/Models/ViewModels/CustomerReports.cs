using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.ViewModels
{
    //qprojeDetay

    public class CustomerReports
    {
        public int qprojeDetayid { get; set; }
        public int qprojeDetayidProje { get; set; }
        public string qprojetanimProjeCode { get; set; }
        public string qprojetanimProjeAcan { get; set; }
        public string qprojetanimOnayDurumu { get; set; }
        public int qprojetanimIdOprArea { get; set; }
        public string fabrikatanimFabrikaAdi { get; set; }
        public int qprojetanimIdMusteri { get; set; }
        public string musteritanimMusteriAdi { get; set; }
        public int qprojetanimIdProjeDurumu { get; set; }
        public string qprojedurumuProjeDurumu { get; set; }
        public string qprojetanimSikayetNo { get; set; }
        public DateTime qprojeDetayKontrolTarihi { get; set; }
        public long qprojeDetayTKontrolAdedi { get; set; }
        public long qprojeDetayTHataAdetiReport { get; set; }
        public long qprojeDetayTHataAdeti { get; set; }
        public long qprojeDetayTTamirAdedi { get; set; }
        public int qprojeDetayTOperatorSayisi { get; set; }
        public long qprojeDetayToplamSuredk { get; set; }
        public double qprojeDetayGerceklesenDk { get; set; }
        public double qprojeDetayGeceklesenDkMesai { get; set; }
        public double qprojeDetayGeceklesenDkMesaGirileni { get; set; }
        public double qprojeDetayHarcananGirilenMesai { get; set; }
        public double qprojeDetayVerimlilik { get; set; }
        public double qprojeDetaySaatUcreti { get; set; }
        public double qprojeDetayAnlasilanZaman { get; set; }
        public double qprojeDetayHarcananSaat { get; set; }
        public double qprojeDetayHarcananSaatMesai { get; set; }
        public bool qprojeDetayMesaiHesapla { get; set; }
        public bool qprojeDetayMesaiHesapla100 { get; set; }
        public double qprojeDetayToplamUcret { get; set; }
        public double qprojeDetayBirimYemek { get; set; }
        public double qprojeDetayBirimOpretorYemek { get; set; }
        public double qprojeDetayToplamYemek { get; set; }
        public double qprojeDetayBirimYol { get; set; }
        public double qprojeDetayBirimOpretorYol { get; set; }
        public double qprojeDetayToplamYol { get; set; }
        public int qprojeDetayToplamEkHizmekDk { get; set; }
        public double qprojeDetayToplamEkHizmet { get; set; }
        public double qprojeDetayToplam { get; set; }
        public double qprojeDetayPpm { get; set; }
        public int qprojeDetayIdProforma { get; set; }
        public int qprojetanimFiyatIdParaBirimi { get; set; }
        public string paraBirimiDoviz { get; set; }
        public double qprojetanimFiyatSaatUcreti { get; set; }
        public string ulkeUlkeAdi { get; set; }
        public double qprojeDetayEksi { get; set; }
        public double qprojeDetayArti { get; set; }
        public double qprojeDetayFix { get; set; }
        public int qprojetanimCls { get; set; }
        public int qprojetanimFiyatIdKontrolTipi { get; set; }
        public double qprojeDetayDkEkleme { get; set; }//from

        /*
 FROM qprojeDetay INNER JOIN qprojetanim ON qprojeDetay.idProje = qprojetanim.id 
            INNER JOIN fabrikatanim ON qprojetanim.idOprArea = fabrikatanim.id 
                INNER JOIN musteritanim ON qprojetanim.idMusteri = musteritanim.id 
                INNER JOIN paraBirimi ON qprojetanim.fiyatIdParaBirimi = paraBirimi.id
                INNER JOIN qprojedurumu ON qprojetanim.idProjeDurumu = qprojedurumu.id 
                INNER JOIN ulke ON fabrikatanim.idUlke = ulke.id 
                WHERE     (qprojeDetay.id > 0) AND(qprojeDetay.idProforma = 0)AND(qprojetanim.idOprArea in (2094,2086,1027,1033,2117,1076,2112,2110,1052,1036,2097,1080,1045,1029,2120,1058,1079,1040,1042,1024,1073,2118,1075,2105,2106,2092,1028,2115,2084,1054,1034,1055,1062,2083,5,2098,12,8,1032,2100,1049,19,2099,1031,1039,2089,1083,1077,2111,3,9,2121,2107,2104,1074,1050,1061,2085,18,2119,1053,1025,1048,2087,1019,1047,1065,2102,2103,1067,2091,2101,1071,2095,2093,1066,1038,11,1072,6,2096,2113,7,2108,1059,1035,2090,1021,1022,1037,1082,2109,1043,1020,1064,1057,1030,1041,1026,1023,1063,1078,16,15,14,1070,1046,10,4,1081,1068,1051,1069,1044,2116,13,1060,2114,0))AND(qprojetanim.idMusteri in (93,0))*/







    }
}
