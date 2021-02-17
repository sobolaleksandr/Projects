using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HallOfFame.Models
{
    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public List<Skill> skills { get; set; }
    }
}