using Microsoft.AspNetCore.Identity;
using Playground.Models.Timeline.Data;

namespace Playground.Models.User
{
    public class PlaygroundUser : IdentityUser<string>
    {
        [PersonalData]
        public virtual TimelineData TimelineData { get; set; }
        public string PasswordSalt { get; set; }
    }
}
