using System.ComponentModel.DataAnnotations;

namespace Playground.Models.Timeline.Data
{
    public class DbLocation
    {
        [Key]
        public int Id { get; set; }
        public int LatitudeE7 { get; set; }
        public int LongitudeE7 { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string PlaceId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DbLocation && obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return PlaceId?.GetHashCode() ?? base.GetHashCode();
        }
    }
}
