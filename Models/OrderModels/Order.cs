using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Models.OrderModels
{
    public class Order
    {
        public int OrderID { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
