using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class LoginViewModel
    {
        [Required, MaxLength(256)]
        public string Username { get; set; }

        [Required] 
        [DataType(DataType.Password)] public string Password { get; set; }

        [Display(Name = "Remember Me")] public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}