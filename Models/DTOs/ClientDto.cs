using IdentityServer4.Models;
using MindHeal.Models.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MindHeal.Models.DTOs
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string State { get; set; }
    }

    public class CreateClientRequestModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Your {0} is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "{0} should be between 8 to 20 Char")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Your {0} is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "{0} should be between 8 to 20 Char")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Your {0} is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "{0} should be between 8 to 20 Char")]
        [Display(Name = "State")]
        public string State { get; set; }
    }

   public class UpdateClientRequestModel
   {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string State { get; set; }
    }
}
