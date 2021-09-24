using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Models.ProductModels
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] ImageData { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
