using Microsoft.AspNetCore.Mvc;
using CoffeeApp.Models;
using CoffeeApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly CoffeeContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(CoffeeContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index() //list page
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

            if (order != null)
            {
                order.OrderFullfilled = DateTime.Now;
                _context.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public IActionResult EditMenu()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        [HttpPost]
        public IActionResult EditMenu(List<Product> prod)
        {

            foreach (var item in prod)
            {
                var product = _context.Products.Find(item.Id);
                if (product != null)
                {
                    product.Price = item.Price;
                    product.IsAvailable = item.IsAvailable;
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Create", "Order");
        }
        public IActionResult CreateCoffee()
        {
            return View("~/Views/Admin/CreateCoffee.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoffee(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "img");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    product.ImageUrl = "/img/" + fileName;
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index"); // list page
            }

            return View(product);


        }
        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if(product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");

        }
    }
}
