using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Producer { get; set; }
        public string Added { get; set; }
        public string Poster { get; set; }
        //public byte[] Poster { get; set; }
    }
}
