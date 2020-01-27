using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Data;
using CheeseMVC.ViewModels;
using CheeseMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {

            return View(context.Menus.ToList());
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };

                context.Menus.Add(newMenu);
                context.SaveChanges();
                return Redirect("/Menu/ViewMenu/"+newMenu.ID);
            }

            return View();
        }

        [HttpGet]
        public IActionResult ViewMenu(int id)
        {
            Menu newMenu = context.Menus.Single(c => c.ID == id);

            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();

            ViewMenuViewModel viewMenu = new ViewMenuViewModel
            {
                Menu = newMenu,
                Items = items
            };

            return View(viewMenu);
        }

        [HttpGet]
        public IActionResult AddItem(int id)
        {
            Menu findMenu = context.Menus.Single(c => c.ID == id);
            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(findMenu, context.Cheeses.ToList());
            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == addMenuItemViewModel.CheeseID)
                    .Where(cm => cm.MenuID == addMenuItemViewModel.MenuID).ToList();

                if(existingItems.Count() == 0)
                {
                    CheeseMenu cheeseMenu = new CheeseMenu
                    { 
                    MenuID = addMenuItemViewModel.MenuID,
                    CheeseID = addMenuItemViewModel.CheeseID
                    };
                    context.CheeseMenus.Add(cheeseMenu);
                    context.SaveChanges();
                }
                return Redirect("/Menu/ViewMenu/" + addMenuItemViewModel.MenuID);
            }
            return View(addMenuItemViewModel);
        }
     }
}
