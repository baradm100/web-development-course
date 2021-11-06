using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using web_development_course.Common;
using web_development_course.Models.OrderModels;

namespace web_development_course.Models
{
    public enum UserLevel
    {
        Client,
        Editor,
        Admin
    }

    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        [Display(Name = "First Name")]
        [RegularExpression(Consts.NAME_REGEX, ErrorMessage = Consts.NAME_ERROR)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        [Display(Name = "Last Name")]
        [RegularExpression(Consts.NAME_REGEX, ErrorMessage = Consts.NAME_ERROR)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(4)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(Consts.PHONE_REGEX, ErrorMessage = Consts.PHONE_ERROR)]
        public string Phone{ get; set; }

        public UserLevel UserType { get; set; } = UserLevel.Client;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        public DateTime DateOfBirth { get; set; }

        public IEnumerable<Order> Orders { get; set; }

        // used for delivery addresses
        public IEnumerable<Address> Addresses { get; set; }

    }
}
