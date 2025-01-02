using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebEcommerce.Models;
using WebEcommerce.Service;

namespace WebEcommerce.Controllers
{
    [Route("/Admin/[controller]/{action=Index}/{id?}")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        private readonly int pageSize = 5;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index(int pageIndex, string sname, string? sort, string? orderby)
        {
            IQueryable<Product> query = context.Products;
            query = context.Products.OrderByDescending(p => p.id);
            string[] validOrderBy = { "desc", "asc" };
            if(pageIndex < 1)
            {
                pageIndex = 1;
            }

            if (!string.IsNullOrEmpty(sname))
            {
                query = query.Where(p => p.name.Contains(sname));
            }

            if (!validOrderBy.Contains(orderby))
            {
                orderby = "asc";
            }
            
            if(sort != null)
            {
                string valueSort = sort+orderby;
                switch (valueSort)
                {
                    case "idasc": 
                        query = query.OrderBy(p => p.id);
                        break;
                    case "nameasc":
                        query = query.OrderBy(p => p.name);
                        break;
                    case "brandasc":
                        query = query.OrderBy(p => p.brand);
                        break;
                    case "priceasc":
                        query = query.OrderBy(p => p.price);
                        break;
                    case "createdateasc":
                        query = query.OrderBy(p => p.createDate);
                        break;
                    case "iddesc":
                        query = query.OrderByDescending(p => p.id);
                        break;
                    case "namedesc":
                        query = query.OrderByDescending(p => p.name);
                        break;
                    case "branddesc":
                        query = query.OrderByDescending(p => p.brand);
                        break;
                    case "pricedesc":
                        query = query.OrderByDescending(p => p.price);
                        break;
                    case "createdatedesc":
                        query = query.OrderByDescending(p => p.createDate);
                        break;
                }
            }

            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var products = query.ToList();

            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalPages;
            ViewData["Sort"] = sort;
            ViewData["OrderBy"] = orderby;

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDTO productDTO) {
            if(productDTO.imageFileName == null)
            {
                ModelState.AddModelError("imageFileName", "The image field is required.");
            }
            
            if (!ModelState.IsValid)
            {
                return View(productDTO);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            newFileName += Path.GetExtension(productDTO.imageFileName!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDTO.imageFileName!.CopyTo(stream);
            }

            var product = new Product
            {
                name = productDTO.name,
                brand = productDTO.brand,
                category = productDTO.category,
                price = productDTO.price,
                description = productDTO.description,
                imageFileName = newFileName,
                createDate = DateTime.Now
            };
            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }

            var productDTO = new ProductDTO
            {
                name = product.name,
                brand = product.brand,
                category = product.category,
                price = product.price,
                description = product.description
            };

            ViewData["ProductId"] = id;
            ViewData["ImageFileName"] = product.imageFileName;
            ViewData["CreateDate"] = product.createDate;

            return View(productDTO);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductDTO productDTO)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }
            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = id;
                ViewData["ImageFileName"] = product.imageFileName;
                ViewData["CreateDate"] = product.createDate;
                return View(productDTO);
            }

            string newFileName = product.imageFileName;
            if (productDTO.imageFileName != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                newFileName += Path.GetExtension(productDTO.imageFileName!.FileName);
                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDTO.imageFileName!.CopyTo(stream);
                }

                string oldImagePath = environment.WebRootPath + "/products/" + product.imageFileName;
                System.IO.File.Delete(oldImagePath);
            }

            product.name = productDTO.name;
            product.brand = productDTO.brand;
            product.category = productDTO.category;
            product.price = productDTO.price;
            product.description = productDTO.description;
            product.imageFileName = newFileName;

            context.Products.Update(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }
            string imagePath = environment.WebRootPath + "/products/" + product.imageFileName;
            System.IO.File.Delete(imagePath);

            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
    }
}
