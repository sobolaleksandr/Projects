using FilmsCatalog.Data;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _appEnvironment;

        public HomeController(
            SignInManager<User> signInManager,
            ApplicationDbContext context,
            IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _signInManager = signInManager;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                Movie movie = await _context.Movies.FirstOrDefaultAsync(p => p.ID == id);
                if (movie != null)
                    return View(movie);
            }
            return NotFound();
        }

        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (id != null)
                {
                    Movie movie = await _context.Movies.FirstOrDefaultAsync(p => p.ID == id);
                    if (movie != null)
                    {
                        if (movie.Added == User.Identity.Name)
                        {
                            return View(movie);
                        }
                        return RedirectToAction(nameof(BadLogin));
                    }
                }

                return View();
            }
            return RedirectToAction(nameof(BadLogin));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(MovieViewModel mvm)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Movie movie = new Movie
                {
                    ID = mvm.ID,
                    Title = mvm.Title,
                    Description = mvm.Description,
                    Year = mvm.Year,
                    Producer = mvm.Producer,
                    Added = User.Identity.Name
                };

                movie.Poster = await SaveFileReturnName(mvm.Poster);

                //if (mvm.Poster != null)
                //        movie.Poster = FormatImage(mvm.Poster);

                if (mvm.ID == 0)
                {
                    _context.Movies.Add(movie);
                }
                else
                {
                    _context.Movies.Update(movie);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction(nameof(BadLogin));
        }
        //Сохранение в файловой системе для string Poster
        private async Task<string> SaveFileReturnName(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                if (IsImage(uploadedFile, 1500000, resolutions))
                {
                    // путь к папке Files
                    string path = "/Files/" + uploadedFile.FileName;
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

        List<string> resolutions = new List<string>
        {
            "image/jpeg","image/bmp","image/jpg","image/gif","image/png","image/png"
        };

        private bool IsImage(IFormFile uploadedFile, long sizeBytes, List<string> resoulutions)
        {
            if (resolutions.Contains(uploadedFile.ContentType) &&                
                uploadedFile.Length < sizeBytes)
                return true;
            else
                return false;
        }

        //Выгрузка в БД для byte[] Poster
        private byte[] FormatImage(IFormFile poster)
        {
            byte[] imageData = null;
            // считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(poster.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)poster.Length);
            }
            // установка массива байтов
            return imageData;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;
            IQueryable<Movie> source = _context.Movies;
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Movies = items
            };
            return View(viewModel);
        }

        public IActionResult BadLogin()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
