using System;
using System.ComponentModel.DataAnnotations;

namespace SportWorld.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }

        public Gender Gender { get; set; }

        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsDeleted { get; set; }
    }
}