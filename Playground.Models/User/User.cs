using Microsoft.AspNetCore.Identity;

namespace Playground.Models.User
{
    public class User : IdentityUser<string>
    {
        public string PasswordSalt { get; set; }
    }
}
