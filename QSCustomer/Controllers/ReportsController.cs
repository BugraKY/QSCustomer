using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using QSCustomer.Models.ViewModels;
using QSCustomer.Views.Reports;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Threading.Tasks;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Interactive;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;
using System.Diagnostics;
using QSCustomer.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using QSCustomer.Utility;
using static QSCustomer.Utility.ProjectConstant;
using Microsoft.AspNetCore.SignalR;
using QSCustomer.Hubs;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html.simpleparser;

namespace QSCustomer.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IUnitOfWork _uow;
        public List<CustomerReports> ProjectReports = new List<CustomerReports>();
        //public List<qprojetanim> ProjectList = new List<qprojetanim>();
        public List<ProjectList> ProjectList = new List<ProjectList>();
        public List<qprojeDetays> ReportDetail = new List<qprojeDetays>();
        public List<qprojehataTanimi> ProjeHataTanimDynamic = new List<qprojehataTanimi>();
        public List<ProjectDetails> projectDetailsList = new List<ProjectDetails>();
        public List<FaultString> FaultStringsList = new List<FaultString>();
        public List<PdfReport> PdfReportList = new List<PdfReport>();
        public List<ProjectTotalsOneByDate> projectTotalsOneByDateList = new List<ProjectTotalsOneByDate>();
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _accessor;
        protected IHubContext<HomeHub> _context;

        [Obsolete]
        public ReportsController(IUnitOfWork uow, IHostingEnvironment hostingEnvironment, IHttpContextAccessor accessor, IHubContext<HomeHub> context)
        {
            _uow = uow;
            _hostingEnvironment = hostingEnvironment;
            _accessor = accessor;
            _context = context;
        }

        [Route("reports")]
        public IActionResult Index()
        {
            //var accessToken = Context.GetHttpContext().Request.Query["access_token"];
            var progressCookieVal=_accessor.HttpContext.Request.Cookies["progress_id"];
            if (progressCookieVal == null)
            {
                var _guid = Guid.NewGuid();
                var progressId = Convert.ToString(_guid);
                Response.Cookies.Append("progress_id", progressId);
            }

            //EmailSenderExtension.SendEmail("bugrakaya16@gmail.com");
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var AppUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value, includeProperties: "UserTypes");
                return View("Index", AppUser.Id);

                /*
                                if (AppUser.UserTypes.Name == UserTypeConst.Customer)
                                {
                                    var Company = _uow.MusteriYetkili.GetFirstOrDefault(i => i.id == AppUser.DefinitionId);
                                    var _userDefinition = new UserDefinition()
                                    {
                                        DefinitionId = AppUser.DefinitionId,
                                        UserTypeId = AppUser.UserTypeId
                                    };
                                    return View(_userDefinition);
                                }
                                else if(AppUser.UserTypes.Name == UserTypeConst.Operation_Area)
                                {
                                    var O_Area = _uow.FabrikaTanimYetkili.GetFirstOrDefault(i => i.id == AppUser.DefinitionId);
                                    var _userDefinition = new UserDefinition()
                                    {
                                        DefinitionId = AppUser.DefinitionId,
                                        UserTypeId = AppUser.UserTypeId
                                    };
                                    return View(_userDefinition);
                                }
                                else*/
                //var Company = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == ApplicationUser.Email);//ORIGINAL
                //TEST
                if (AppUser != null && AppUser.EmailConfirmed == false)
                    return RedirectToAction("Unconfirmed", "Home");

            }
            else
                return NotFound();
            return View(null);
            #endregion Authentication Index
        }
        [HttpPost]
        public JsonResult GetAllReportJson(string id, bool open, bool close, bool problematic)
        {
            Response.Cookies.Append("_kj6ght", open.ToString());
            Response.Cookies.Append("_h4k9xp", close.ToString());



            #region Authentication Json
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ApplicationUser AppUser;
            if (Claims != null)
            {
                AppUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value, includeProperties: "UserTypes");
                /*
                if (AppUser.UserTypes.Name == UserTypeConst.Customer)
                {
                    var Company = _uow.MusteriYetkili.GetFirstOrDefault(i => i.id == AppUser.DefinitionId);//TEST
                }
                if (AppUser.UserTypes.Name == UserTypeConst.Operation_Area)
                {
                    var Company = _uow.FabrikaTanimYetkili.GetFirstOrDefault(i => i.id == AppUser.DefinitionId);//TEST
                }
                */
                if (AppUser != null && AppUser.EmailConfirmed == false)
                    return Json(HttpStatusCode.NoContent);
            }
            else
                return Json(HttpStatusCode.NoContent);
            #endregion Authentication
            /*
            #region MainCode
            var GetAllProject = _uow.ProjeTanim.GetAll(i => i.idMusteri == id);
            foreach (var item in GetAllProject)
            {
                if (item.idProjeDurumu == 1)
                {
                    var Customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == item.idMusteri);
                    var Operation = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == item.idOprArea);
                    var Currency = _uow.ParaBirimi.GetFirstOrDefault(i => i.id == item.fiyatIdParaBirimi);
                    var ProjectStatus = _uow.ProjeDurumu.GetFirstOrDefault(i => i.id == item.idProjeDurumu);
                    var ProjectControlType = _uow.ProjeKontrolTipi.GetFirstOrDefault(i => i.id == item.fiyatIdKontrolTipi);
                    var ProjectItem = new ProjectList()
                    {
                        Id = item.id,
                        OperationArea = Operation.fabrikaAdi,
                        CustomerName = Customer.musteriAdi,
                        ProjectCode = item.projeCode,
                        State = ProjectStatus.projeDurumu,
                        ApprovalState = item.onayDurumu,
                        StartDate = item.baslangicTarihi.ToString("dd/MM/yyyy"),
                        FinishDate = item.bitisTarihi.ToString(),
                        ApprovalDate = item.onayTarih.ToString(),
                        Responsible = item.projeAcan,
                        ControlType = ProjectControlType.controlType,
                        CompNr = item.sikayetNo,
                        Material = item.materyel,
                        Note = item.note,
                        Currency = Currency.Sembol

                    };
                    ProjectList.Add(ProjectItem);
                }
            }
            #endregion MainCode
            */
            //IEnumerable<qprojetanim> GetAllProject=null;
            var firstdef = _uow.DefinitionUser.GetAll(i => i.UserId == AppUser.Id);
            List<qprojetanim> GetAllProject = new List<qprojetanim>();
            #region MainCode
            if (AppUser.UserTypes.Name == UserTypeConst.Customer)
            {
                foreach (var item in firstdef)
                {
                    var _customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == item.DefinitionId);

                    #region TEST
                    var _oprArea = _uow.FabrikaTanim.GetFirstOrDefault(n => n.fabrikaAdi == _customer.musteriAdi);
                    if (_oprArea != null)
                    {
                        var exampledate = Convert.ToDateTime("01.01.2021");
                        var _projects = _uow.ProjeTanim.GetAll(i => i.idMusteri == _customer.id).Where(n => n.idOprArea == _oprArea.id).OrderByDescending(o => o.id);
                        GetAllProject.AddRange(_projects);
                    }
                    else
                    {
                        var _projects = _uow.ProjeTanim.GetAll(i => i.idMusteri == _customer.id);
                        GetAllProject.AddRange(_projects);
                    }
                    #endregion TEST

                    /*
                    #region Original
                    var _projects = _uow.ProjeTanim.GetAll(i => i.idMusteri == _customer.id);
                    GetAllProject.AddRange(_projects);
                    #endregion Original*/
                }

            }
            if (AppUser.UserTypes.Name == UserTypeConst.Operation_Area)
            {
                /*
                foreach (var item in firstdef)
                {
                    #region TEST
                    var _oprArea = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == item.DefinitionId);
                    var exampledate = Convert.ToDateTime("01.01.2021");
                    var _projects = _uow.ProjeTanim.GetAll(i => i.idOprArea == _oprArea.id).OrderByDescending(o => o.id);
                    GetAllProject.AddRange(_projects);

                    #endregion TEST
                    //GetAllProject.AddRange(_uow.ProjeTanim.GetAll(i => i.idOprArea == item.DefinitionId));
                }*/
                var _oprArea = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == AppUser.DefinitionId);
                var exampledate = Convert.ToDateTime("01.01.2021");
                var _projects = _uow.ProjeTanim.GetAll(i => i.idOprArea == _oprArea.id).OrderByDescending(o => o.id);
                GetAllProject.AddRange(_projects);

            }
            //GetAllProject = _uow.ProjeTanim.GetAll(i => i.idMusteri == firstdef.DefinitionId);


            foreach (var item in GetAllProject)
            {
                if (open)
                {
                    if (item.idProjeDurumu == 1)
                    {
                        var Customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == item.idMusteri);
                        var Operation = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == item.idOprArea);
                        var Currency = _uow.ParaBirimi.GetFirstOrDefault(i => i.id == item.fiyatIdParaBirimi);
                        var ProjectStatus = _uow.ProjeDurumu.GetFirstOrDefault(i => i.id == item.idProjeDurumu);
                        var ProjectControlType = _uow.ProjeKontrolTipi.GetFirstOrDefault(i => i.id == item.fiyatIdKontrolTipi);

                        /*
                        var ProjectItem = new ProjectList()
                        {
                            Id = item.id,
                            OperationArea = Operation.fabrikaAdi,
                            CustomerName = Customer.musteriAdi,
                            ProjectCode = item.projeCode,
                            State = ProjectStatus.projeDurumu,
                            ApprovalState = item.onayDurumu,
                            StartDate = item.baslangicTarihi.ToString("dd/MM/yyyy"),
                            FinishDate = item.bitisTarihi.ToString(),
                            ApprovalDate = item.onayTarih.ToString(),
                            Responsible = item.projeAcan,
                            ControlType = ProjectControlType.controlType,
                            CompNr = item.sikayetNo,
                            Material = item.materyel,
                            Note = item.note,
                            Currency = Currency.Sembol

                        };
                        ProjectList.Add(ProjectItem);*/

                        var ProjeDetailCount = _uow.ProjeDetay.GetAll(i => i.idProje == item.id).Where(p => p.idProforma == 0).Count();
                        if (ProjeDetailCount > 0)
                        {
                            var ProjectItem = new ProjectList()
                            {
                                Id = item.id,
                                OperationArea = Operation.fabrikaAdi,
                                CustomerName = Customer.musteriAdi,
                                ProjectCode = item.projeCode,
                                State = ProjectStatus.projeDurumu,
                                ApprovalState = item.onayDurumu,
                                StartDate = item.baslangicTarihi.ToString("dd/MM/yyyy"),
                                FinishDate = item.bitisTarihi.ToString(),
                                ApprovalDate = item.onayTarih.ToString(),
                                Responsible = item.projeAcan,
                                ControlType = ProjectControlType.controlType,
                                CompNr = item.sikayetNo,
                                Material = item.materyel,
                                Note = item.note,
                                Currency = Currency.Sembol

                            };
                            ProjectList.Add(ProjectItem);
                        }

                    }
                }
                if (close)
                {
                    if (item.idProjeDurumu == 4)
                    {
                        var Customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == item.idMusteri);
                        var Operation = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == item.idOprArea);
                        var Currency = _uow.ParaBirimi.GetFirstOrDefault(i => i.id == item.fiyatIdParaBirimi);
                        var ProjectStatus = _uow.ProjeDurumu.GetFirstOrDefault(i => i.id == item.idProjeDurumu);
                        var ProjectControlType = _uow.ProjeKontrolTipi.GetFirstOrDefault(i => i.id == item.fiyatIdKontrolTipi);
                        var ProjectItem = new ProjectList()
                        {
                            Id = item.id,
                            OperationArea = Operation.fabrikaAdi,
                            CustomerName = Customer.musteriAdi,
                            ProjectCode = item.projeCode,
                            State = ProjectStatus.projeDurumu,
                            ApprovalState = item.onayDurumu,
                            StartDate = item.baslangicTarihi.ToString("dd/MM/yyyy"),
                            FinishDate = item.bitisTarihi.ToString(),
                            ApprovalDate = item.onayTarih.ToString(),
                            Responsible = item.projeAcan,
                            ControlType = ProjectControlType.controlType,
                            CompNr = item.sikayetNo,
                            Material = item.materyel,
                            Note = item.note,
                            Currency = Currency.Sembol

                        };
                        ProjectList.Add(ProjectItem);
                    }
                }
                if (open && close)
                {
                    if (item.idProjeDurumu == 1 && item.idProjeDurumu == 4)
                    {
                        var Customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == item.idMusteri);
                        var Operation = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == item.idOprArea);
                        var Currency = _uow.ParaBirimi.GetFirstOrDefault(i => i.id == item.fiyatIdParaBirimi);
                        var ProjectStatus = _uow.ProjeDurumu.GetFirstOrDefault(i => i.id == item.idProjeDurumu);
                        var ProjectControlType = _uow.ProjeKontrolTipi.GetFirstOrDefault(i => i.id == item.fiyatIdKontrolTipi);

                        var ProjeDetailCount = _uow.ProjeDetay.GetAll(i => i.idProje == item.id).Count();
                        if (ProjeDetailCount > 0)
                        {
                            var ProjectItem = new ProjectList()
                            {
                                Id = item.id,
                                OperationArea = Operation.fabrikaAdi,
                                CustomerName = Customer.musteriAdi,
                                ProjectCode = item.projeCode,
                                State = ProjectStatus.projeDurumu,
                                ApprovalState = item.onayDurumu,
                                StartDate = item.baslangicTarihi.ToString("dd/MM/yyyy"),
                                FinishDate = item.bitisTarihi.ToString(),
                                ApprovalDate = item.onayTarih.ToString(),
                                Responsible = item.projeAcan,
                                ControlType = ProjectControlType.controlType,
                                CompNr = item.sikayetNo,
                                Material = item.materyel,
                                Note = item.note,
                                Currency = Currency.Sembol
                            };
                            ProjectList.Add(ProjectItem);
                        }
                    }
                }

            }
            #endregion MainCode


            return Json(ProjectList.ToList());
        }
        [HttpPost]
        public JsonResult GetReportTotalValuesJson([FromBody] List<CustomerReports> customerReports)
        {
            #region Get the totals of all reports
            var ReportTotals = new ReportsTotalValues();

            if (customerReports == null)
            {
                ReportTotals.qprojeDetayTKontrolAdedi = 0;
                ReportTotals.qprojeDetayTHataAdeti = 0;
                ReportTotals.qprojeDetayTTamirAdedi = 0;
                ReportTotals.qprojeDetayTOperatorSayisi = 0;
                ReportTotals.qprojeDetayToplamSuredk = 0;
                ReportTotals.qprojeDetayGerceklesenDk = 0;
                ReportTotals.qprojeDetayHarcananSaat = 0;
                ReportTotals.qprojeDetayToplam = 0;
            }
            else
            {
                for (int i = 0; i < customerReports.Count; i++)
                {
                    ReportTotals.qprojeDetayTKontrolAdedi += customerReports[i].qprojeDetayTKontrolAdedi;
                    ReportTotals.qprojeDetayTHataAdeti += customerReports[i].qprojeDetayTHataAdeti;
                    ReportTotals.qprojeDetayTTamirAdedi += customerReports[i].qprojeDetayTTamirAdedi;
                    ReportTotals.qprojeDetayTOperatorSayisi += customerReports[i].qprojeDetayTOperatorSayisi;
                    ReportTotals.qprojeDetayToplamSuredk += customerReports[i].qprojeDetayToplamSuredk;
                    ReportTotals.qprojeDetayGerceklesenDk += customerReports[i].qprojeDetayGerceklesenDk;
                    ReportTotals.qprojeDetayHarcananSaat += customerReports[i].qprojeDetayHarcananSaat;
                    ReportTotals.qprojeDetayToplam += customerReports[i].qprojeDetayToplam;
                }
            }
            #endregion Get the totals of all reports

            return Json(ReportTotals);
        }

        //[Route("project/{projectCode}")]
        //[HttpPost("project")]
        //[HttpGet("reports/project={projectCode}&open={open}&close={close}")]
        [HttpGet("reports/project")]
        public IActionResult Details(string startDate, string finishDate, string projectCode, bool open, bool close, bool problematic,int filterRadio)
        //public IActionResult Details(string projectCode)
        {
            #region Variables
            string[] finishDateArr = finishDate.Split(' ');

            string Weak_str = finishDateArr[0];
            string Month_str = finishDateArr[1];
            string Day_str = finishDateArr[2];
            string Yearstr = finishDateArr[3];

            int Day = int.Parse(Day_str);
            int Month = 0;
            int Year = int.Parse(Yearstr);


            if (Month_str == "Jan")
                Month = 1;
            if (Month_str == "Feb")
                Month = 2;
            if (Month_str == "Mar")
                Month = 3;
            if (Month_str == "Apr")
                Month = 4;
            if (Month_str == "May")
                Month = 5;
            if (Month_str == "Jun")
                Month = 6;
            if (Month_str == "Jul")
                Month = 7;
            if (Month_str == "Aug")
                Month = 8;
            if (Month_str == "Sep")
                Month = 9;
            if (Month_str == "Oct")
                Month = 10;
            if (Month_str == "Nov")
                Month = 11;
            if (Month_str == "Dec")
                Month = 12;


            if (startDate.Contains('\"'))
            {
                startDate = startDate.Replace("\"", "");
                string[] VirtualDate = startDate.Split('.');
                string Month_virtual = VirtualDate[0];
                string Day_virtual = VirtualDate[1];
                string Year_virtual = VirtualDate[2];
                startDate = Day_virtual +"."+ Month_virtual +"."+ Year_virtual;
            }


            startDate = DateTime.Parse(startDate).ToString("dd/MM/yyyy");


            DateTime _startDate = Convert.ToDateTime(startDate);
            DateTime _finishDate = new DateTime(Year, Month, Day);

            if (filterRadio == 0)
            {
                var projectId = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode).id;
                var _projectDetail = _uow.ProjeDetay.GetAll(i => i.idProje == projectId).ToList();
                var _projectDetailCount = _uow.ProjeDetay.GetAll(i => i.idProje == projectId).Count();
                //var _startDateFilter = _projectDetail[_projectDetailCount-7].kontrolTarihi;
                var _finishDateFilter = _projectDetail[_projectDetailCount - 1].kontrolTarihi;
                var _startDateFilter = _finishDateFilter;
                _startDate = _startDateFilter;
                _finishDate = _finishDateFilter;
            }
            if (filterRadio == 1)
            {
                var projectId = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode).id;
                var _projectDetail = _uow.ProjeDetay.GetAll(i => i.idProje == projectId).ToList();
                var _projectDetailCount = _uow.ProjeDetay.GetAll(i => i.idProje == projectId).Count();
                //var _startDateFilter = _projectDetail[_projectDetailCount-7].kontrolTarihi;
                var _finishDateFilter = _projectDetail[_projectDetailCount-1].kontrolTarihi;
                var _startDateFilter = _finishDateFilter.AddDays(-7);
                _startDate = _startDateFilter;
                _finishDate = _finishDateFilter;
            }
            if (filterRadio == 2)
            {
                var projectId = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode).id;
                var _projectDetail = _uow.ProjeDetay.GetAll(i => i.idProje == projectId).ToList();
                var _projectDetailCount = _uow.ProjeDetay.GetAll(i => i.idProje == projectId).Count();
                //var _startDateFilter = _projectDetail[_projectDetailCount-7].kontrolTarihi;
                var _finishDateFilter = _projectDetail[_projectDetailCount - 1].kontrolTarihi;
                var _startDateFilter = _finishDateFilter.AddMonths(-1);
                _startDate = _startDateFilter;
                _finishDate = _finishDateFilter;
            }
            if (filterRadio == 4)
            {
                var projectId = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode).id;
                var _projectDetail = _uow.ProjeDetay.GetAll(i => i.idProje == projectId).ToList();
                var _projectDetailCount = _uow.ProjeDetay.GetAll(i => i.idProje == projectId).Count();
                //var _startDateFilter = _projectDetail[_projectDetailCount-7].kontrolTarihi;
                var _finishDateFilter = _projectDetail[_projectDetailCount - 1].kontrolTarihi;
                var _startDateFilter = _finishDateFilter.AddMonths(-2);
                _startDate = _startDateFilter;
                _finishDate = _finishDateFilter;
            }

            //return NoContent();


            //_dateTime.Date.Month = DateTime.Parse(Month).Month;
            #endregion Variables

            #region Authentication
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (projectCode == null)
                return NotFound();
            var SelectedProject = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode);
            var ProjeHataTanim = _uow.ProjeHataTanimi.GetAll(i => i.idProje == SelectedProject.id);
            var Customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == SelectedProject.idMusteri);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                //var CompanyAuth = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == ApplicationUser.Email);//ORGINAL


                /*
                var CompanyAuth = _uow.MusteriYetkili.GetFirstOrDefault(i => i.id == Customer.id);//TEST
                if (SelectedProject.idMusteri != CompanyAuth.idMusteriTanim)
                    return NotFound();*/


                if (ApplicationUser != null && ApplicationUser.EmailConfirmed == false)
                    return RedirectToAction("Unconfirmed", "Home");
            }
            else
                return NotFound();
            #endregion Authentication
            ProjectVariables.CountProgress = 0;
            ProjectVariables.LengthProgress = 0;
            #region Queries
            var Operation = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == SelectedProject.idOprArea);
            var Currency = _uow.ParaBirimi.GetFirstOrDefault(i => i.id == SelectedProject.fiyatIdParaBirimi);
            var ProjectStatus = _uow.ProjeDurumu.GetFirstOrDefault(i => i.id == SelectedProject.idProjeDurumu);
            var ProjectControlType = _uow.ProjeKontrolTipi.GetFirstOrDefault(i => i.id == SelectedProject.fiyatIdKontrolTipi);
            var GetProjeDetay = _uow.ProjeDetay.GetAll(i => i.idProje == SelectedProject.id);
            #endregion Queries

            var FilteredProjectDetails = _uow.ProjeDetay.GetAll(i => i.idProje == SelectedProject.id).Where(d => d.kontrolTarihi >= _startDate).Where(d => d.kontrolTarihi <= _finishDate);

            #region MainCode
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


                        foreach (var HataTanimi in ProjeHataTanim)
                        {
                            // HataTanimi.hataTanimi; ---> bir hata tanımı idprojedetayın id sine göre alınacak. Her hata tanımı idprojedetaydakine özel column eklenecek. Bu column html tabloda aolacak. Html tablodaki hata tanımlarının adetleri olacak.
                            // Dynamic Model yapılabilir.
                        }



                        harcanansaat = 0;//foreach döngüsünde bir kere yazılması gerektiği için sıfırlıyoruz.
                        harcanangirilenmesai = 0;//foreach döngüsünde bir kere yazılması gerektiği için sıfırlıyoruz.
                        s++;
                    }
                }
            }

            var deg = s;
            var deg_2 = projedetay_say;*/
            ProjectFilter projectFilter = new ProjectFilter()
            {
                StartDate = _startDate,
                FinishDate = _finishDate
            };
            ProjectState states = new ProjectState()
            {
                Close = close,
                Open = open,
                Problematic = problematic
            };
            PdfReport report = new PdfReport()
            {
                _ProjectCode = projectCode,
                _ProjeHataTanimCount = ProjeHataTanim.Count(),
                _ProjectState = states,
                _ProjectFilter = projectFilter,
                _FilterRadio=filterRadio
            };

            if (SelectedProject != null)
                return View("Details", report);
            else
                return NotFound();

            #endregion MainCode

        }
        [HttpGet("detailTest/{projectCode}")]
        public async Task<JsonResult> DetailTest(string projectCode, string startDate, string finishDate, bool open, bool close, bool problematic)
        {
            Console.WriteLine("Section 1");
            #region Authentication
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (projectCode == null)
                return Json(HttpStatusCode.NoContent);
            var AuthSelectedProject = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode);
            var AuthProjeHataTanim = _uow.ProjeHataTanimi.GetAll(i => i.idProje == AuthSelectedProject.id);
            var AuthCustomer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == AuthSelectedProject.idMusteri);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                //var CompanyAuth = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == ApplicationUser.Email); //ORIGINAL
                /*
                var CompanyAuth = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == "Goezde.Avci@fst.com"); //TEST
                if (AuthSelectedProject.idMusteri != CompanyAuth.idMusteriTanim)
                    return Json(HttpStatusCode.NoContent);
                */
                if (ApplicationUser != null && ApplicationUser.EmailConfirmed == false)
                    return Json(HttpStatusCode.Unauthorized);
            }
            else
                return Json(HttpStatusCode.NoContent);
            #endregion Authentication

            Console.WriteLine("Section 2");
            if (AuthSelectedProject.idProjeDurumu == 4)
            {
                open = false;
                close = true;
            }
            if (AuthSelectedProject.idProjeDurumu == 1)
            {
                open = true;
                close = false;
            }

            ProjectState state = new ProjectState()
            {
                Close = close,
                Open = open,
                Problematic = problematic
            };
            PdfReport report = new PdfReport()
            {
                _ProjectCode = projectCode,
                _ProjectState = state,
                _SelectedProject = AuthSelectedProject,
            };
            PdfReport StatusOpenProjects = new PdfReport();
            PdfReport StatusCloseProjects = new PdfReport();
            PdfReport StatusProblematicProjects = new PdfReport();

            PdfReport AllReports = new PdfReport();
            Console.WriteLine("Section 3");
            GetProjectDetailsExtensions getProjectDetail = new GetProjectDetailsExtensions(_uow, report,GetClaim(),_context,_accessor);

            if (open == true && close == true)
            {
                StatusOpenProjects = await getProjectDetail.GetStatusOpen(startDate, finishDate);
                StatusCloseProjects = await getProjectDetail.GetStatusClose(startDate, finishDate);

                AllReports = StatusOpenProjects;

                AllReports._CheckedTotal += StatusCloseProjects._CheckedTotal;
                AllReports._NokTotal += StatusCloseProjects._NokTotal;
                AllReports._OverTime100Total += StatusCloseProjects._OverTime100Total;
                AllReports._OverTime50Total += StatusCloseProjects._OverTime50Total;
                AllReports._PartNrTanimlari.AddRange(StatusCloseProjects._PartNrTanimlari);
                AllReports._PPMTotal += StatusCloseProjects._PPMTotal;
                AllReports._ProjectDetails.AddRange(StatusCloseProjects._ProjectDetails);
                AllReports._ProjectTotalsOnebyDate.AddRange(StatusCloseProjects._ProjectTotalsOnebyDate);
                AllReports._ProjeHataTanim.AddRange(StatusCloseProjects._ProjeHataTanim);
                AllReports._ProjeHataTanimCount += StatusCloseProjects._ProjeHataTanimCount;
                AllReports._ReworkedTotal += StatusCloseProjects._ReworkedTotal;
                AllReports._SpentHours += StatusCloseProjects._SpentHours;
                AllReports._SpentHoursTotal += StatusCloseProjects._SpentHoursTotal;
                return Json(AllReports);
            }
            if (open == true)
            {
                StatusOpenProjects = await getProjectDetail.GetStatusOpen(startDate, finishDate);
                return Json(StatusOpenProjects);
            }
            if (close == true)
            {
                StatusCloseProjects = await getProjectDetail.GetStatusClose(startDate, finishDate);
                return Json(StatusCloseProjects);
            }
            return Json(StatusProblematicProjects);
        }

        [Route("pdfviewtest/{projectCode}")]
        public IActionResult PDFviewTest(string projectCode)
        {
            #region Authentication
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (projectCode == null)
                return NotFound();
            var AuthSelectedProject = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode);
            var AuthProjeHataTanim = _uow.ProjeHataTanimi.GetAll(i => i.idProje == AuthSelectedProject.id);
            var AuthCustomer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == AuthSelectedProject.idMusteri);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                //var CompanyAuth = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == ApplicationUser.Email); //ORIGINAL
                var CompanyAuth = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == "Goezde.Avci@fst.com"); // TEST
                if (AuthSelectedProject.idMusteri != CompanyAuth.idMusteriTanim)
                    return NotFound();
                if (ApplicationUser != null && ApplicationUser.EmailConfirmed == false)
                    return RedirectToAction("Unconfirmed", "Home");
            }
            else
                return Json(HttpStatusCode.NoContent);
            #endregion Authentication

            var SelectedProject = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode);
            var ProjeHataTanim = _uow.ProjeHataTanimi.GetAll(i => i.idProje == SelectedProject.id);

            var Customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == SelectedProject.idMusteri);
            var Operation = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == SelectedProject.idOprArea);
            var Currency = _uow.ParaBirimi.GetFirstOrDefault(i => i.id == SelectedProject.fiyatIdParaBirimi);
            var ProjectStatus = _uow.ProjeDurumu.GetFirstOrDefault(i => i.id == SelectedProject.idProjeDurumu);
            var ProjectControlType = _uow.ProjeKontrolTipi.GetFirstOrDefault(i => i.id == SelectedProject.fiyatIdKontrolTipi);
            var PartNrTanimlari = _uow.ProjePartNrTanimi.GetAll(i => i.idProje == SelectedProject.id);


            var GetProjeDetay = _uow.ProjeDetay.GetAll(i => i.idProje == SelectedProject.id);
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
            foreach (var ProjeDetayItem in GetProjeDetay)
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
                _ProjectTotalsOnebyDate = projectTotalsOneByDateList.ToList(),
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
            ViewBag.dataSource = projectTotalsOneByDateList;
            var _aaaaaaa = Test;
            return View(PdfReport);
        }
        [HttpGet("FaultDefinition/{projectCode}")]
        public JsonResult FaultDefinition(string projectCode)//Hata Tanımı Json Result
        {
            var SelectedProject = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode);
            var ProjeHataTanim = _uow.ProjeHataTanimi.GetAll(i => i.idProje == SelectedProject.id);
            foreach (var HataTanim in ProjeHataTanim)
            {
                var hataDetay = _uow.ProjeHataDetay.GetAll(i => i.idHataTanimi == HataTanim.id);
                foreach (var detay in hataDetay)
                {

                }
            }

            ProjeHataTanimDynamic.AddRange(ProjeHataTanim);

            /*
            dynamic dynamic = new System.Dynamic.ExpandoObject();

            foreach (var HataTanim in ProjeHataTanimDynamic)
            {
                ((IDictionary<String,int>)dynamic).Add(HataTanim.hataTanimi, HataTanim.id);
            }*/

            // output - from Json.Net NuGet package
            //var deg = Newtonsoft.Json.JsonConvert.SerializeObject(dynamic);
            //return Json(deg);

            //hatadetaydan hatatanim id sine göre yapılabilir.
            return Json(ProjeHataTanim);
        }


        [Route("pdfview/{projectCode}")]
        public IActionResult PDFview(string projectCode)
        {
            var SelectedProject = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode);
            return View("PDFview", projectCode);
            //return View(projectCode);
        }
        [Obsolete]
        public async Task<IActionResult> download(string id)
        {


            string _url = "https://localhost:5001/pdfviewtest/" + id + "?", derlenmisweb;


            using (var client = new WebClient())
            {
                Task<string> downloadStringTask = client.DownloadStringTaskAsync(new Uri(_url));
                derlenmisweb = await downloadStringTask;
            }



            Regex hrefTags = new Regex(@"<script type='text/javascript'(.|\n)*?script>", RegexOptions.IgnoreCase);
            string scriptText = hrefTags.Match(derlenmisweb).Value;



            /*
            IWebDriverExtension.CheckProcess(_url);
            var test = IWebDriverExtension.WebDriverModel.PageSource;
            */



            //PDDocument pdocument = PDDocument.load(targetStream);
            //return Content(test, "text/html", Encoding.UTF8);

            //return Content(test,"text/html",Encoding.UTF8);

            /*
            HtmlToPdf Converter = new HtmlToPdf();
            PdfDocument Doc = Converter.ConvertHtmlString(CdnCss+html+CdnJss,"test.pdf");
            
            Doc.Save("Test.pdf");
            Doc.Close();*/

            /*
            StringReader sr = new StringReader(derlenmisweb);
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                return Content(memoryStream.ToString());
            }
            */



            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            WebKitConverterSettings webkitConverterSettings = new WebKitConverterSettings();
            webkitConverterSettings.WebKitPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ExtensionLibs/QtBinariesDotNetCore");
            webkitConverterSettings.SplitImages = true;
            webkitConverterSettings.Orientation = PdfPageOrientation.Landscape;
            htmlConverter.ConverterSettings = webkitConverterSettings;
            PdfDocument document = htmlConverter.Convert(derlenmisweb, _url);
            document.PageSettings.Margins.Top = 200;
            MemoryStream ms = new MemoryStream();

            document.Save(ms);
            document.Close(true);
            ms.Position = 0;


            return File(ms, "application/pdf");// OPEN PDF IN WEB PAGE

            //return Content();
        }
        [HttpPost]
        public IActionResult LoadingPdfView(PdfReport pdfReport)
        {
            var _projectTotalsOneByDate = new List<ProjectTotalsOneByDate>();
            #region TEST
            /*
            var SelectedProject = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == pdfReport._ProjectCode);
            var ProjeHataTanim = _uow.ProjeHataTanimi.GetAll(i => i.idProje == SelectedProject.id).Where(e=>e.hataTanimi!="");

            var Customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == SelectedProject.idMusteri);
            var Operation = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == SelectedProject.idOprArea);
            //var Currency = _uow.ParaBirimi.GetFirstOrDefault(i => i.id == SelectedProject.fiyatIdParaBirimi);
            //var ProjectStatus = _uow.ProjeDurumu.GetFirstOrDefault(i => i.id == SelectedProject.idProjeDurumu);
            //var ProjectControlType = _uow.ProjeKontrolTipi.GetFirstOrDefault(i => i.id == SelectedProject.fiyatIdKontrolTipi);
            var PartNrTanimlari = _uow.ProjePartNrTanimi.GetAll(i => i.idProje == SelectedProject.id);
            var GetProjeDetay = _uow.ProjeDetay.GetAll(i => i.idProje == SelectedProject.id);


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
            foreach (var ProjeDetayItem in GetProjeDetay)
            {


                var GetProjeDetays = _uow.ProjeDetays.GetAll(i => i.idProjeDetay == ProjeDetayItem.id);
                say += GetProjeDetays.Count();
                if (ProjeDetayItem.idProforma == 0)
                {
                    PPM += ProjeDetayItem.ppm;
                    Overtime100 += ProjeDetayItem.harcananGirilenMesai;
                    projedetay_say++;
                    var harcanansaat = ProjeDetayItem.harcananSaat;
                    var harcanangirilenmesai = ProjeDetayItem.harcananGirilenMesai;
                    foreach (var ProjeDetaysItem in GetProjeDetays)
                    {


                        qprojepartNrTanimi? getPartNrItem = _uow.ProjePartNrTanimi.GetFirstOrDefault(i => i.id == ProjeDetaysItem.idReferansPartNr);

                        List<qprojeHataDetay?> getHataDetay = (List<qprojeHataDetay?>)_uow.ProjeHataDetay.GetAll(i => i.idProjeDetays == ProjeDetaysItem.id).OrderBy(x => x.idHataTanimi).ToList();

                        harcanansaat = 0;
                        harcanangirilenmesai = 0;
                        s++;
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
                _ProjectTotalsOnebyDate = projectTotalsOneByDateList.ToList(),
                _CheckedTotal = Checked,
                _ReworkedTotal = Reworked,
                _NokTotal = Nok,
                _SpentHoursTotal = SpentHours,
                _SpentHours = (SpentHours - Overtime100),
                _OverTime50Total = Overtime50,
                _OverTime100Total = Overtime100,
                _PPMTotal = convertedPpm,
                _DataFields = DataFields,
                _ProjectFilter=pdfReport._ProjectFilter
            };

            var deg = s;
            var deg_2 = projedetay_say;
            ProjeHataTanimDynamic.AddRange(ProjeHataTanim);
            //return Json(projectDetailsList.ToList());
            ViewBag.dataSource = projectTotalsOneByDateList;
            var _aaaaaaa = Test;
            */
            //pdfReport.
            #endregion TEST
            var _selectedProject = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == pdfReport._ProjectCode);
            var _ProjectDetails = _uow.ProjeDetay.GetAll(i => i.idProje == _selectedProject.id).Where(d => d.kontrolTarihi >= pdfReport._ProjectFilter.StartDate).Where(d => d.kontrolTarihi <= pdfReport._ProjectFilter.FinishDate);

            foreach (var item in _ProjectDetails)
            {
                var _chartvalues = new ProjectTotalsOneByDate()
                {
                    kontrolTarihi = item.kontrolTarihi.ToString("dd/MM/yyyy"),
                    hataAdeti = item.tHataAdeti,
                    kontrolAdedi = item.tKontrolAdedi
                };
                _projectTotalsOneByDate.Add(_chartvalues);
            }
            var _pdfReport = new PdfReport()
            {
                _ProjectCode = pdfReport._ProjectCode,
                _ProjectFilter = pdfReport._ProjectFilter,
                _ProjectTotalsOnebyDate = _projectTotalsOneByDate
            };

            //return NoContent();
            return View("loadingPdfView", _pdfReport);
        }

        [HttpPost]
        [Obsolete]
        //[ValidateInput(false)]
        public IActionResult Renderedpdf(PdfReport renderedHtmlStr)
        {
            #region MainCode
            string derlenmisweb = PDFString.Before + renderedHtmlStr._HtmlString + PDFString.After;
            return Redirect("/reports/project?startDate=Fri+Jan+01+2021+00%3A00%3A00+GMT%2B0300+%28GMT%2B03%3A00%29&finishDate=Mon+Feb+14+2022+14%3A30%3A02+GMT%2B0300+%28GMT%2B03%3A00%29&projectCode=FR22-8&open=True&open=false&close=false");
            string _url = "https://localhost:5001/pdfviewtest/" + renderedHtmlStr._SelectedProject.projeCode + "?";


            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            WebKitConverterSettings webkitConverterSettings = new WebKitConverterSettings();
            webkitConverterSettings.WebKitPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ExtensionLibs/QtBinariesDotNetCore");

            webkitConverterSettings.Orientation = PdfPageOrientation.Landscape;
            //webkitConverterSettings.EnableForm = true;
            webkitConverterSettings.EnableRepeatTableHeader = true;
            /*webkitConverterSettings.EnableRepeatTableFooter = true;*/
            webkitConverterSettings.MediaType = MediaType.Print;
            webkitConverterSettings.Margin.Bottom = 40;
            webkitConverterSettings.Margin.Top = 20;
            /*webkitConverterSettings.AdditionalDelay = 3000;*/
            htmlConverter.ConverterSettings = webkitConverterSettings;
            PdfDocument document = new PdfDocument();
            document = htmlConverter.Convert(derlenmisweb, _url);
            MemoryStream ms = new MemoryStream();

            document.Save(ms);
            document.Close(true);
            ms.Position = 0;
            #endregion MainCode

            return File(ms, "application/pdf", renderedHtmlStr._SelectedProject.projeCode + ".pdf", true);// Download Directly Pdf
            //return File(ms, "application/pdf",renderedHtmlStr._SelectedProject.projeCode+".pdf");// Open Pdf File in Web
        }

        [Obsolete]
        [HttpPost("DownloadPDF")]
        public async Task<IActionResult> DownloadPdf(PdfReport renderedHtmlStr)
        {
            string MainTab_Chart = renderedHtmlStr._HtmlString;
            string startDate = renderedHtmlStr._ProjectFilter.StartDate.ToString();
            string finishDate = renderedHtmlStr._ProjectFilter.FinishDate.ToString();
            PdfReport StatusOpenProjects = new PdfReport();
            PdfReport StatusCloseProjects = new PdfReport();

            string projectCode = renderedHtmlStr._ProjectCode;
            //string projectCode = "FR21-25";

            #region Authentication
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (projectCode == null)
                return Json(HttpStatusCode.NoContent);
            var AuthSelectedProject = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode);
            var ProjectStatus = _uow.ProjeDurumu.GetFirstOrDefault(i => i.id == AuthSelectedProject.idProjeDurumu);
            var AuthProjeHataTanim = _uow.ProjeHataTanimi.GetAll(i => i.idProje == AuthSelectedProject.id);
            var AuthCustomer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == AuthSelectedProject.idMusteri);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                //var CompanyAuth = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == ApplicationUser.Email); //ORIGINAL
                /*
                var CompanyAuth = _uow.MusteriYetkili.GetFirstOrDefault(i => i.id == "Goezde.Avci@fst.com"); //TEST
                if (AuthSelectedProject.idMusteri != CompanyAuth.idMusteriTanim)
                    return Json(HttpStatusCode.NoContent);
                if (ApplicationUser != null && ApplicationUser.EmailConfirmed == false)
                    return Json(HttpStatusCode.Unauthorized);*/
            }
            else
                return Json(HttpStatusCode.NoContent);
            #endregion Authentication

            PdfReport report = new PdfReport()
            {
                _ProjectCode = projectCode,
                _SelectedProject = AuthSelectedProject,
            };
            PdfReport StatusProblematicProjects = new PdfReport();

            PdfReport AllReports = new PdfReport();
            Console.WriteLine("Section 3");
            GetProjectDetailsExtensions getProjectDetail = new GetProjectDetailsExtensions(_uow, report,GetClaim(),_context,_accessor);
            RenderHtmlTableExtensions renderHtmlTable = new RenderHtmlTableExtensions();

            if (ProjectStatus.sıra == 1)
            {
                StatusOpenProjects = await getProjectDetail.GetStatusOpen(startDate, finishDate);
            }
            else if(ProjectStatus.sıra == 4)
            {
                StatusOpenProjects = await getProjectDetail.GetStatusClose(startDate, finishDate);
            }

            //StatusCloseProjects = getProjectDetail.GetStatusClose();

            string RenderedTableString = await renderHtmlTable.GetTableString(StatusOpenProjects, MainTab_Chart);

            string _PdfString = PDFString.Before + RenderedTableString + PDFString.After;
            //string baseUrl = string.Empty;
            //string baseUrl = "https://localhost:5001/reports/DownloadPdf";
            string baseUrl = "https://localhost:5001/baseurlforpdfconvert/";


            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            WebKitConverterSettings webkitConverterSettings = new WebKitConverterSettings();
            webkitConverterSettings.WebKitPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ExtensionLibs/QtBinariesDotNetCore");

            webkitConverterSettings.Orientation = PdfPageOrientation.Landscape;
            webkitConverterSettings.EnableRepeatTableHeader = true;
            webkitConverterSettings.MediaType = MediaType.Print;
            //webkitConverterSettings.Margin.Bottom = 40;
            webkitConverterSettings.Margin.Bottom = 30;
            webkitConverterSettings.Margin.Top = 20;

            htmlConverter.ConverterSettings = webkitConverterSettings;
            PdfDocument document = new PdfDocument();
            document = htmlConverter.Convert(_PdfString, baseUrl);
            MemoryStream ms = new MemoryStream();

            document.Save(ms);
            document.Close(true);
            ms.Position = 0;

            Console.WriteLine("Pdf Converted.");
            return File(ms, "application/pdf", projectCode + ".pdf", true);// Download Directly Pdf
            //return File(ms, "application/pdf", true);// Open Pdf File in Web
            /*
            MemoryStream ms = new MemoryStream();
            iText.Html2pdf.HtmlConverter.ConvertToElements(_PdfString);*/
        }
        //[HttpGet("baseurlforpdfconvert/{projectCode}")]
        [Route("/baseurlforpdfconvert/{projectCode}")]
        public IActionResult BaseUrlforPdfConvert(string projectCode)
        {
            //projectCode = "FR21-25";
            #region Authentication
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (projectCode == null)
                return NotFound();
            var AuthSelectedProject = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode);
            var AuthProjeHataTanim = _uow.ProjeHataTanimi.GetAll(i => i.idProje == AuthSelectedProject.id);
            var AuthCustomer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == AuthSelectedProject.idMusteri);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                //var CompanyAuth = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == ApplicationUser.Email); //ORIGINAL
                var CompanyAuth = _uow.MusteriYetkili.GetFirstOrDefault(i => i.mail == "Goezde.Avci@fst.com"); // TEST
                if (AuthSelectedProject.idMusteri != CompanyAuth.idMusteriTanim)
                    return NotFound();
                if (ApplicationUser != null && ApplicationUser.EmailConfirmed == false)
                    return RedirectToAction("Unconfirmed", "Home");
            }
            else
                return Json(HttpStatusCode.NoContent);
            #endregion Authentication


            #region API_CHART_and_Rework
            var ProjectTotalsOneBydate = new ProjectTotalsOneByDate();
            var SelectedProject = _uow.ProjeTanim.GetFirstOrDefault(i => i.projeCode == projectCode);
            var ProjeHataTanim = _uow.ProjeHataTanimi.GetAll(i => i.idProje == SelectedProject.id);

            var Customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == SelectedProject.idMusteri);
            var Operation = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == SelectedProject.idOprArea);
            var Currency = _uow.ParaBirimi.GetFirstOrDefault(i => i.id == SelectedProject.fiyatIdParaBirimi);
            var ProjectStatus = _uow.ProjeDurumu.GetFirstOrDefault(i => i.id == SelectedProject.idProjeDurumu);
            var ProjectControlType = _uow.ProjeKontrolTipi.GetFirstOrDefault(i => i.id == SelectedProject.fiyatIdKontrolTipi);
            var PartNrTanimlari = _uow.ProjePartNrTanimi.GetAll(i => i.idProje == SelectedProject.id);


            var GetProjeDetay = _uow.ProjeDetay.GetAll(i => i.idProje == SelectedProject.id);
            int s = 0;
            int say = 0;
            double Checked = 0;
            int Reworked = 0;
            double Nok = 0;
            double SpentHours = 0;
            double Overtime100 = 0;


            int projedetay_say = 0;
            foreach (var ProjeDetayItem in GetProjeDetay)
            {
                if (ProjeDetayItem.idProforma == 0)
                {
                    var ProjectTotals = new ProjectTotalsOneByDate()
                    {
                        hataAdeti = ProjeDetayItem.tHataAdeti,
                        kontrolAdedi = ProjeDetayItem.tKontrolAdedi,
                        kontrolTarihi = ProjeDetayItem.kontrolTarihi.ToString()
                    };
                    projectTotalsOneByDateList.Add(ProjectTotals);

                    Checked += (double)ProjectTotals.kontrolAdedi;
                    Nok += (double)ProjectTotals.hataAdeti;
                    SpentHours += ProjeDetayItem.harcananSaat;
                    Reworked += (int)ProjeDetayItem.tTamirAdedi;
                    Overtime100 += ProjeDetayItem.harcananGirilenMesai;
                }

                /*
                var GetProjeDetays = _uow.ProjeDetays.GetAll(i => i.idProjeDetay == ProjeDetayItem.id);
                foreach (var item in GetProjeDetays)
                {
                    if ((item.toplamSuredk == 0) && (item.idReferansPartNr == 0) && (item.KontrolAdedi == 0) && (item.SaatUcreti == 0))
                    {
                        Checked += (double)item.KontrolAdedi;
                        Nok += (double)item.HataAdeti;
                        if (ProjeDetayItem.mesaiHesapla100)
                            SpentHours += item.harcananGirilenMesai;
                        Reworked += (int)item.TamirAdedi;
                    }

                }
                */
            }
            double ppm = (Nok / Checked) * 1000000;
            List<qprojepartNrTanimi> qprojepartNrTanimiList = new List<qprojepartNrTanimi>();
            qprojepartNrTanimiList.AddRange(PartNrTanimlari);
            List<qprojehataTanimi> qprojehataTanimiList = new List<qprojehataTanimi>();
            qprojehataTanimiList.AddRange(ProjeHataTanim);

            var pdfrepor = new PdfReport()
            {
                _ProjectTotalsOnebyDate = projectTotalsOneByDateList,
                _SelectedProject = SelectedProject,
                _Operation = Operation,
                _Customer = Customer,
                _PartNrTanimlari = qprojepartNrTanimiList,
                _ProjeHataTanim = qprojehataTanimiList,
                _SpentHours = (SpentHours - Overtime100),
                _PPMTotal = Convert.ToInt32(ppm),
                _NokTotal = Nok,
                _CheckedTotal = Checked,
                _OverTime100Total = Overtime100,
                _SpentHoursTotal = SpentHours
            };

            #endregion API_CHART_and_Rework


            return View(pdfrepor);
        }


        public IActionResult requestUrl()
        {
            return View();
        }

        [HttpGet("progressbarval")]
        public async Task<JsonResult> ProgressbarAsync()
        {
            /*
            await Task.Delay(10);
            double rate = 1;
            /*
            Task.Run(() =>
            {
                Task.
                return Json(ProjectVariables.CountProgress);
            });*/
            /*
            if (ProjectVariables.LengthProgress > 0 && ProjectVariables.CountProgress > 0)
                rate = (ProjectVariables.CountProgress) / (ProjectVariables.LengthProgress) * 100;

            return Json((int)rate*/
            return Json(null);

            //return await Task.Run(Ok(ProjectVariables.CountProgress));
        }

        [HttpGet("getuser")]
        public JsonResult GetUser()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (Claims != null)
            {
                var AppUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value, includeProperties: "UserTypes");
                if (AppUser.UserTypes.Name == UserTypeConst.Customer)
                {
                    var firstdef = _uow.DefinitionUser.GetFirstOrDefault(i => i.UserId == AppUser.Id);
                    var _customer = _uow.MusteriTanim.GetFirstOrDefault(i => i.id == firstdef.DefinitionId);
                    var _user = new User()
                    {
                        Email = AppUser.Email,
                        NameField = _customer.musteriAdi,
                        UserType = UserTypeConst.Customer

                    };
                    return Json(_user);
                }
                if (AppUser.UserTypes.Name == UserTypeConst.Operation_Area)
                {
                    var _operation = _uow.FabrikaTanim.GetFirstOrDefault(i => i.id == AppUser.DefinitionId);
                    var _user = new User()
                    {
                        Email = AppUser.Email,
                        NameField = _operation.fabrikaAdi,
                        UserType = UserTypeConst.Operation_Area

                    };
                    return Json(_user);
                }
            }
            User _userNull = null;
            return Json(_userNull);

        }
        public Claim GetClaim()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                return Claims;
            }
            return null;
        }

    }
}



/*
FROM qprojeDetay INNER JOIN qprojetanim ON qprojeDetay.idProje = qprojetanim.id 
INNER JOIN fabrikatanim ON qprojetanim.idOprArea = fabrikatanim.id 
INNER JOIN musteritanim ON qprojetanim.idMusteri = musteritanim.id 
INNER JOIN paraBirimi ON qprojetanim.fiyatIdParaBirimi = paraBirimi.id
INNER JOIN qprojedurumu ON qprojetanim.idProjeDurumu = qprojedurumu.id 
INNER JOIN ulke ON fabrikatanim.idUlke = ulke.id 
WHERE     (qprojeDetay.id > 0) AND(qprojeDetay.idProforma = 0)AND(qprojetanim.idOprArea in 
(2094,2086,1027,1033,2117,1076,2112,2110,1052,1036,2097,1080,1045,1029,2120,1058,1079,1040,1042,1024,1073,2118,1075,2105,
2106,2092,1028,2115,2084,1054,1034,1055,1062,2083,5,2098,12,8,1032,2100,1049,19,2099,1031,1039,2089,1083,1077,2111,3,9,2121,2107,
2104,1074,1050,1061,2085,18,2119,1053,1025,1048,2087,1019,1047,1065,2102,2103,1067,2091,2101,1071,2095,2093,1066,1038,11,1072,6,2096,
2113,7,2108,1059,1035,2090,1021,1022,1037,1082,2109,1043,1020,1064,1057,1030,1041,1026,1023,1063,1078,16,15,14,1070,1046,10,4,1081,1068,1051,
1069,1044,2116,13,1060,2114,0))AND(qprojetanim.idMusteri in (93,0))*/