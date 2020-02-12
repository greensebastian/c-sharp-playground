using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Playground.Models.Timeline.Data
{
    public class TimelineData
    {
        [Key]
        public int Id { get; set; }
        public virtual List<DbPlaceVisit> PlaceVisits { get; set; }
        public virtual List<DbActivitySegment> ActivitySegments { get; set; }
    }
}
