using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame.Models
{
    public class Person
    {
        public int id { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public Skill skills { get; set; }
    //public List<Skill> skills { get; set; }
    //public int SkillId { get; set; }
}
}