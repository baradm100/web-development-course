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
            
        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string Name { get; set; } = "Default Name";

        public int AddressId { get; set; }

        [Required]
        public Address Address { get; set; }

        [Required]
        public IEnumerable<OpeningHour> OpeningHours { get; set; }

    }
}
