using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Backend.Model
{
    public class Foodbox
    {
        [Key]
        public int Id { get; set; }
        public string DishName { get; set; }
        public decimal PriceKr { get; set; }
        public string Category { get; set; }
        public DateTime ExpirationDate { get; set; }

        public  Order Order { get; set; }
        public  Restaurant Restaurant { get; set; }


    }
}
