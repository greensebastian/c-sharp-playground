using System.Collections.Generic;
using System.Linq;
using Playground.Models.User;

namespace Playground.Models.Dto
{
    public class RegisterUserResponseModel : UserResponseModel
    {
        public RegisterUserResponseModel(PlaygroundUser user, Dictionary<string, string> fieldErrors = null) : base(user)
        {
            FieldErrors = fieldErrors?.Select(error => new FieldErrorDescriptor(error.Key, error.Value));
        }
        public IEnumerable<FieldErrorDescriptor> FieldErrors { get; set; }
    }

    public struct FieldErrorDescriptor
    {
        public FieldErrorDescriptor(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
