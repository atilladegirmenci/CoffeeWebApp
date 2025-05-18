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
            if(!IsLOggedIn()) return RedirectToAction("Login", "Account");

            ViewBag.Products = _context.Products.ToList();
            return View(new Order());
        }
        [HttpPost]
        public IActionResult Create(string CartJson)
        {
            if (!IsLOggedIn()) return RedirectToAction("Login", "Account");
            var customerId = GetCurrentCustomerId();

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
  
        public IActionResult MyOrders()
        {
            if (!IsLOggedIn()) return RedirectToAction("Login", "Account");
            var customerId = GetCurrentCustomerId();

            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.CustomerId == customerId) // varsa filtrele
                .OrderByDescending(o => o.OrderCreated) // en yeni en üstte
                .ToList();

            return View(orders);
        }

        private bool IsLOggedIn()
        {
            var customerId = GetCurrentCustomerId();
            return customerId != null;
        }
        private int? GetCurrentCustomerId()
        {
            return HttpContext.Session.GetInt32("CustomerId");
        }
    }
}
