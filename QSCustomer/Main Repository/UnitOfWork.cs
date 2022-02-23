using QSCustomer.Data;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SecondDbContext _dbSec;
        private readonly ApplicationDbContext _dbApp;
        public UnitOfWork(SecondDbContext dbSec, ApplicationDbContext dbApp)
        {
            _dbSec = dbSec;
            _dbApp = dbApp;
            ApplicationUser = new ApplicationUserRepository(_dbApp);
            UserType = new UserTypeRepository(_dbApp);

            MusteriYetkili = new MusteriYetkiliRepository(_dbSec);
            MusteriTanim = new MusteriTanimRepository(_dbSec);
            ProjeTanim = new ProjeTanimRepository(_dbSec);
            ProjeDetay = new ProjeDetayRepository(_dbSec);
            ProjeDetays = new ProjeDetaysRepository(_dbSec);
            FabrikaTanim = new FabrikaTanimRepository(_dbSec);
            FabrikaTanimYetkili = new FabrikaTanimYetkiliRepository(_dbSec);
            ProjeDurumu = new ProjeDurumuRepository(_dbSec);
            ProjeKontrolTipi = new ProjeKontrolTipiRepository(_dbSec);
            ProjePartNrTanimi = new ProjePartNrTanimiRepository(_dbSec);
            ProjeHataTanimi = new ProjeHataTanimiRepository(_dbSec);
            ProjeHataDetay = new ProjeHataDetayRepository(_dbSec);
            ParaBirimi = new ParaBirimiRepository(_dbSec);
            Ulke = new UlkeRepository(_dbSec);
        }
        /*
        public UnitOfWork()
        {

        }*/
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IMusteriYetkiliRepository MusteriYetkili { get; private set; }
        public IMusteriTanimRepository MusteriTanim { get; private set; }
        public IProjeTanimRepository ProjeTanim { get; private set; }
        public IProjeDetayRepository ProjeDetay { get; private set; }
        public IProjeDetaysRepository ProjeDetays { get; private set; }
        public IFabrikaTanimRepository FabrikaTanim { get; private set; }
        public IFabrikaTanimYetkiliRepository FabrikaTanimYetkili { get; private set; }
        public IProjeDurumuRepository ProjeDurumu { get; private set; }
        public IProjeKontrolTipiRepository ProjeKontrolTipi { get; private set; }
        public IProjePartNrTanimiRepository ProjePartNrTanimi { get; private set; }
        public IProjeHataTanimiRepository ProjeHataTanimi { get; private set; }
        public IProjeHataDetayRepository ProjeHataDetay { get; private set; }
        public IParaBirimiRepository ParaBirimi { get; private set; }
        public IUlkeRepository Ulke { get; private set; }
        public IUserTypeRepository UserType { get; private set; }
        public void Dispose()
        {
           //if (_dbSec != null)
                _dbSec.Dispose();
            //else
                _dbApp.Dispose();

        }
        public void Save()
        {
            //if (_dbSec != null)
                _dbSec.SaveChanges();
            //else
                _dbApp.SaveChanges();
        }
    }
}
