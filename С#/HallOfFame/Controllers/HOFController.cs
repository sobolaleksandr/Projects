using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HallOfFame.Models;

namespace HallOfFame.Controllers
{
    [Route("api/v1")]
    public class HOFController : Controller
    {
        HOFContext db;
        public HOFController(HOFContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View(db.Skills.ToList());
        }
        //[HttpGet]
        //public IActionResult Buy(int? id)
        //{
        //    if (id == null) return RedirectToAction("Index");
        //    ViewBag.PhoneId = id;
        //    return View();
        //}
        //[HttpPost]
        //public string Buy(Order order)
        //{
        //    db.Orders.Add(order);
        //    // сохраняем в бд все изменения
        //    db.SaveChanges();
        //    return "Спасибо, " + order.User + ", за покупку!";
        //}
    }
}