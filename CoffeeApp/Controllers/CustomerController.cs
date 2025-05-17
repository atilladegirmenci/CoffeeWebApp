using Microsoft.AspNetCore.Mvc;
using CoffeeApp.Models;
using CoffeeApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CoffeeContext _context;

        public CustomerController(CoffeeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View( _context.Customers.ToList());
        }
    }
}
