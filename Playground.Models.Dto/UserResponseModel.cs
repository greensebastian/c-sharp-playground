using Playground.Models.User;

namespace Playground.Models.Dto
{
    public class UserResponseModel
    {
        public UserResponseModel(PlaygroundUser user)
        {
            if (user == null) return;
            Id = user.Id;
            Username = user.UserName;
            Email = user.Email;
            ActivitySegmentCount = user.TimelineData?.ActivitySegments?.Count ?? 0;
            PlaceVisitCount = user.TimelineData?.PlaceVisits?.Count ?? 0;
        }
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int ActivitySegmentCount { get; set; }
        public int PlaceVisitCount { get; set; }
    }
}
