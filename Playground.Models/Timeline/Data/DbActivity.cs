namespace Playground.Models.Timeline.Data
{
    public class DbActivity
    {
        public int Id { get; set; }
        public virtual DbActivitySegment ActivitySegment { get; set; }
        public string ActivityType { get; set; }
        public decimal Probability { get; set; }
    }
}
