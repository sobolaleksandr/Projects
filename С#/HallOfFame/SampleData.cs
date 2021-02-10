using System.Linq;
using HallOfFame.Models;

namespace HallOfFame
{
    public static class SampleData
    {
        public static void Initialize(HOFContext context)
        {
            if (!context.Skills.Any())
            {
                context.Skills.AddRange(
                    new Skill
                    {
                        name = "Agility",
                        level = 5
                    },
                    new Skill
                    {
                        name = "Strength",
                        level = 9
                    },
                    new Skill
                    {
                        name = "Intellegence",
                        level = 2
                    }
                );
                context.SaveChanges();
            }
        }
    }
}