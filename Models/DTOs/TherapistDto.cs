using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MindHeal.Models.Entities;
using MindHeal.Models.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MindHeal.Models.DTOs
{
    public class TherapistDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Certificate { get; set; }
        public string Credential { get; set; }
        public string RegNo { get; set; }
        public string ProfilePicture { get; set; }
        public string UserId { get; set; }
        public bool IsAvailable { get; set; }
        public Gender Gender { get; set; }
        public string Description { get; set; }
        public List<Guid> IssueIds { get; set; }
        public IList<TherapistIssues > TherapistIssues { get; set; }
        public SelectList Issues { get; set; }

    }

    public class CreateTherapistRequestModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Your {0} is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "{0} should be between 5 to 20 Char")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Your {0} is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "{0} should be between 5 to 20 Char")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Your {0} is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0} should be between 2 to 20 Char")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "RegNo")]
        public string RegNo { get; set; }
        public IFormFile? Certificate { get; set; }
        public IFormFile? Credential { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        [Required(ErrorMessage = "Your {0} is required")]
        [StringLength(60, MinimumLength = 4, ErrorMessage = "{0} should be between 4 to 60 Char")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public Gender Gender { get; set; }
        public List<Guid>? IssueIds { get; set; }
        public SelectList? Issues { get; set; }
    }

    public class UpdateTherapistRequestModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public string Description { get; set; }
        public List<Guid>? IssueIds { get; set; }
        public SelectList? Issues { get; set; }

    }
}
