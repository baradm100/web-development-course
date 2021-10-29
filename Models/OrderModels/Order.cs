using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Models.OrderModels
{


    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public bool IsCart { get; set; } = true;

        public DeliveryOptions Delivery { get; set; } = DeliveryOptions.Nearest_Store;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        public IEnumerable<OrderItem> OrderItems { get; set; }

    }
}
