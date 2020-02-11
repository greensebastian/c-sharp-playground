using System.ComponentModel.DataAnnotations;

namespace Playground.Models.Timeline.Data
{
    public class DbWaypoint
    {
        [Key]
        public int Id { get; set; }
        public int LatitudeE7 { get; set; }
        public int LongitudeE7 { get; set; }
    }
}
