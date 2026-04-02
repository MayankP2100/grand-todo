using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace dotnet_backend.Modules.Users.Entities
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
    }
}
