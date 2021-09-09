using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Models
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public int? ParentCategoryId { get; set; }

        public Category ParentCategory { get; set; }
    }
}
