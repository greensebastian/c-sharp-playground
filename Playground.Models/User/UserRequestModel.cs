using System.ComponentModel.DataAnnotations;

namespace Playground.Models.User
{
    public class UserRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
