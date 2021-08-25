using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Models
{
    public class OpeningHour
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression("^(2[0-3]|[01]?[0-9]):([0-5]?[0-9])$", ErrorMessage = "Please enter valid hour")]
        public string Open { get; set; }

        [Required]
        [RegularExpression("^(2[0-3]|[01]?[0-9]):([0-5]?[0-9])$", ErrorMessage = "Please enter valid hour")]
        public string Close{ get; set; }

    }
}
