using System.ComponentModel.DataAnnotations;

namespace Playground.Models.User
{
    public class RegisterUserRequestModel : UserRequestModel
    {
        [Required]
        public string RegistrationKey { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool SignIn { get; set; } = true;
    }
}
