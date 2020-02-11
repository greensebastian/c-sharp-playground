using System.ComponentModel.DataAnnotations.Schema;

namespace Playground.Models.Timeline.Data
{
    public class DbActivity
    {
        public int Id { get; set; }
        public virtual DbActivitySegment ActivitySegment { get; set; }
        public string ActivityType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Probability { get; set; }
    }
}
