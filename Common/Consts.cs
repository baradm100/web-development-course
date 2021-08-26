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
        public const int MaxProductsTotalPrice = 10000;

        // Error messages
        public const string DiscountPercentageErrorMessage = "The value should be between 0 - 100";
        public const string ProductTypeQuantityErrorMessage = "quantity must be between 0 and 100000";
        public const string ProductTotalPriceErrorMessage = "the price must be a value between 0 and 10000";
    }
}
