using Microsoft.AspNetCore.Mvc;
using WebEcommerce.Service;

namespace WebEcommerce.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext context;

        public StoreController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p => p.id).ToList();

            ViewBag.Products = products;
            return View();
        }
    }
}
