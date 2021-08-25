﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
