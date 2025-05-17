using Microsoft.AspNetCore.Mvc;
using CoffeeApp.Models;
using CoffeeApp.Data;

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
            //var latte = _context.Products.FirstOrDefault(p => p.Name == "Latte");
            //if (latte != null)
            //{
            //    latte.ImageUrl = "/img/latte.png";
            //}
            //var espresso = _context.Products.FirstOrDefault(p => p.Name == "Espresso");
            //if (espresso != null)
            //{
            //    espresso.ImageUrl = "/img/espresso.png";
            //}
            //var americano = _context.Products.FirstOrDefault(p => p.Name == "Americano");
            //if (americano != null)
            //{
            //    americano.ImageUrl = "/img/americano.png";
            //}
            //var filtre = _context.Products.FirstOrDefault(p => p.Name == "Filter Coffee");
            //if(filtre != null)
            //{
            //    filtre.ImageUrl = "/img/filter.png";
                
            //}
            //_context.SaveChanges();

            //Product espresso = new Product()
            //{
            //    Name = "Espresso",
            //    Price = 60.00m

            //};
            //Product americano = new Product()
            //{
            //    Name = "Americano",
            //    Price = 75.00m

            //};
            //Product filtre = new Product()
            //{
            //    Name = "Filter Coffee",
            //    Price = 70.00m

            //};
            //_context.Products.AddRange(espresso,americano,filtre);
            //_context.SaveChanges();

            return RedirectToAction("Index","Customer");
        }
    }
}
