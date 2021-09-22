using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using web_development_course.Common;

namespace web_development_course.Models.OrderModels
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId{ get; set; }

        [Required]
        public Order Order { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        [Display(Name = "total price")]
        [Range(0,Consts.MaxProductsTotalPrice,ErrorMessage = Consts.ProductTotalPriceErrorMessage)]
        public int TotalPrice { get; set; }
        
        public int ProductTypeID { get; set; }

        [Required]
        public ProductType ProductType { get; set; }
    }
}
