using Microsoft.EntityFrameworkCore;
using QompanyVKApp.Models;

namespace QompanyVKApp.Data
{
    public class QompanyDbContext : DbContext
    {
        public QompanyDbContext(DbContextOptions<QompanyDbContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EmployeeMeeting>()
                .HasKey(t => new {t.EmployeeId, t.MeetingId});

            modelBuilder.Entity<EmployeeMeeting>()
                .HasOne(em => em.Meeting)
                .WithMany("EmployeeMeetings")
                .HasForeignKey(em => em.MeetingId);

            modelBuilder.Entity<EmployeeMeeting>()
                .HasOne(em => em.Employee)
                .WithMany("EmployeeMeetings")
                .HasForeignKey(em => em.EmployeeId);

            modelBuilder.Entity<MeetingRoom>()
                .HasMany(c => c.Meetings)
                .WithOne(e => e.MeetingRoom)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Employees)
                .WithOne(e => e.Group);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.MeetingRooms)
                .WithOne(e => e.Group);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Meetings)
                .WithOne(e => e.Group);


        }

        public DbSet<MeetingRoom> MeetingRooms { get; set; }
        public DbSet<EmployeeMeeting> EmployeeMeetings { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<QompanyVKApp.Models.Group> Groups { get; set; }
        

    }
}