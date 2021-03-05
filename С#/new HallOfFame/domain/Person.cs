using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace domain
{
    public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<Skill> SkillsCollection {get;set;}
    }

}
