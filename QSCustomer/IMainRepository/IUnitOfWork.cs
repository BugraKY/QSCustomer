using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.IMainRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        IMusteriYetkiliRepository MusteriYetkili { get; }
        IFabrikaTanimYetkiliRepository FabrikaTanimYetkili { get; }
        IMusteriTanimRepository MusteriTanim { get; }
        IProjeTanimRepository ProjeTanim { get; }
        IProjeDetayRepository ProjeDetay { get; }
        IProjeDetaysRepository ProjeDetays { get; }
        IFabrikaTanimRepository FabrikaTanim { get; }
        IProjeDurumuRepository ProjeDurumu { get; }
        IProjeKontrolTipiRepository ProjeKontrolTipi { get; }
        IProjePartNrTanimiRepository ProjePartNrTanimi { get; }
        IProjeHataTanimiRepository ProjeHataTanimi { get; }
        IProjeHataDetayRepository ProjeHataDetay { get; }
        IParaBirimiRepository ParaBirimi { get; }
        IUlkeRepository Ulke { get; }
        IUserTypeRepository UserType { get; }
        void Save();
    }
}
