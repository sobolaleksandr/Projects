using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FilmsCatalog.Services
{
    public class ImageSaver : IImageSaver
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private const long MaxSizeInBytes = 1500000;

        readonly List<string> resolutions = new()
        {
            "image/jpeg","image/bmp","image/jpg","image/gif","image/png","image/png"
        };

        public ImageSaver(
              IWebHostEnvironment appEnvironment
            )
        {
            _appEnvironment = appEnvironment;
        }

        public async Task<string> SaveFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                if (IsImage(uploadedFile, MaxSizeInBytes))
                {
                    string path = "/Files/" + uploadedFile.FileName; // путь к папке Files

                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }

                    return path;
                }
            }
            return null;
        }

        private bool IsImage(IFormFile uploadedFile, long sizeBytes)
        {
            if (resolutions.Contains(uploadedFile.ContentType) &&
                uploadedFile.Length < sizeBytes)
                return true;
            else
                return false;
        }
    }
}
