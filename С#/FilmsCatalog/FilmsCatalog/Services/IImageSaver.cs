using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FilmsCatalog.Services
{
    public interface IImageSaver
    {
        Task<string> SaveFile(IFormFile name);
    }
}
