using CoffeeApp.Data;
using CoffeeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly CoffeeContext _context;

        public AccountController(CoffeeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string phone)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.Email == email && c.Phone == phone);

            if (customer != null)
            {
                HttpContext.Session.SetInt32("CustomerId", customer.Id);
                return RedirectToAction("Create", "Order");
            }

            ViewBag.Error = "Giriş bilgileri hatalı.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            // Email zaten kayıtlı mı kontrol et
            var existing = _context.Customers.FirstOrDefault(c => c.Email == customer.Email);
            if (existing != null)
            {
                ViewBag.Error = "Bu e-posta zaten kayıtlı.";
                return View();
            }

            _context.Customers.Add(customer);
            _context.SaveChanges();

            // Otomatik giriş yapsın
           // HttpContext.Session.SetInt32("CustomerId", customer.Id);

            return RedirectToAction("Create", "Order");
        }

    }
}
