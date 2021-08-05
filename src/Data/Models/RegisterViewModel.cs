using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class RegisterViewModel
    {
        [Required, MaxLength(256)] public string Username { get; set; }

        [Required, EmailAddress] public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}