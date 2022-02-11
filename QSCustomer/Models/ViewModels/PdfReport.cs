using QSCustomer.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.ViewModels
{
    public class PdfReport
    {
        public List<ProjectTotalsOneByDate> _ProjectTotalsOnebyDate { get; set; }
        public qprojetanim _SelectedProject { get; set; }
        public musteritanim _Customer { get; set; }
        public fabrikatanim _Operation { get; set; }
        public List<qprojepartNrTanimi> _PartNrTanimlari { get; set; }
        public List<qprojehataTanimi> _ProjeHataTanim { get; set; }
        public List<ProjectDetails> _ProjectDetails { get; set; }
        public ProjectState _ProjectState { get; set; }
        public ProjectFilter _ProjectFilter { get; set; }
        public string _ProjectCode { get; set; }
        public int _ProjeHataTanimCount { get; set; }
        public double _CheckedTotal { get; set; }
        public double _ReworkedTotal { get; set; }
        public double _NokTotal { get; set; }
        public double _SpentHoursTotal { get; set; }
        public double _SpentHours { get; set; }
        public double _OverTime50Total { get; set; }
        public double _OverTime100Total { get; set; }
        public int _PPMTotal { get; set; }

        public string _DataFields { get; set; }
        public string _HtmlString { get; set; }
        public string _Url { get; set; }

    }
}
