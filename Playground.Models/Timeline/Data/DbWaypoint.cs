using System.ComponentModel.DataAnnotations;

namespace Playground.Models.Timeline.Data
{
    public class DbWaypoint
    {
        [Key]
        public int Id { get; set; }
        public int LatitudeE7 { get; set; }
        public int LongitudeE7 { get; set; }

        public override int GetHashCode()
        {
            return (LatitudeE7.ToString() + LongitudeE7.ToString()).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var waypoint = obj as DbWaypoint;
            return waypoint != null && waypoint.LatitudeE7 == LatitudeE7 && waypoint.LongitudeE7 == LongitudeE7;
        }
    }
}
