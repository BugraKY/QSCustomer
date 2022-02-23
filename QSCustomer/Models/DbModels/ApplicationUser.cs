using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Models.DbModels
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("UserTypeId")]
        public int UserTypeId { get; set; }
        public UserType UserTypes { get; set; }
        public int DefinitionId { get; set; }
        public bool Status { get; set; }
    }
}
