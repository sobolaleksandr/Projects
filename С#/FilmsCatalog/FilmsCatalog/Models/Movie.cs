using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Models
{
    public class Movie
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]        
        public DateTime? Year { get; set; }

        [Required]
        public string Producer { get; set; }

        public string Added { get; set; }

        public string Poster { get; set; }
    }
}
