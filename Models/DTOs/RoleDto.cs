namespace MindHeal.Models.DTOs
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<UserDto> Users { get; set; }
    }

    public class CreateRoleRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
