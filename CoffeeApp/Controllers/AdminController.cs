using Microsoft.AspNetCore.Mvc;
using CoffeeApp.Models;
using CoffeeApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly CoffeeContext _context;

        public AdminController(CoffeeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.OrderCreated)
                .ToList();

            return View(orders);
        }
        [HttpPost]
        public IActionResult FulfillOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            
            if(order != null)
            {
                order.OrderFullfilled = DateTime.Now;
                _context.SaveChanges();

            }
            return RedirectToAction("Index");
        }
    }
}
