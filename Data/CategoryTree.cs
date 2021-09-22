using System;
using System.Collections.Generic;
using web_development_course.Models;

namespace web_development_course.Data
{
    public class CategoryTree
    {
        public Category Category { get; set; }

        public List<Category> SubCategories { get; set; }
    }
}
