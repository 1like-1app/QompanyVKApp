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
                .WithMany(m => m.EmployeeMeetings)
                .HasForeignKey(em => em.MeetingId);

            modelBuilder.Entity<EmployeeMeeting>()
                .HasOne(em => em.Employee)
                .WithMany(e => e.EmployeeMeetings)
                .HasForeignKey(em => em.EmployeeId);

            modelBuilder.Entity<MeetingRoom>()
                .HasMany(c => c.Meetings)
                .WithOne(e => e.MeetingRoom)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<MeetingRoom> MeetingRooms { get; set; }
        public DbSet<EmployeeMeeting> EmployeeMeetings { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        

    }
}