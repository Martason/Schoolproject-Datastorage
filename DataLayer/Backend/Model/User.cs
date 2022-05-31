
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Backend.Model
{
    public class User
    {
        public int Id { get; set; }
        public DateTime FirstRegisteredAt { get; set; }
        public DateTime PasswordLastChangedAt { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Foodbox> Foodboxes { get; set; }

        public UserPrivateInfo UserPrivateInfo { get; set; }



    }
}
