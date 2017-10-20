using Microsoft.EntityFrameworkCore;
using QompanyVKApp.Models;

namespace QompanyVKApp
{
    public class QompanyDbContext : DbContext
    {
        public QompanyDbContext(DbContextOptions<QompanyDbContext> options) : base(options)
        {}

        public DbSet<MeetingRoom> MeetingRooms { get; set; }
    }
}