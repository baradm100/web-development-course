using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public IEnumerable<ProductCategory> ProductCategories { get; set; } = new HashSet<ProductCategory>();

        public IEnumerable<ProductType> ProductTypes { get; set; }

        public IEnumerable<ProductImage> ProductImages { get; set; }

    }
}
