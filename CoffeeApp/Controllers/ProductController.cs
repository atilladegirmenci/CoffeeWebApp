using Microsoft.AspNetCore.Mvc;
using CoffeeApp.Models;
using CoffeeApp.Data;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoffeeApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly CoffeeContext _context;
        public ProductController(CoffeeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return RedirectToAction("Index","Customer");
        }
    }
}
