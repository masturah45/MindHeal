using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MindHeal.Models.Entities;

namespace MindHeal.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Issues> Issues { get; set; }
        public DbSet<Therapist> Therapists { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<TherapistIssues> TherapistIssues { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Client)
                .WithOne(c => c.User)
                .HasForeignKey<Client>(c => c.UserId); // Assuming you have a UserId property in the Client entity
            modelBuilder.Entity<User>()
               .HasOne(u => u.SuperAdmin)
               .WithOne(sa => sa.User)
               .HasForeignKey<SuperAdmin>(sa => sa.UserId);
            modelBuilder.Entity<User>()
        .HasOne(u => u.Therapist)
        .WithOne(t => t.User)
        .HasForeignKey<Therapist>(t => t.UserId); // Assuming y
        }
    }
}
