using Proiect_Forum.Models;
using Proiect_Forum_Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_Forum.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            var categories = from cat in db.Categories
                         select cat;
            ViewBag.Categories = categories;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            return View();
        }

        public ActionResult New()
        {
            Category category = new Category();

            return View(category);
        }

        [HttpPost]
        public ActionResult New(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    TempData["message"] = "The category has been added.";
                    return RedirectToAction("Index");
                }

                return View(category);
                
            }
            catch (Exception e)
            {
                return View(category);
            }
        }

        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        [HttpPut]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                Category category = db.Categories.Find(id);
                if (ModelState.IsValid && TryUpdateModel(category))
                {
                    category.Title = requestCategory.Title;
                    category.Description = requestCategory.Description;
                    category.CategoryId = requestCategory.CategoryId;
                    db.SaveChanges();
                    TempData["message"] = "The category \"" + category.Title + "\" has been edited.";
                    return RedirectToAction("Index");
                }
                return View(requestCategory);
            }
            catch (Exception e)
            {
                return View(requestCategory);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            TempData["message"] = "The category \"" + category.Title + "\" has been deleted.";
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}