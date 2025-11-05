namespace PAIFGAMES.FCG.Api.Helpers.Auth.Models
{
    public class UserData
    {
        public User User { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();

    }

    public class Role
    {
        public string RoleName { get; set; }
        public string RoleUId { get; set; }
    }
}
