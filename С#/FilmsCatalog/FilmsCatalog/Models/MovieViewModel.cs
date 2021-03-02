using Microsoft.AspNetCore.Http;

namespace FilmsCatalog.Models
{
    public class MovieViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Producer { get; set; }
        public string Added { get; set; }
        public IFormFile Poster { get; set; }
    }
}