using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Accounts
{
    public class SignupModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least six character")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage ="Confirm Password must match password")]
        public string ConfirmPassword { get; set; }
    }
}
