using domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Person>(action =>
            //{
            //    action.HasData(
            //        new Person
            //        {
            //            Id = 1,
            //            Name = "Alexandr",
            //            DisplayName = "Alex",
            //            SkillsCollection = new Skill[]
            //            {
            //                new Skill
            //                {
            //                    //Id = 1,
            //                    Name = "agility",
            //                    Level = 9,
            //                    //PersonId = 1
            //                },
            //                new Skill
            //                {
            //                    //Id = 1,
            //                    Name = "strength",
            //                    Level = 5,
            //                    //PersonId = 1
            //                },
            //                new Skill
            //                {
            //                    //Id = 1,
            //                    Name = "intellegence",
            //                    Level = 3,
            //                    //PersonId = 1
            //                }
            //            }

            //        });
            //});
        }
    }
}
