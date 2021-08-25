using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Common
{
    public static class Consts
    {
        // General Consts
        public const int MaxProductsQuantity = 100000;

        // Error messages
        public const string DiscountPercentageErrorMessage = "The value should be between 0 - 100";
        public const string ProductTypeQuantityErrorMessage = "quantity must be between min and max";
    }
}
