using Domain.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Repository.Context
{
    public class ActivityDbContext : DbContext
    {
        public DbSet<Activity> Activitys { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<ActivityTechnology> ActivityTechnologies { get; set; }
        public DbSet<TimeActivity> TimeActivities { get; set; }
        public DbSet<User> Users { get; set; }

        public ActivityDbContext(DbContextOptions<ActivityDbContext> options) : base(options)
        {
        }
    }
}
