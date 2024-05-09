namespace UserService.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<FollowedUser> Followers { get; set; } = new List<FollowedUser>();
        public ICollection<FollowedUser> Following { get; set; } = new List<FollowedUser>();
    }
    
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}