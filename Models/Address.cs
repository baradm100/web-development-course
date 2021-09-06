using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using web_development_course.Common;

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
        [Range(Consts.FIRST_BUILDING_NUM, Consts.MAX_BUILDING_NUM, ErrorMessage = Consts.ONLY_DIGITS_ERROR)]
        public int BuildingNumber { get; set; }

        public float Longitude { get; set; }

        public float Latitude { get; set; }


    }
}
