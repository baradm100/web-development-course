using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using web_development_course.Data;

namespace web_development_course.Models.ProductModels
{
    public class CategoryModelService
    {
        private readonly ApplicationDbContext _context;


        public CategoryModelService(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public List<CategoryTree> GetCategoryTrees()
        {
            var mainCatagories = _context.Category.Where(c => c.ParentCategoryId == null).ToList<Category>();
            List<CategoryTree> categoryTrees = new List<CategoryTree>();

            foreach (Category mainCategory in mainCatagories)
            {
                List<Category> subCatagories = _context.Category.Where(c => c.ParentCategoryId == mainCategory.Id).ToList<Category>();
                categoryTrees.Add(new CategoryTree { Category = mainCategory, SubCategories = subCatagories });
            }


            return categoryTrees;
        }
    }
}
