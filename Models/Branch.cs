using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_development_course.Models
{
    public class Branch
    {
        public int Id { get; set; }

        public int AddressId { get; set; }

        [Required]
        public Address Address { get; set; }

        [Required]
        public OpeningHour[] OpeningHours { get; set; }
    }
}
