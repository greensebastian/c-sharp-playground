using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Playground.Models.Timeline.Data
{
    public class DbLocationVisit
    {
        [Key]
        public int Id { get; set; }
        public string SemanticType { get; set; }
        public virtual DbLocation Location { get; set; }

    }
}
