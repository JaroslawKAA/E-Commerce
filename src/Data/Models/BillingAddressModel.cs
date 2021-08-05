using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Models;

namespace Codecool.CodecoolShop.Models
{
    public class BillingAddressModel 
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("First name")]
        [StringLength(100, ErrorMessage = "First name have to be longer than 1 characters.", MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last name")]
        [StringLength(100, ErrorMessage = "Last name have to be longer than 1 character.", MinimumLength = 2)]
        public string LastName { get; set; }
        public string Name => $"{FirstName} {LastName}";
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DisplayName("Phone number")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Country { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string City { get; set; }
        [Required]
        [RegularExpression( "^\\d{2}[- ]{0,1}\\d{3}$", ErrorMessage = "Wrong zipcode format.")]
        public string Zipcode { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Street { get; set; }
        [Required]
        [DisplayName("Street number")]
        public int StreetNumber { get; set; }
        [DisplayName("Home number")]
        public int? HomeNumber { get; set; }

        [ForeignKey(nameof(User))]
        public User Owner { get; set; }

        public override string ToString()
        {
            return $"{Name} {Street} {StreetNumber}/{HomeNumber}, {Zipcode} {City}, {Country}";
        }

        
    }
}