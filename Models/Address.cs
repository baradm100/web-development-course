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
        [Display(Name = "Building Number")]
        [Range(Consts.FIRST_BUILDING_NUM, Consts.MAX_BUILDING_NUM, ErrorMessage = Consts.ONLY_DIGITS_ERROR)]
        public int BuildingNumber { get; set; }

        // Collection of users used this address for delivery
        public IEnumerable<User> Users{ get; set; }
      
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public override string ToString()
        {
            return Street + " St." + BuildingNumber + " " + City; 
        }
    }
}
