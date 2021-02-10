using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Models
{
    public class HOFContext : DbContext
    {
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Person> Person { get; set; }

        public HOFContext(DbContextOptions<HOFContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
