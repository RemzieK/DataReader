using DataReader.Domain.Entities;


namespace DataReader.Infrastructure.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int RoleId { get; set; }
        public string? Role { get; set; }
        public bool IsDeleted { get; set; }

        public Role UserRole { get; set; }
    }
}
