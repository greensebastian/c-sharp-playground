using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Playground.Models.Timeline.Data;
using Playground.Models.User;

namespace Playground.Repository.Data
{
    public class PlaygroundDatabaseContext : IdentityDbContext<PlaygroundUser, Role, string>
    {
        public DbSet<DbActivity> Activities { get; set; }
        public DbSet<DbActivitySegment> ActivitySegments { get; set; }
        public DbSet<DbLocation> Locations { get; set; }
        public DbSet<DbLocationVisit> LocationVisits { get; set; }
        public DbSet<DbPlaceVisit> PlaceVisits { get; set; }
        public DbSet<DbWaypoint> Waypoints { get; set; }
        public DbSet<TimelineData> TimelineData { get; set; }
        public PlaygroundDatabaseContext(DbContextOptions<PlaygroundDatabaseContext> options) : base(options)
        {
        }
    }
}
