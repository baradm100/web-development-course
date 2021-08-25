using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Models.OrderModels
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }

        [Required]
        public Order Order { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public int TotalPrice { get; set; }
        
        //TODO: Validate
        public int ProductTypeID { get; set; }

        [Required]
        public ProductType ProductType { get; set; }
    }
}
