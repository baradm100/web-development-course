using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using web_development_course.Common;
using web_development_course.Models.ProductModels;

namespace web_development_course.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0,100,ErrorMessage = Consts.DiscountPercentageErrorMessage)]
        public float DiscountPercentage { get; set; } = 0;

        // TODO: check if it should be a REQUIRED attr.
        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<ProductType> ProductTypes { get; set; }

        public IEnumerable<ProductImage> ProductImages { get; set; }

    }
}
