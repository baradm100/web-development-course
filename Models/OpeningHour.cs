using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using web_development_course.Common;

namespace web_development_course.Models
{
    public class OpeningHour
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(Consts.HOUR24_REGEX, ErrorMessage = Consts.HOUR_VIOLATION_ERROR)]
        public string Open { get; set; }

        [Required]
        [RegularExpression(Consts.HOUR24_REGEX, ErrorMessage = Consts.HOUR_VIOLATION_ERROR)]
        public string Close{ get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }
    }
}
