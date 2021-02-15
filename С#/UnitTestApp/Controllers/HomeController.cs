using Microsoft.AspNetCore.Mvc;
using UnitTestApp.Models;

namespace UnitTestApp.Controllers
{
    public class HomeController : Controller
    {
        IRepository repo;
        public HomeController(IRepository r)
        {
            repo = r;
        }
        public IActionResult Index()
        {
            return View(repo.GetAll());
        }
        //проверка баланса клиента
        public IActionResult GetUser(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            User user = repo.Get(id.Value);
            if (user == null)
                return NotFound();

            return View(user);
        }

        public IActionResult AddUser() => View();
        //регистрация пользователя
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                repo.Create(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }
        //начисление/списание суммы
        public IActionResult Withdraw(long? id, decimal? sum)
        {
            if (!id.HasValue)
                return BadRequest();
            User user = repo.Get(id.Value);
            if (user == null)
                return NotFound();

            if (user.Balance + sum.Value >= 0)
            {
                user.Balance += sum.Value;

                repo.Update(user);
                return View(user);
            }
            else
            {
                return View(user);//просто возвращаю user
            }

        }
    }
}