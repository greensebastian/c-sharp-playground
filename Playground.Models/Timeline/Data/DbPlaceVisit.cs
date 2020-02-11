using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Playground.Models.Timeline.Data
{
    public class DbPlaceVisit
    {
        [Key]
        public int Id { get; set; }
        public virtual TimelineData TimelineData { get; set; }
        public string Hash { get; set; }
        public virtual DbLocationVisit LocationVisit { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Confidence { get; set; }
        public int CenterLatE7 { get; set; }
        public int CenterLngE7 { get; set; }
        public virtual List<DbPlaceVisit> ChildVisits { get; set; }
    }
}
