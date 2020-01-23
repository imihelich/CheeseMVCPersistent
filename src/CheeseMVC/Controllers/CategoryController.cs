using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;

namespace CheeseMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CheeseDbContext context;

        public CategoryController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<CheeseCategory> categoryList = context.Categories.ToList();
            return View(categoryList);
        }

        public IActionResult Add()
        {
            AddCategoryViewModel addCategory = new AddCategoryViewModel();
            return View(addCategory);
        }

        [HttpPost]
        public IActionResult Add(AddCategoryViewModel addCategory)
        {
            if (ModelState.IsValid)
            {
                CheeseCategory newCategory = new CheeseCategory()
                {
                    Name = addCategory.Name

                };

                context.Categories.Add(newCategory);
                context.SaveChanges();
                return Redirect("/Category");
            }

            return View(addCategory);
        }
    }
}
