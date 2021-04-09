using Microsoft.AspNetCore.Identity;

namespace AutorizeServiceApi.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public const string SuperAdminUserName = "admin";
        public const string SuperAdminEmail = "admin@admin.ru";
        public const string SuperAdminDefaultPassword = "admin";

        public const string RoleSuperAdmin = "Super Administrator";
        public const string RoleAdmin = "Administrator";
        public const string RoleUser = "User";

    }
}
