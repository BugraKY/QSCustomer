using Microsoft.AspNetCore.Mvc;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using QSCustomer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using QSCustomer.Controllers;
using System.Threading.Tasks;
using QSCustomer.Utility;
using System.Threading;

namespace QSCustomer.Extensions
{

    public class GetProjectDetailsExtensions
    {
        private readonly IUnitOfWork _uow;
        private readonly PdfReport _report;

        private fabrikatanim Operation;
        private musteritanim Customer;
        private paraBirimi Currency;
        private qprojedurumu ProjectStatus;
        private qprojecontroltipi ProjectControlType;
        private IEnumerable<qprojeDetay> GetProjeDetay;
        private IEnumerable<qprojehataTanimi> ProjeHataTanim;
        private IEnumerable<qprojepartNrTanimi> PartNrTanimlari;

        List<ProjectDetails> projectDetailsList = new List<ProjectDetails>();
        private List<ProjectTotalsOneByDate> projectTotalsOneByDateList = new List<ProjectTotalsOneByDate>();
        private List<qprojehataTanimi> ProjeHataTanimDynamic = new List<qprojehataTanimi>();
        private List<FaultString> FaultStringsList = new List<FaultString>();

        public GetProjectDetailsExtensions(IUnitOfWork uow, PdfReport report)
        {
            _uow = uow;
            _report = report;
        }
        public async Task<PdfReport> GetStatusOpen(string startDate, string endDate)
        {
            Console.WriteLine("Section 4");

            #region Queries
            Operation = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == _report._SelectedProject.idOprArea);
            Customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == _report._SelectedProject.idMusteri);
            Currency = _uow.ParaBirimi.GetFirstOrDefault(i => i.id == _report._SelectedProject.fiyatIdParaBirimi);
            ProjectStatus = _uow.ProjeDurumu.GetFirstOrDefault(i => i.id == _report._SelectedProject.idProjeDurumu);
            ProjectControlType = _uow.ProjeKontrolTipi.GetFirstOrDefault(i => i.id == _report._SelectedProject.fiyatIdKontrolTipi);
            GetProjeDetay = _uow.ProjeDetay.GetAll(i => i.idProje == _report._SelectedProject.id);
            ProjeHataTanim = _uow.ProjeHataTanimi.GetAll(i => i.idProje == _report._SelectedProject.id);
            PartNrTanimlari = _uow.ProjePartNrTanimi.GetAll(i => i.idProje == _report._SelectedProject.id);
            #endregion Queries
            Console.WriteLine("Section 5");
            qprojetanim SelectedProject = new qprojetanim();
            ProjectVariables.LengthProgress = 0;

            var _SelectedProject = _uow.ProjeTanim.GetAll(i => i.projeCode == _report._ProjectCode);

            int s = 0;
            int say = 0;

            double Checked = 0;
            double Reworked = 0;
            double Nok = 0;
            double SpentHours = 0;
            int Overtime50 = 0;
            double Overtime100 = 0;
            double SpentHr = 0;
            double PPM = 0;
            int _ProjectDetailsId = 0;
            int convertedPpm = 0;
            double Test = 0;

            int ProjectDetCount = 0;

            string DataFields = "{ dataField: 'id', caption:'id' },{ dataField: 'kontrolTarihi', caption: 'Control Date' },{ dataField: 'uretimTarihi', caption: 'Product Date' },{ dataField: 'partNrTanimi', caption: 'Ürün Referansları Prod' },{ dataField: 'iotNo', caption: 'lotNo' },{ dataField: 'seriNo', caption: 'Seri No Serial Number' },{ dataField: 'harcanansaat', caption: 'Harcanan Süre Spent' },{ dataField: 'mesai50Hesapla', caption: '%50 Fazla Mesai' },{ dataField: 'harcanangirilenmesai', caption: '%100 Fazla Mesai' },{ dataField: 'kontrolAdedi', caption: 'Kontrol Adet Checked' },{ dataField: 'tamirAdedi', caption: 'Rötüş Adet Reworker' },{ dataField: 'hataAdeti', caption: 'Hatalı Adet Nok' },";

            int projedetay_say = 0;
            Console.WriteLine("Section 6");
            await Task.Run(() =>
            {

                #region Main Code
                Console.WriteLine("Section 7");
                foreach (var item in _SelectedProject)
                {
                    Console.WriteLine("Section GetProjectDetExt. Foreach Loop");
                    if (item.idProjeDurumu == 1)
                        SelectedProject = item;
                }

                var FilteredProjectDetails = _uow.ProjeDetay.GetAll(i => i.idProje == SelectedProject.id).Where(d => d.kontrolTarihi >= DateTime.Parse(startDate)).Where(d => d.kontrolTarihi <= DateTime.Parse(endDate));

                Console.WriteLine("Section 8");
                Console.WriteLine("Status Open/");
                Console.WriteLine("Process Begining..");


                foreach (var ProjeDetayItem in FilteredProjectDetails)
                {
                    var GetProjeDetaysLength = _uow.ProjeDetays.GetAll(i => i.idProjeDetay == ProjeDetayItem.id).Count();
                    if (ProjeDetayItem.idProforma == 0)
                    {
                        ProjectDetCount += GetProjeDetaysLength;
                    }
                }
                ProjectVariables.LengthProgress = ProjectDetCount;

                foreach (var ProjeDetayItem in FilteredProjectDetails)
                {


                    var GetProjeDetays = _uow.ProjeDetays.GetAll(i => i.idProjeDetay == ProjeDetayItem.id);
                    say += GetProjeDetays.Count();
                    if (ProjeDetayItem.idProforma == 0)
                    {
                        PPM += ProjeDetayItem.ppm;
                        Overtime100 += ProjeDetayItem.harcananGirilenMesai;
                        projedetay_say++;
                        var harcanansaat = ProjeDetayItem.harcananSaat;//ProjeDetaysItem ın içerisinde birkere yazılacak
                        var harcanangirilenmesai = ProjeDetayItem.harcananGirilenMesai;//ProjeDetaysItem ın içerisinde birkere yazılacak
                        foreach (var ProjeDetaysItem in GetProjeDetays)
                        {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
                            qprojepartNrTanimi? getPartNrItem = _uow.ProjePartNrTanimi.GetFirstOrDefault(i => i.id == ProjeDetaysItem.idReferansPartNr);
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
                            List<qprojeHataDetay?> getHataDetay = (List<qprojeHataDetay?>)_uow.ProjeHataDetay.GetAll(i => i.idProjeDetays == ProjeDetaysItem.id).OrderBy(x => x.idHataTanimi).ToList();

                            if ((ProjeDetaysItem.toplamSuredk == 0) && (ProjeDetaysItem.idReferansPartNr == 0) && (ProjeDetaysItem.KontrolAdedi == 0) && (ProjeDetaysItem.SaatUcreti == 0))
                            {

                                _ProjectDetailsId++;
                                var _projectDetails = new ProjectDetails
                                {
                                    Id = _ProjectDetailsId,
                                    KontrolTarihi = ProjeDetayItem.kontrolTarihi.ToString("dd/MM/yyyy"),
                                    UretimTarihi = ProjeDetaysItem.uretimTarihi.ToString("dd/MM/yyyy"),
                                    PartNrTanimi = "",
                                    IotNo = "",
                                    SeriNo = "",
                                    Harcanansaat = 0,
                                    Mesai50Hesapla = 0,
                                    Harcanangirilenmesai = 0,
                                    KontrolAdedi = 0,
                                    TamirAdedi = 0,
                                    HataAdeti = 0,
                                };
                                projectDetailsList.Add(_projectDetails);
                                /*
                                #region TEST
                                var HatalarCount = 0;
                                #region HataTanimsDuzen
                                foreach (var itemm in ProjeHataTanim)
                                {

                                    qprojeHataDetay item = new qprojeHataDetay();


                                    if (HatalarCount < getHataDetay.Count)
                                    {
                                        item = getHataDetay[HatalarCount];
                                    }

                                    if (item.idHataTanimi == itemm.id)
                                    {
                                        var _FaultStr = _uow.ProjeHataTanimi.GetFirstOrDefault(i => i.id == item.idHataTanimi);
                                        var _projectFaults = new FaultString
                                        {
                                            id = item.id,
                                            idProje = item.idProje,
                                            idProjeDetay = item.idProjeDetay,
                                            idProjeDetays = item.idProjeDetays,
                                            idHataTanimi = item.idHataTanimi,
                                            Adet = item.Adet,
                                            hataTanimi = _FaultStr.hataTanimi,
                                        };
                                        FaultStringsList.Add(_projectFaults);
                                        HatalarCount++;
                                    }

                                    else
                                        FaultStringsList.Add(null);
                                }
                                #endregion HataTanimsDuzen

                                _ProjectDetailsId++;
                                if (getPartNrItem == null)
                                {
                                    getPartNrItem.idProje = ProjeDetaysItem.id;
                                    getPartNrItem.partNrTanimi = " ";
                                }


                                if (ProjeDetayItem.mesaiHesapla100)
                                    ProjeDetaysItem.harcananGirilenMesai = Convert.ToInt64(harcanansaat);
                                var _projectDetails = new ProjectDetails
                                {
                                    Id = _ProjectDetailsId,
                                    KontrolTarihi = ProjeDetayItem.kontrolTarihi.ToString("dd/MM/yyyy"),
                                    UretimTarihi = ProjeDetaysItem.uretimTarihi.ToString("dd/MM/yyyy"),
                                    PartNrTanimi = getPartNrItem.partNrTanimi,
                                    IotNo = ProjeDetaysItem.lotNo,
                                    SeriNo = ProjeDetaysItem.seriNo,
                                    Harcanansaat = harcanansaat,
                                    Mesai50Hesapla = ProjeDetaysItem.mesai50Hesapla,
                                    Harcanangirilenmesai = ProjeDetaysItem.harcananGirilenMesai,
                                    KontrolAdedi = ProjeDetaysItem.KontrolAdedi,
                                    TamirAdedi = ProjeDetaysItem.TamirAdedi,
                                    HataAdeti = ProjeDetaysItem.HataAdeti,
                                    Faults = FaultStringsList.ToList()

                                };
                                FaultStringsList.Clear();
                                projectDetailsList.Add(_projectDetails);
                                Checked += _projectDetails.KontrolAdedi;
                                Reworked += _projectDetails.TamirAdedi;
                                Nok += _projectDetails.HataAdeti;
                                SpentHours += _projectDetails.Harcanansaat;
                                Overtime50 += _projectDetails.Mesai50Hesapla;
                                SpentHr += _projectDetails.Harcanangirilenmesai;


    #endregion TEST
                                */
                            }
                            else
                            {
                                var HatalarCount = 0;
                                #region HataTanimsDuzen
                                foreach (var itemm in ProjeHataTanim)
                                {

                                    qprojeHataDetay item = new qprojeHataDetay();


                                    if (HatalarCount < getHataDetay.Count)
                                    {
                                        item = getHataDetay[HatalarCount];
                                    }

                                    if (item.idHataTanimi == itemm.id)
                                    {
                                        var _FaultStr = _uow.ProjeHataTanimi.GetFirstOrDefault(i => i.id == item.idHataTanimi);
                                        var _projectFaults = new FaultString
                                        {
                                            id = item.id,
                                            idProje = item.idProje,
                                            idProjeDetay = item.idProjeDetay,
                                            idProjeDetays = item.idProjeDetays,
                                            idHataTanimi = item.idHataTanimi,
                                            Adet = item.Adet,
                                            hataTanimi = _FaultStr.hataTanimi,
                                        };
                                        FaultStringsList.Add(_projectFaults);
                                        HatalarCount++;
                                    }

                                    else
                                        FaultStringsList.Add(null);
                                }
                                #endregion HataTanimsDuzen

                                _ProjectDetailsId++;

                                if (ProjeDetayItem.mesaiHesapla100)
                                    ProjeDetaysItem.harcananGirilenMesai = Convert.ToInt64(harcanansaat);
                                var _projectDetails = new ProjectDetails
                                {
                                    Id = _ProjectDetailsId,
                                    KontrolTarihi = ProjeDetayItem.kontrolTarihi.ToString("dd/MM/yyyy"),
                                    UretimTarihi = ProjeDetaysItem.uretimTarihi.ToString("dd/MM/yyyy"),
                                    PartNrTanimi = getPartNrItem.partNrTanimi,
                                    IotNo = ProjeDetaysItem.lotNo,
                                    SeriNo = ProjeDetaysItem.seriNo,
                                    Harcanansaat = harcanansaat,
                                    Mesai50Hesapla = ProjeDetaysItem.mesai50Hesapla,
                                    Harcanangirilenmesai = ProjeDetaysItem.harcananGirilenMesai,
                                    KontrolAdedi = ProjeDetaysItem.KontrolAdedi,
                                    TamirAdedi = ProjeDetaysItem.TamirAdedi,
                                    HataAdeti = ProjeDetaysItem.HataAdeti,
                                    Faults = FaultStringsList.ToList()

                                };
                                FaultStringsList.Clear();
                                projectDetailsList.Add(_projectDetails);
                                Checked += _projectDetails.KontrolAdedi;
                                Reworked += _projectDetails.TamirAdedi;
                                Nok += _projectDetails.HataAdeti;
                                SpentHours += _projectDetails.Harcanansaat;
                                Overtime50 += _projectDetails.Mesai50Hesapla;
                                SpentHr += _projectDetails.Harcanangirilenmesai;
                            }

                            harcanansaat = 0;//foreach döngüsünde bir kere yazılması gerektiği için sıfırlıyoruz.
                            harcanangirilenmesai = 0;//foreach döngüsünde bir kere yazılması gerektiği için sıfırlıyoruz.
                            s++;
                            Console.WriteLine("Processing.. " + s.ToString());
                            ProjectVariables.CountProgress = s;
                        }
                    }

                }

                double ppm = (Nok / Checked) * 1000000;
                if (!Double.IsNaN(ppm))
                    convertedPpm = Convert.ToInt32(ppm);


                string cdateString = "";
                if (projectDetailsList.Count() > 0)
                    cdateString = projectDetailsList[0].KontrolTarihi;


                long cVal = 0, eVal = 0;
                int ptd_count = 0;
                foreach (var item in projectDetailsList)
                {
                    if (ptd_count == (projectDetailsList.Count() - 1))
                    {
                        cVal += item.KontrolAdedi;
                        eVal += item.HataAdeti;
                        var projectTotalsOneByDate = new ProjectTotalsOneByDate
                        {
                            kontrolTarihi = cdateString,
                            kontrolAdedi = cVal,
                            hataAdeti = eVal
                        };
                        projectTotalsOneByDateList.Add(projectTotalsOneByDate);
                    }
                    else
                    {
                        if (item.KontrolTarihi != cdateString)
                        {
                            var projectTotalsOneByDate = new ProjectTotalsOneByDate
                            {
                                kontrolTarihi = cdateString,
                                kontrolAdedi = cVal,
                                hataAdeti = eVal
                            };
                            projectTotalsOneByDateList.Add(projectTotalsOneByDate);
                            cdateString = item.KontrolTarihi;
                            cVal = 0;
                            eVal = 0;
                            cVal += item.KontrolAdedi;
                            eVal += item.HataAdeti;
                        }
                        else
                        {
                            cVal += item.KontrolAdedi;
                            eVal += item.HataAdeti;
                        }
                    }

                    ptd_count++;
                }




            });

            var PdfReport = new PdfReport()
            {
                _SelectedProject = SelectedProject,
                _Customer = Customer,
                _Operation = Operation,
                _PartNrTanimlari = PartNrTanimlari.ToList(),
                _ProjeHataTanim = ProjeHataTanim.ToList(),
                _ProjeHataTanimCount = ProjeHataTanim.Count(),
                _ProjectDetails = projectDetailsList.ToList(),
                _ProjectTotalsOnebyDate = projectTotalsOneByDateList,
                _CheckedTotal = Checked,
                _ReworkedTotal = Reworked,
                _NokTotal = Nok,
                _SpentHoursTotal = SpentHours,
                _SpentHours = (SpentHours - Overtime100),
                _OverTime50Total = Overtime50,
                _OverTime100Total = Overtime100,
                _PPMTotal = convertedPpm,
                _DataFields = DataFields
            };

            var deg = s;
            var deg_2 = projedetay_say;
            ProjeHataTanimDynamic.AddRange(ProjeHataTanim);
            //return Json(projectDetailsList.ToList());
            var _aaaaaaa = Test;



            Console.WriteLine("\n Status Open Done.");
            #endregion MainCode

            return PdfReport;


            /*

            int s = 0;
            int say = 0;
            int projedetay_say = 0;
            foreach (var ProjeDetayItem in GetProjeDetay)
            {

                var GetProjeDetays = _uow.ProjeDetays.GetAll(i => i.idProjeDetay == ProjeDetayItem.id);
                say += GetProjeDetays.Count();
                if (ProjeDetayItem.idProforma == 0)
                {
                    projedetay_say++;
                    var harcanansaat = ProjeDetayItem.harcananSaat;//ProjeDetaysItem ın içerisinde birkere yazılacak
                    var harcanangirilenmesai = ProjeDetayItem.harcananGirilenMesai;//ProjeDetaysItem ın içerisinde birkere yazılacak
                    foreach (var ProjeDetaysItem in GetProjeDetays)
                    {
                        var getPartNrItem = _uow.ProjePartNrTanimi.GetFirstOrDefault(i => i.id == ProjeDetaysItem.idReferansPartNr);
                        var getHataDetay = _uow.ProjeHataDetay.GetFirstOrDefault(i => i.idProjeDetays == ProjeDetaysItem.id);



                        harcanansaat = 0;//foreach döngüsünde bir kere yazılması gerektiği için sıfırlıyoruz.
                        harcanangirilenmesai = 0;//foreach döngüsünde bir kere yazılması gerektiği için sıfırlıyoruz.
                        s++;
                    }
                }
            }

            var deg = s;
            var deg_2 = projedetay_say;
            _report._PPMTotal = 9999;
            _report._OverTime50Total = 50000;
            _report._PPMTotal = 5400;

            */
            return null;
        }
        
        public PdfReport GetStatusClose()
        {
            #region Queries
            Operation = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == _report._SelectedProject.idOprArea);
            Customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == _report._SelectedProject.idMusteri);
            Currency = _uow.ParaBirimi.GetFirstOrDefault(i => i.id == _report._SelectedProject.fiyatIdParaBirimi);
            ProjectStatus = _uow.ProjeDurumu.GetFirstOrDefault(i => i.id == _report._SelectedProject.idProjeDurumu);
            ProjectControlType = _uow.ProjeKontrolTipi.GetFirstOrDefault(i => i.id == _report._SelectedProject.fiyatIdKontrolTipi);
            GetProjeDetay = _uow.ProjeDetay.GetAll(i => i.idProje == _report._SelectedProject.id);
            ProjeHataTanim = _uow.ProjeHataTanimi.GetAll(i => i.idProje == _report._SelectedProject.id);
            PartNrTanimlari = _uow.ProjePartNrTanimi.GetAll(i => i.idProje == _report._SelectedProject.id);
            #endregion Queries


            qprojetanim SelectedProject = new qprojetanim();
            var _SelectedProject = _uow.ProjeTanim.GetAll(i => i.projeCode == _report._ProjectCode);
            foreach (var item in _SelectedProject)
            {
                if (item.idProjeDurumu == 1)
                    SelectedProject = item;
            }


            int s = 0;
            int say = 0;

            double Checked = 0;
            double Reworked = 0;
            double Nok = 0;
            double SpentHours = 0;
            int Overtime50 = 0;
            double Overtime100 = 0;
            double SpentHr = 0;
            double PPM = 0;
            int _ProjectDetailsId = 0;

            double Test = 0;

            string DataFields = "{ dataField: 'id', caption:'id' },{ dataField: 'kontrolTarihi', caption: 'Control Date' },{ dataField: 'uretimTarihi', caption: 'Product Date' },{ dataField: 'partNrTanimi', caption: 'Ürün Referansları Prod' },{ dataField: 'iotNo', caption: 'lotNo' },{ dataField: 'seriNo', caption: 'Seri No Serial Number' },{ dataField: 'harcanansaat', caption: 'Harcanan Süre Spent' },{ dataField: 'mesai50Hesapla', caption: '%50 Fazla Mesai' },{ dataField: 'harcanangirilenmesai', caption: '%100 Fazla Mesai' },{ dataField: 'kontrolAdedi', caption: 'Kontrol Adet Checked' },{ dataField: 'tamirAdedi', caption: 'Rötüş Adet Reworker' },{ dataField: 'hataAdeti', caption: 'Hatalı Adet Nok' },";

            int projedetay_say = 0;
            Console.WriteLine("\n Status Close/");
            Console.WriteLine("Process Begining..");
            foreach (var ProjeDetayItem in GetProjeDetay)
            {


                var GetProjeDetays = _uow.ProjeDetays.GetAll(i => i.idProjeDetay == ProjeDetayItem.id);
                say += GetProjeDetays.Count();
                if (ProjeDetayItem.idProforma != 0)
                {
                    PPM += ProjeDetayItem.ppm;
                    Overtime100 += ProjeDetayItem.harcananGirilenMesai;
                    projedetay_say++;
                    var harcanansaat = ProjeDetayItem.harcananSaat;//ProjeDetaysItem ın içerisinde birkere yazılacak
                    var harcanangirilenmesai = ProjeDetayItem.harcananGirilenMesai;//ProjeDetaysItem ın içerisinde birkere yazılacak
                    foreach (var ProjeDetaysItem in GetProjeDetays)
                    {
                        #pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
                        qprojepartNrTanimi? getPartNrItem = _uow.ProjePartNrTanimi.GetFirstOrDefault(i => i.id == ProjeDetaysItem.idReferansPartNr);
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
                        List<qprojeHataDetay?> getHataDetay = (List<qprojeHataDetay?>)_uow.ProjeHataDetay.GetAll(i => i.idProjeDetays == ProjeDetaysItem.id).OrderBy(x=>x.idHataTanimi).ToList();

                        if ((ProjeDetaysItem.toplamSuredk == 0) && (ProjeDetaysItem.idReferansPartNr == 0) && (ProjeDetaysItem.KontrolAdedi == 0) && (ProjeDetaysItem.SaatUcreti == 0))
                        {

                            _ProjectDetailsId++;
                            var _projectDetails = new ProjectDetails
                            {
                                Id = _ProjectDetailsId,
                                KontrolTarihi = ProjeDetayItem.kontrolTarihi.ToString("dd/MM/yyyy"),
                                UretimTarihi = ProjeDetaysItem.uretimTarihi.ToString("dd/MM/yyyy"),
                                PartNrTanimi = "",
                                IotNo = "",
                                SeriNo = "",
                                Harcanansaat = 0,
                                Mesai50Hesapla = 0,
                                Harcanangirilenmesai = 0,
                                KontrolAdedi = 0,
                                TamirAdedi = 0,
                                HataAdeti = 0,
                            };
                            projectDetailsList.Add(_projectDetails);
                        }
                        else
                        {
                            var HatalarCount = 0;
                            #region HataTanimsDuzen
                            foreach (var itemm in ProjeHataTanim)
                            {

                                qprojeHataDetay item = new qprojeHataDetay();


                                if (HatalarCount < getHataDetay.Count)
                                {
                                    item = getHataDetay[HatalarCount];
                                }

                                if (item.idHataTanimi == itemm.id)
                                {
                                    var _FaultStr = _uow.ProjeHataTanimi.GetFirstOrDefault(i => i.id == item.idHataTanimi);
                                    var _projectFaults = new FaultString
                                    {
                                        id = item.id,
                                        idProje = item.idProje,
                                        idProjeDetay = item.idProjeDetay,
                                        idProjeDetays = item.idProjeDetays,
                                        idHataTanimi = item.idHataTanimi,
                                        Adet = item.Adet,
                                        hataTanimi = _FaultStr.hataTanimi,
                                    };
                                    FaultStringsList.Add(_projectFaults);
                                    HatalarCount++;
                                }
                                
                                else
                                    FaultStringsList.Add(null);
                            }
                            #endregion HataTanimsDuzen

                            _ProjectDetailsId++;

                            if (ProjeDetayItem.mesaiHesapla100)
                                ProjeDetaysItem.harcananGirilenMesai = Convert.ToInt64(harcanansaat);
                            var _projectDetails = new ProjectDetails
                            {
                                Id = _ProjectDetailsId,
                                KontrolTarihi = ProjeDetayItem.kontrolTarihi.ToString("dd/MM/yyyy"),
                                UretimTarihi = ProjeDetaysItem.uretimTarihi.ToString("dd/MM/yyyy"),
                                PartNrTanimi = getPartNrItem.partNrTanimi,
                                IotNo = ProjeDetaysItem.lotNo,
                                SeriNo = ProjeDetaysItem.seriNo,
                                Harcanansaat = harcanansaat,
                                Mesai50Hesapla = ProjeDetaysItem.mesai50Hesapla,
                                Harcanangirilenmesai = ProjeDetaysItem.harcananGirilenMesai,
                                KontrolAdedi = ProjeDetaysItem.KontrolAdedi,
                                TamirAdedi = ProjeDetaysItem.TamirAdedi,
                                HataAdeti = ProjeDetaysItem.HataAdeti,
                                Faults = FaultStringsList.ToList()

                            };
                            FaultStringsList.Clear();
                            projectDetailsList.Add(_projectDetails);
                            Checked += _projectDetails.KontrolAdedi;
                            Reworked += _projectDetails.TamirAdedi;
                            Nok += _projectDetails.HataAdeti;
                            SpentHours += _projectDetails.Harcanansaat;
                            Overtime50 += _projectDetails.Mesai50Hesapla;
                            SpentHr += _projectDetails.Harcanangirilenmesai;
                        }

                        harcanansaat = 0;//foreach döngüsünde bir kere yazılması gerektiği için sıfırlıyoruz.
                        harcanangirilenmesai = 0;//foreach döngüsünde bir kere yazılması gerektiği için sıfırlıyoruz.
                        s++;
                        Console.WriteLine("Processing.. " + s.ToString());
                    }
                }

            }
            int convertedPpm = 0;
            double ppm = (Nok / Checked) * 1000000;
            if (!Double.IsNaN(ppm))
                convertedPpm = Convert.ToInt32(ppm);


            string cdateString = "";
            if (projectDetailsList.Count() > 0)
                cdateString = projectDetailsList[0].KontrolTarihi;


            long cVal = 0, eVal = 0;
            int ptd_count = 0;
            foreach (var item in projectDetailsList)
            {
                if (ptd_count == (projectDetailsList.Count() - 1))
                {
                    cVal += item.KontrolAdedi;
                    eVal += item.HataAdeti;
                    var projectTotalsOneByDate = new ProjectTotalsOneByDate
                    {
                        kontrolTarihi = cdateString,
                        kontrolAdedi = cVal,
                        hataAdeti = eVal
                    };
                    projectTotalsOneByDateList.Add(projectTotalsOneByDate);
                }
                else
                {
                    if (item.KontrolTarihi != cdateString)
                    {
                        var projectTotalsOneByDate = new ProjectTotalsOneByDate
                        {
                            kontrolTarihi = cdateString,
                            kontrolAdedi = cVal,
                            hataAdeti = eVal
                        };
                        projectTotalsOneByDateList.Add(projectTotalsOneByDate);
                        cdateString = item.KontrolTarihi;
                        cVal = 0;
                        eVal = 0;
                        cVal += item.KontrolAdedi;
                        eVal += item.HataAdeti;
                    }
                    else
                    {
                        cVal += item.KontrolAdedi;
                        eVal += item.HataAdeti;
                    }
                }

                ptd_count++;
            }



            var PdfReport = new PdfReport()
            {
                _SelectedProject = SelectedProject,
                _Customer = Customer,
                _Operation = Operation,
                _PartNrTanimlari = PartNrTanimlari.ToList(),
                _ProjeHataTanim = ProjeHataTanim.ToList(),
                _ProjeHataTanimCount = ProjeHataTanim.Count(),
                _ProjectDetails = projectDetailsList.ToList(),
                _ProjectTotalsOnebyDate = projectTotalsOneByDateList,
                _CheckedTotal = Checked,
                _ReworkedTotal = Reworked,
                _NokTotal = Nok,
                _SpentHoursTotal = SpentHours,
                _SpentHours = (SpentHours - Overtime100),
                _OverTime50Total = Overtime50,
                _OverTime100Total = Overtime100,
                _PPMTotal = convertedPpm,
                _DataFields = DataFields
            };

            var deg = s;
            var deg_2 = projedetay_say;
            ProjeHataTanimDynamic.AddRange(ProjeHataTanim);
            //return Json(projectDetailsList.ToList());
            var _aaaaaaa = Test;



            Console.WriteLine("\n Status Close Done.");

            return PdfReport;
        }

        /*
        private ActionResult ProgressBar(int min,int max,int val)
        {
            string _Valval="test";

            ReportsController controller;
            controller.ViewBag.deneme = "asds";
            return ViewComponent("fasdg");
        }*/
    }
}
