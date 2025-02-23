﻿using Microsoft.AspNetCore.Mvc;
using WebEcommerce.Models;
using WebEcommerce.Service;

namespace WebEcommerce.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly int pageSize = 8;

        public StoreController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(int pageIndex, string? search, string? brand, string? category, string? sort)
        {
            IQueryable<Product> query = context.Products;

            if (!string.IsNullOrEmpty(search) && search.Length > 0)
            {
                query = query.Where(p => p.name.Contains(search));
            }

            if (!string.IsNullOrEmpty(brand) && brand.Length > 0)
            {
                query = query.Where(p => p.brand.Contains(brand));
            }

            if (!string.IsNullOrEmpty(category) && category.Length > 0)
            {
                query = query.Where(p => p.category.Contains(category));
            }

            if (!string.IsNullOrEmpty(sort) && sort.Length > 0)
            {
                switch (sort)
                {
                    case "newest": 
                        query = query.OrderByDescending(p => p.createDate);
                        break;
                    case "price_asc": 
                        query = query.OrderBy(p => p.price);
                        break;
                    case "price_des":
                        query = query.OrderByDescending(p => p.price);
                        break;
                }
            }

            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var products = query.ToList();

            ViewBag.Products = products; 
            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalPages;

            var storeSearchModel = new StoreSearchModel
            {
                Search = search,
                Brand = brand,
                Category = category,
                Sort = sort
            };

            return View(storeSearchModel);
        }

        public IActionResult Details(int id)
        {
            var product = context.Products.Find(id);
            if (product == null) { 
                return RedirectToAction("Index", "Store");
            }
            return View(product);
        }
    }
}
