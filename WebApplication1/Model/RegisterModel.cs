﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class RegisterModel
    {
        [Required]
        public string?Name { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string ?Password { get; set; }

        public string ?MobileNo { get; set; }  // Add if needed
    }
}
