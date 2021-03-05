using System;
using System.ComponentModel.DataAnnotations;

namespace domain
{
    public class Skill
    {
        //private byte level;
        public long Id { get; set; }
        //[Key]
        public string Name { get; set; }
        [Range(1, 10)]
        public byte Level { get; set; }
        public long PersonId { get; set; }
        //public byte Level
        //{
        //    get => level;
        //    set
        //    {
        //        if (value <= 10 && value >= 0)
        //            level = value;
        //        else
        //            throw new ArgumentException(nameof(Level));
        //    }
        //}

        //public Skill(int id, string name, byte level)
        //{
        //    Id = id;
        //    Name = name;
        //    Level = level;
        //}
    }
}
