﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Models.ProductModels
{
    public class ProductCategory
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}