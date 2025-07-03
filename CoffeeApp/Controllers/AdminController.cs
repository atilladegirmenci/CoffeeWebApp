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
        public IActionResult EditMenu()
        {
            ViewBag.product = _context.Products.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult EditMenu(List<Product> prod)
        {

            foreach (var item in prod)
            {
                var product = _context.Products.Find(item.Id);
                product.Price = item.Price;
                product.IsAvailable = item.IsAvailable;
            }

            _context.SaveChanges();
            return RedirectToAction("Create", "Order");

        }
    }
}
