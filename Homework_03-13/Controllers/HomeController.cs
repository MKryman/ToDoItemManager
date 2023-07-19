using Homework_03_13.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Homework_03_13.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ToDoItemManager; Integrated Security=true";
        public IActionResult Index()
        {
            var mgr = new ToDoItemManager(connectionString);
            return View(mgr.GetAllNoncompletedItems());
        }

        [HttpPost]
        public IActionResult MarkCompleted(int id)
        {
            var mgr = new ToDoItemManager(connectionString);
            mgr.MarkAsComplete(id);
            return Redirect("/home/index");
        }

        public IActionResult NewCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category c)
        {
            var mgr = new ToDoItemManager(connectionString);
            mgr.AddCategory(c);
            return Redirect("/home/index");
        }

        public IActionResult NewItem()
        {
            var mgr = new ToDoItemManager(connectionString);
            return View(mgr.GetCategories());
        }

        [HttpPost]
        public IActionResult AddItem(ToDoItem item)
        {
            var mgr = new ToDoItemManager(connectionString);
            mgr.AddToDoItem(item);
            return Redirect("/home/index");
        }

        public IActionResult Completed()
        {
            var mgr = new ToDoItemManager(connectionString);
            return View(mgr.GetAllCompletedItems());
        }

        public IActionResult Categories()
        {
            var mgr = new ToDoItemManager(connectionString);
            return View(mgr.GetCategories());
        }

        public IActionResult ByCategory(int id)
        {
            var mgr = new ToDoItemManager(connectionString);
            return View(mgr.GetItemsForCategory(id));
        }

        public IActionResult EditCategory(int id)
        {
            var mgr = new ToDoItemManager(connectionString);
            return View(mgr.GetCategoryById(id));
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category c)
        {
            var mgr = new ToDoItemManager(connectionString);
            mgr.UpdateCategory(c);
            return Redirect("/home/categories");
        }
    }
}