using System;
using System.ComponentModel.DataAnnotations;

namespace Honeymustard
{
    public class UserModel
    {
        public string ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}