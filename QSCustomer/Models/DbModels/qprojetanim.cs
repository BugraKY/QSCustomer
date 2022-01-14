using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.DbModels
{

    public class qprojetanim
    {
        [Key]
        public int id { get; set; }
        public int idOprArea { get; set; }
        public int idMusteri { get; set; }
        public int idProjeDurumu { get; set; }
        public string projeCode { get; set; }
        public DateTime baslangicTarihi { get; set; }//?
        public DateTime? bitisTarihi { get; set; }
        public string projeAcan { get; set; }
        public string projeAcan1 { get; set; }
        public DateTime? projeOlusturmaTarihi { get; set; }
        public DateTime? onayTarih { get; set; }
        public string onayDurumu { get; set; }
        public DateTime? onaySonTarih { get; set; }
        public double onayToplamPiece { get; set; }
        public double onayToplamDk { get; set; }
        public double onayToplamCost { get; set; }
        public string onayLastNo { get; set; }
        public int fiyatIdKontrolTipi { get; set; }
        public int fiyatIdTipi { get; set; }
        public double fiyatSaatUcreti { get; set; }//single
        public double fiyatAnlasilanZaman { get; set; }//single
        public int fiyatIdParaBirimi { get; set; }
        public double fiyatYemek { get; set; }
        public double fiyatUlasim { get; set; }
        public double fiyatOfferSaatUcreti { get; set; }
        public string sikayetNo { get; set; }
        public string note { get; set; }
        public string materyel { get; set; }
        public int cls { get; set; }

    }
}
