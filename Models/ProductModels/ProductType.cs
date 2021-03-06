using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using web_development_course.Common;
using web_development_course.Models.ProductModels;

namespace web_development_course.Models
{
    public enum ProductSize
    {
        XS,
        S,
        M,
        L,
        XL,
        XXL
    }

    public class ProductType
    {
        public int Id { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public ProductSize Size { get; set; }

        [Range(0, Consts.MaxProductsQuantity, ErrorMessage = Consts.ProductTypeQuantityErrorMessage)]
        public int Quantity { get; set; } = 0;

        [Required]
        public int ColorId { get; set; }
        public ProductColor Color { get; set; }

    }
}
