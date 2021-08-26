﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public UserLevel UserType { get; set; } = UserLevel.Client;

        [Required]
        [DataType(DataType.Date)]
        public int DateOfBirth { get; set; }

        public Order OrderID { get; set; }

        public Order Cart { get; set; }

        public IEnumerable<Order> Orders { get; set; }

    }
}
