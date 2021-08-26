﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using web_development_course.Common;

namespace web_development_course.Models
{
    public class ProductType
    {
        public int Id { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public int Size { get; set; }

        [Range(0,Consts.MaxProductsQuantity,ErrorMessage = Consts.ProductTypeQuantityErrorMessage)]
        public int Quantity { get; set; } = 0;

        [Required]
        public Color Color { get; set; }

        public byte[] Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}