﻿using Proiect_Forum.Models;
using Proiect_Forum_Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_Forum.Controllers
{
    public class TopicsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Topics
        public ActionResult Index(int id)
        {
            var category = db.Categories.Find(id);
            var topics = from top in db.Topics
                         where top.CategoryId == id select top;

            ViewBag.Category = category;
            ViewBag.Topics = topics;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            return View();
        }

        

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;
            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Title.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }

        public ActionResult New(int id)
        {
            Topic topic = new Topic();
            topic.CategoryId = id;
            return View(topic);
        }

        [HttpPost]
        public ActionResult New(Topic topic)
        {
            topic.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Topics.Add(topic);
                    db.SaveChanges();
                    TempData["message"] = "The topic has been added.";
                    return RedirectToAction("Index", new { id = topic.CategoryId });
                }
                return View(topic);
            }
            catch (Exception e)
            {
                return View(topic);
            }
        }

        public ActionResult Show(int id)
        {
            Topic topic = db.Topics.Find(id);

            if (TempData.ContainsKey("message"))
                ViewBag.message = TempData["message"].ToString();

            return View(topic);
        }

        public ActionResult Edit(int id)
        {
            Topic topic = db.Topics.Find(id);
            topic.Categ = GetAllCategories();
            return View(topic);
        }

        [HttpPut]
        public ActionResult Edit(int id, Topic requestTopic)
        {
            try
            {
                Topic topic = db.Topics.Find(id);
                if (ModelState.IsValid && TryUpdateModel(topic))
                {
                    topic.Title = requestTopic.Title;
                    topic.Content = requestTopic.Content;
                    topic.Date = requestTopic.Date;
                    topic.CategoryId = requestTopic.CategoryId;
                    db.SaveChanges();
                    return RedirectToAction("Show", new { id = topic.TopicId });
                }

                return View(requestTopic);
            }
            catch (Exception e)
            {
                return View(requestTopic);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Topic topic = db.Topics.Find(id);
            TempData["message"] = "The topic \"" + topic.Title + "\" has been deleted.";
            db.Topics.Remove(topic);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = topic.CategoryId });
        }
    }
}