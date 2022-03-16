using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.ViewModels
{
    public class ProjectList
    {
        public int Id { get; set; }
        public string OperationArea { get; set; }
        public string CustomerName { get; set; }
        public string ProjectCode { get; set; }
        public string State { get; set; }
        public string ApprovalState { get; set; }
        public string? StartDate { get; set; }
        public string? FinishDate { get; set; }
        public string? ApprovalDate { get; set; }
        public string Responsible { get; set; }
        public string ControlType { get; set; }
        public string CompNr { get; set; }
        public string Material { get; set; }
        public string Note { get; set; }
        public string Currency { get; set; }
        public string ProductRefs { get; set; }
        public string NokDescs { get; set; }
    }
}
