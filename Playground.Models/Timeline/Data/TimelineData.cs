using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Playground.Models.User;

namespace Playground.Models.Timeline.Data
{
    public class TimelineData
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("PlaygroundUserForeignKey")]
        public virtual PlaygroundUser PlaygroundUser { get; set; }
        public virtual List<DbPlaceVisit> PlaceVisits { get; set; }
        public virtual List<DbActivitySegment> ActivitySegments { get; set; }
    }
}
