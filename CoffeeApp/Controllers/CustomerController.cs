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
        public IActionResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if(!ModelState.IsValid)
            {
                return View(customer);
            }

            _context.Customers.Update(customer);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
