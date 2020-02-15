using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Playground.Models.Timeline.Data
{
    public class DbPlaceVisit
    {
        [Key]
        public int Id { get; set; }
        public int Hash { get; set; }
        public virtual DbLocationVisit LocationVisit { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Confidence { get; set; }
        public int CenterLatE7 { get; set; }
        public int CenterLngE7 { get; set; }
        public virtual List<DbPlaceVisit> ChildVisits { get; set; }

        public override int GetHashCode()
        {
            var dateString = LocationVisit.Location.PlaceId + StartDateTime.ToString(CultureInfo.InvariantCulture) + EndDateTime.ToString(CultureInfo.InvariantCulture);
            return dateString.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var placeVisit = obj as DbPlaceVisit;
            return placeVisit != null && placeVisit.GetHashCode() == GetHashCode();
        }
    }
}
