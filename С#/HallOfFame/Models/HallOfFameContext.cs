using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Models
{
    public class HallOfFameContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public HallOfFameContext(DbContextOptions<HallOfFameContext> options)
            : base(options)
        {
        }
    }
}