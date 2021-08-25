using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        public string Street { get; set; }

        [Required]
        [RegularExpression("([1-9][0-9]*)"), ErrorMessage = "Characters are not allowed.")]
        public int BuildingNumber { get; set; }
    }
}
