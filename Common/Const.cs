using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Common
{
    public static class Consts
    {
        #region Address

        public const int FIRST_BUILDING_NUM = 1;

        public const int MAX_BUILDING_NUM = 10000;

        # endregion

        #region Errors 

        public const string ONLY_DIGITS_ERROR = "Characters are not allowed.";

        public const string HOUR_VIOLATION_ERROR = "Please enter valid hour";

        #endregion

        #region Regex

        public const string HOUR24_REGEX = "^(2[0 - 3]|[01]?[0 - 9]):([0 - 5]?[0 - 9])$";

        # endregion
    }
}
