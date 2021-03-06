using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HallOfFame
{
    public class Person
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string DisplayName { get; set; }
        [Required]
        public IEnumerable<Skill> SkillsCollection { get; set; }
    }
}
