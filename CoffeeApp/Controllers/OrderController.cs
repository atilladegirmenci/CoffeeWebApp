using CoffeeApp.Data;
using CoffeeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CoffeeApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly CoffeeContext _context;

        public OrderController(CoffeeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            //var products = _context.Products.ToList(); // Ürünleri alıyoruz
            //ViewBag.Products = new SelectList(products, "Id", "Name"); // Ürünleri dropdown menüsüne yerleştiriyoruz
            ViewBag.Products = _context.Products.ToList();
            return View(new Order());
        }
        [HttpPost]
        public IActionResult Create(string CartJson)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null) return RedirectToAction("Login", "Account");

            var order = new Order
            {
                CustomerId = customerId.Value,
                OrderCreated = DateTime.Now,
                OrderDetails = new List<OrderDetail>()
            };

            var cartItems = JsonSerializer.Deserialize<List<CartItemDto>>(CartJson);
            foreach (var item in cartItems)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            return RedirectToAction("MyOrders");
        }

        public class CartItemDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        //[HttpPost]
        //public IActionResult Create(Order order)
        //{
        //    var customerId = HttpContext.Session.GetInt32("CustomerId");

        //    if (customerId == null)
        //        return RedirectToAction("Login", "Account");

        //    order.CustomerId = customerId.Value;
        //    order.OrderCreated = DateTime.Now;

        //    // OrderDetail nesnelerini de ekleyelim
        //    foreach (var orderDetail in order.OrderDetails)
        //    {
        //        orderDetail.OrderId = order.Id;
        //    }

        //    _context.Orders.Add(order);
        //    _context.SaveChanges();

        //    return RedirectToAction("MyOrders");
        //}

        public IActionResult MyOrders()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.CustomerId == customerId) // varsa filtrele
                .ToList();

            return View(orders);
        }
    }
}
