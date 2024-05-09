using Microsoft.EntityFrameworkCore;
using UserService.Model;

namespace UserService
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<FollowedUser> FollowedUsers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FollowedUser>()
                .HasKey(f => new { f.FollowerId, f.FollowedId });

            modelBuilder.Entity<FollowedUser>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FollowedUser>()
                .HasOne(f => f.Followed)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}