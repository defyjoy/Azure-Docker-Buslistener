using Microsoft.EntityFrameworkCore;
using Sync.Publisher.Models;

namespace Sync.Publisher
{
    public class SyncContext : DbContext
    {
        private string ConnectionString;

        public SyncContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Member>().HasMany(x => x.Contacts).WithOne(x => x.Member).HasForeignKey(x => x.MemberId);
            builder.Entity<Member>().HasKey(p => new { p.Id });
            builder.Entity<Contact>().HasKey(p => new { p.Id });
            base.OnModelCreating(builder);
        }
    }
}
