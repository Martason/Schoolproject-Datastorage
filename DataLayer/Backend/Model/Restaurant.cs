using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Backend.Model
{
    public class Restaurant : UserPrivateInfo
    {

        [Required]
        [MaxLength(20)]
        public string CompanyName { get; set; }

        public string Type { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Organisationsnummer { get; set; }

    }
}
