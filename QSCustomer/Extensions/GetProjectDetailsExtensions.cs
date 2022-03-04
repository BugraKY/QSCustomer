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
using Microsoft.AspNetCore.SignalR;
using QSCustomer.Hubs;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace QSCustomer.Extensions
{

    public class GetProjectDetailsExtensions
    {
        private readonly IUnitOfWork _uow;
        private readonly PdfReport _report;
        private readonly Claim _claim;
        private readonly IHttpContextAccessor _accessor;

        protected IHubContext<HomeHub> _context;
        public WebSocketActionExtensions WebSocket;
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

        public GetProjectDetailsExtensions(IUnitOfWork uow, PdfReport report, Claim claim, IHubContext<HomeHub> context, IHttpContextAccessor accessor)
        {
            _uow = uow;
            _report = report;
            _claim = claim;
            _context = context;
            _accessor = accessor;
        }
        public async Task<PdfReport> GetStatusOpen(string startDate, string endDate)
        {
            WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);

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
            qprojepartNrTanimi getPartNrItem = null;

            var _SelectedProject = _uow.ProjeTanim.GetAll(i => i.projeCode == _report._ProjectCode);

            int ProjectDetCount = 0;
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
            int ProjectDetLength = 0;
            string DataFields = "{ dataField: 'id', caption:'id' },{ dataField: 'kontrolTarihi', caption: 'Control Date' },{ dataField: 'uretimTarihi', caption: 'Product Date' },{ dataField: 'partNrTanimi', caption: 'Ürün Referansları Prod' },{ dataField: 'iotNo', caption: 'lotNo' },{ dataField: 'seriNo', caption: 'Seri No Serial Number' },{ dataField: 'harcanansaat', caption: 'Harcanan Süre Spent' },{ dataField: 'mesai50Hesapla', caption: '%50 Fazla Mesai' },{ dataField: 'harcanangirilenmesai', caption: '%100 Fazla Mesai' },{ dataField: 'kontrolAdedi', caption: 'Kontrol Adet Checked' },{ dataField: 'tamirAdedi', caption: 'Rötüş Adet Reworker' },{ dataField: 'hataAdeti', caption: 'Hatalı Adet Nok' },";
            await Task.Run(async () =>
            {
                #region Main Code
                foreach (var item in _SelectedProject)
                {
                    if (item.idProjeDurumu == 1)
                        SelectedProject = item;
                }

                var FilteredProjectDetails = _uow.ProjeDetay.GetAll(i => i.idProje == SelectedProject.id).Where(d => d.kontrolTarihi >= DateTime.Parse(startDate)).Where(d => d.kontrolTarihi <= DateTime.Parse(endDate));

                foreach (var ProjeDetayItem in FilteredProjectDetails)
                {
                    var GetProjeDetaysLength = _uow.ProjeDetays.GetAll(i => i.idProjeDetay == ProjeDetayItem.id).Count();
                    if (ProjeDetayItem.idProforma == 0)
                    {
                        ProjectDetLength += GetProjeDetaysLength;
                    }
                }
                Console.WriteLine("\n Status Open Begining.");
                foreach (var ProjeDetayItem in FilteredProjectDetails)
                {
                    var GetProjeDetays = _uow.ProjeDetays.GetAll(i => i.idProjeDetay == ProjeDetayItem.id);
                    say += GetProjeDetays.Count();
                    if (ProjeDetayItem.idProforma == 0)
                    {
                        PPM += ProjeDetayItem.ppm;
                        Overtime100 += ProjeDetayItem.harcananGirilenMesai;
                        var harcanansaat = ProjeDetayItem.harcananSaat;//ProjeDetaysItem ın içerisinde birkere yazılacak
                        var harcanangirilenmesai = ProjeDetayItem.harcananGirilenMesai;//ProjeDetaysItem ın içerisinde birkere yazılacak
                        foreach (var ProjeDetaysItem in GetProjeDetays)
                        {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
                            getPartNrItem = _uow.ProjePartNrTanimi.GetFirstOrDefault(i => i.id == ProjeDetaysItem.idReferansPartNr);
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

                                if (getPartNrItem == null)
                                {
                                    var _projectDetails = new ProjectDetails
                                    {
                                        Id = _ProjectDetailsId,
                                        KontrolTarihi = ProjeDetayItem.kontrolTarihi.ToString("dd/MM/yyyy"),
                                        UretimTarihi = ProjeDetaysItem.uretimTarihi.ToString("dd/MM/yyyy"),
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
                                else
                                {
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

                            }

                            harcanansaat = 0;//foreach döngüsünde bir kere yazılması gerektiği için sıfırlıyoruz.
                            harcanangirilenmesai = 0;//foreach döngüsünde bir kere yazılması gerektiği için sıfırlıyoruz.
                            ProjectDetCount++;
                            await WebSocAct.ProgressBar_WebSocket(_claim, Convert.ToDouble(ProjectDetCount), Convert.ToDouble(ProjectDetLength), _accessor.HttpContext.Request.Cookies["progress_id"]);
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
            var _projectFilter = new ProjectFilter()
            {
                StartDate = DateTime.Parse(startDate),
                FinishDate = DateTime.Parse(endDate)
            };

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
                _DataFields = DataFields,
                _ProjectFilter = _projectFilter
            };
            ProjeHataTanimDynamic.AddRange(ProjeHataTanim);

            #endregion MainCode
            Console.WriteLine("\n Status Close Done.");

            return PdfReport;
        }

        public async Task<PdfReport> GetStatusClose(string startDate, string endDate)
        {
            WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
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
                if (item.idProjeDurumu == 4)
                    SelectedProject = item;
            }

            int ProjectDetCount = 0;
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
            string cdateString = "";
            int ProjectDetLength = 0;
            int convertedPpm = 0;
            long cVal = 0, eVal = 0;
            int ptd_count = 0;
            string DataFields = "{ dataField: 'id', caption:'id' },{ dataField: 'kontrolTarihi', caption: 'Control Date' },{ dataField: 'uretimTarihi', caption: 'Product Date' },{ dataField: 'partNrTanimi', caption: 'Ürün Referansları Prod' },{ dataField: 'iotNo', caption: 'lotNo' },{ dataField: 'seriNo', caption: 'Seri No Serial Number' },{ dataField: 'harcanansaat', caption: 'Harcanan Süre Spent' },{ dataField: 'mesai50Hesapla', caption: '%50 Fazla Mesai' },{ dataField: 'harcanangirilenmesai', caption: '%100 Fazla Mesai' },{ dataField: 'kontrolAdedi', caption: 'Kontrol Adet Checked' },{ dataField: 'tamirAdedi', caption: 'Rötüş Adet Reworker' },{ dataField: 'hataAdeti', caption: 'Hatalı Adet Nok' },";
            await Task.Run(async () =>
            {
                foreach (var item in _SelectedProject)
                {
                    if (item.idProjeDurumu == 4)
                        SelectedProject = item;
                }

                var FilteredProjectDetails = _uow.ProjeDetay.GetAll(i => i.idProje == SelectedProject.id).Where(d => d.kontrolTarihi >= DateTime.Parse(startDate)).Where(d => d.kontrolTarihi <= DateTime.Parse(endDate));
                foreach (var ProjeDetayItem in FilteredProjectDetails)
                {
                    var GetProjeDetaysLength = _uow.ProjeDetays.GetAll(i => i.idProjeDetay == ProjeDetayItem.id).Count();
                    if (ProjeDetayItem.idProforma != 0)
                    {
                        ProjectDetLength += GetProjeDetaysLength;
                    }
                }
                Console.WriteLine("\n Status Close Begining.");
                Console.WriteLine("Length: " + ProjectDetLength + ", " + "Count: " + ProjectDetCount+", "+"Project Code: "+ _report._ProjectCode);
                foreach (var ProjeDetayItem in FilteredProjectDetails)
                {
                    var GetProjeDetays = _uow.ProjeDetays.GetAll(i => i.idProjeDetay == ProjeDetayItem.id);
                    say += GetProjeDetays.Count();
                    if (ProjeDetayItem.idProforma != 0)
                    {
                        PPM += ProjeDetayItem.ppm;
                        Overtime100 += ProjeDetayItem.harcananGirilenMesai;
                        var harcanansaat = ProjeDetayItem.harcananSaat;
                        var harcanangirilenmesai = ProjeDetayItem.harcananGirilenMesai;
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
                            ProjectDetCount++;
                            await WebSocAct.ProgressBar_WebSocket(_claim, Convert.ToDouble(ProjectDetCount), Convert.ToDouble(ProjectDetLength), _accessor.HttpContext.Request.Cookies["progress_id"]);
                        }
                    }
                }
                double ppm = (Nok / Checked) * 1000000;
                if (!Double.IsNaN(ppm))
                    convertedPpm = Convert.ToInt32(ppm);

                if (projectDetailsList.Count() > 0)
                    cdateString = projectDetailsList[0].KontrolTarihi;

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
                _DataFields = DataFields,
            };
            ProjeHataTanimDynamic.AddRange(ProjeHataTanim);

            Console.WriteLine("\n Status Close Done.");
            return PdfReport;
        }
    }
}
