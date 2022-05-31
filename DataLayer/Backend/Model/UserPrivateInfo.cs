using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Backend.Model
{
    public class UserPrivateInfo
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(15)]
        public string Username { get; set; }
        [Required]
        [MaxLength(15)]
        public string Password { get; set; }

        [Required]
        [ForeignKey("Id")] public User User { get; set; }

    }
}
