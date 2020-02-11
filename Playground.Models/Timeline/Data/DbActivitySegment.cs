using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Playground.Models.Timeline.Data
{
    public class DbActivitySegment
    {
        [Key]
        public int Id { get; set; }
        public virtual TimelineData TimelineData { get; set; }
        public string Hash { get; set; }
        public virtual DbWaypoint StartWaypoint { get; set; }
        public virtual DbWaypoint EndWaypoint { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Confidence { get; set; }
        public string ActivityType { get; set; }
        public int Distance { get; set; }
        public virtual List<DbWaypoint> Waypoints { get; set; }
        public virtual List<DbLocationVisit> TransitLocationVisits { get; set; }
    }
}
