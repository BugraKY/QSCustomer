using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace QSCustomer.Models.DbModels
{

    public class DefinitionUsers
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DefinitionId { get; set; }
    }
}
