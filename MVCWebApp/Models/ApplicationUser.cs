using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace MVCWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;

        [Display(Name = "Your profile picture")]
        public byte[] ProfilePicture { get; set; }
    }
}
