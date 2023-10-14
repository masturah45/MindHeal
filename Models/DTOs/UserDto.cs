using MindHeal.Models.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace MindHeal.Models.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public Gender Gender { get; set; }
        public IList<RoleDto> Roles { get; set; } = new List<RoleDto>();

    }

    public class LogInUserRequestModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Password { get; set; }
    }
}
