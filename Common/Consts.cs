using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Common
{
    public static class Consts
    {

        #region Product

        public const int MaxProductsTotalPrice = 10000;
        public const int MaxProductsQuantity = 100000;
        // Error messages
        public const string DiscountPercentageErrorMessage = "The value should be between 0 - 100";
        public const string ProductTypeQuantityErrorMessage = "quantity must be between 0 and 100000";
        public const string ProductTotalPriceErrorMessage = "the price must be a value between 0 and 10000";
        public const string ProductImageMissingError = "You must upload at least one image";
        public const string ProductExsistError = "This product is already exist";


        # endregion


        #region Address

        public const int FIRST_BUILDING_NUM = 1;

        public const int MAX_BUILDING_NUM = 1000;

        # endregion

        #region Errors 

        public const string ONLY_DIGITS_ERROR = "Characters are not allowed.";

        public const string HOUR_VIOLATION_ERROR = "Please enter valid hour";

        public const string ITEM_NOT_FOUND = "Item was not found";

        public const string NOT_IN_STOCK = "Not enough item in stock";

        public const string PHONE_ERROR = "Please enter valid phone number";

        public const string NAME_ERROR = "Please write your name with letters only";

        #endregion

        #region Regex

        public const string HOUR24_REGEX = "^(?:[01][0-9]|2[0-3]):[0-5][0-9]$";

        public const string PHONE_REGEX = "[0]{1}[0-9]{8,9}";

        public const string NAME_REGEX = "^[a-zA-Z]+$";

        # endregion
    }
}
