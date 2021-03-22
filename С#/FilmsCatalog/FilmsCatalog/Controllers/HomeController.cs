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

                movie.Poster = await _imageSaver.SaveFile(mvm.Poster);

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
