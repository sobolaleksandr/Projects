using System.ComponentModel.DataAnnotations;

namespace HallOfFame
{
    public class Skill
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1, 10)]
        public byte Level { get; set; }
        [Required]
        public long PersonId { get; set; }
    }
}
