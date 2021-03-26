using FilmsCatalog.Data;
using FilmsCatalog.Services;
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
        private readonly IImageSaver _imageSaver;

        public HomeController(
            SignInManager<User> signInManager,
            ApplicationDbContext context,
            IImageSaver imageSaver
            )
        {
            _context = context;
            _signInManager = signInManager;
            _imageSaver = imageSaver;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                Movie movie = await _context.Movies.FirstOrDefaultAsync(p => p.ID == id.Value);

                if (movie != null)
                    return View(movie);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (!id.HasValue)
                {
                    Movie movie = await _context.Movies.FirstOrDefaultAsync(p => p.ID == id.Value);
                    if (movie != null)
                    {
                        if (movie.Added == User.Identity.Name)
                            return View(movie);

                        return RedirectToAction(nameof(BadLogin));
                    }
                }

                return View();
            }
            return RedirectToAction(nameof(BadLogin));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(MovieViewModel model)
        {
            Movie movie = new()
            {
                ID = model.ID,
                Title = model.Title,
                Description = model.Description,
                Year = model.Year,
                Producer = model.Producer,
                Added = User.Identity.Name,
                Poster = await _imageSaver.SaveFile(model.Poster)
            };

            if (!ModelState.IsValid)
                return View(movie);

            if (_signInManager.IsSignedIn(User))
            {
                AddOrUpdateEntity(movie);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(BadLogin));
        }

        private async void AddOrUpdateEntity(Movie movie)
        {
            if (movie.ID == 0)
                _context.Movies.Add(movie);
            else
                _context.Movies.Update(movie);

            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;
            IQueryable<Movie> source = _context.Movies;
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new(count, page, pageSize);
            IndexViewModel viewModel = new()
            {
                PageViewModel = pageViewModel,
                Movies = items
            };

            return View(viewModel);
        }

        public IActionResult BadLogin() =>  View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => 
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
