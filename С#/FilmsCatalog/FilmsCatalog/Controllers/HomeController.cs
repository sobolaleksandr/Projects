using FilmsCatalog.Data;
using FilmsCatalog.Models;
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
        ApplicationDbContext db;
        private readonly SignInManager<User> _signInManager;

        public HomeController(
            SignInManager<User> signInManager,
            ApplicationDbContext context)
        {
            db = context;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                Movie movie = await db.Movies.FirstOrDefaultAsync(p => p.ID == id);
                if (movie != null)
                    return View(movie);
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (id != null)
                {
                    Movie movie = await db.Movies.FirstOrDefaultAsync(p => p.ID == id);
                    if (movie != null)
                    {
                        if (movie.Added == User.Identity.Name)
                        {
                            return View(movie);
                        }
                        return RedirectToAction(nameof(BadLogin));
                    }
                }
                return NotFound();
            }
            return RedirectToAction(nameof(BadLogin));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MovieViewModel mvm)
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

            if (mvm.Poster != null)
            {
                movie.Poster = FormatImage(mvm.Poster);
            }

            db.Movies.Update(movie);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return View();
            }
            return RedirectToAction(nameof(BadLogin));
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieViewModel mvm)
        {
            Movie movie = new Movie
            {
                Title = mvm.Title,
                Description = mvm.Description,
                Year = mvm.Year,
                Producer = mvm.Producer,
                Added = User.Identity.Name
            };

            if (mvm.Poster != null)
            {
                movie.Poster = FormatImage(mvm.Poster);
            }

            db.Movies.Add(movie);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

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
            IQueryable<Movie> source = db.Movies;
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
