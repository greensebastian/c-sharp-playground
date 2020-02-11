using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Playground.Models.Timeline.Data
{
    public class DbActivitySegment
    {
        [Key]
        public int Id { get; set; }
        public int Hash { get; set; }
        public virtual DbWaypoint StartWaypoint { get; set; }
        public virtual DbWaypoint EndWaypoint { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Confidence { get; set; }
        public string ActivityType { get; set; }
        public int Distance { get; set; }
        public virtual List<DbWaypoint> Waypoints { get; set; }
        public virtual List<DbLocationVisit> TransitLocationVisits { get; set; }

        public override int GetHashCode()
        {
            var dateString = StartDateTime.ToString(CultureInfo.InvariantCulture) + EndDateTime.ToString(CultureInfo.InvariantCulture);
            return dateString.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var activitySegment = obj as DbActivitySegment;
            return activitySegment != null && activitySegment.GetHashCode() == GetHashCode();
        }
    }
}
