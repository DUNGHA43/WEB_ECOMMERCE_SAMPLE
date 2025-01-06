﻿using System.ComponentModel.DataAnnotations;

namespace WebEcommerce.Models
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "The First Name field is required"), MaxLength(100)]
        public string firstName { get; set; }
        
        [Required(ErrorMessage = "The Last Name field is required"), MaxLength(100)]
        public string lastName { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string email { get; set; }

        [Phone(ErrorMessage = "The format of the phone number is not valid"), MaxLength(20)]
        public string? phoneNumber { get; set; }

        [Required, MaxLength(200)]
        public string address { get; set; }

        [Required, MaxLength(100)]
        public string password { get; set; }

        [Required(ErrorMessage = "The comfirm password field is required")]
        [Compare("password", ErrorMessage = "Comfirm password and password do not match")]
        public string comfirmPassword { get; set; }
    }
}
