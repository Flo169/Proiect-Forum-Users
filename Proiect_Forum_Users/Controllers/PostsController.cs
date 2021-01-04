using Microsoft.AspNet.Identity;
using Proiect_Forum.Models;
using Proiect_Forum_Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_Forum.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Edit(int id1, int id2)
        {
            Post post = db.Posts.Find(id1, id2); // PostId, TopicId
            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Moderator") || User.IsInRole("Admin"))
            {
                return View(post);
            }
            else
            {
                TempData["message"] = "You cannot edit someone else's post!";
                return RedirectToAction("Show", "Topics", new { id = post.TopicId });
            }
        }

        [HttpPut]
        [Authorize(Roles = "User,Moderator,Admin")]
        public ActionResult Edit(int id1, int id2, Post requestPost)
        {
            try
            {
                Post post = db.Posts.Find(id1, id2);
                if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Moderator") || User.IsInRole("Admin"))
                {
                    if (ModelState.IsValid && TryUpdateModel(post))
                    {
                        post.Content = requestPost.Content;
                        post.Date = requestPost.Date;
                        db.SaveChanges();
                        return RedirectToAction("Show", "Topics", new { id = post.TopicId });
                    }
                    return View(requestPost);
                }
                TempData["message"] = "You cannot edit someone else's post!";
                return RedirectToAction("Show", "Topics", new { id = post.TopicId });
            }
            catch (Exception e)
            {
                return View(requestPost);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User,Moderator,Admin")]
        public ActionResult New(Post post)
        {
            post.Date = DateTime.Now;
            int? maxid = db.Posts.Max(p => (int?)p.PostId);
            if (maxid == null)
                post.PostId = 0;
            else
                post.PostId = (int) maxid + 1;

            post.UserId = User.Identity.GetUserId();

            try
            {
                if (ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["message"] = "The post has been added.";
                }
                else
                    TempData["message"] = "Content is mandatory.";

                return RedirectToAction("Show", "Topics", new { id = post.TopicId });
            }
            catch (Exception e)
            {
                TempData["message"] = "The post was not added.";
                return RedirectToAction("Show", "Topics", new { id = post.TopicId });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User,Moderator,Admin")]
        public ActionResult Delete(int id1, int id2)
        {
            Post post = db.Posts.Find(id1, id2);
            int TopicId = post.TopicId;

            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Moderator") || User.IsInRole("Admin"))
            {
                TempData["message"] = "The post has been deleted.";
                db.Posts.Remove(post);
                db.SaveChanges();
                return RedirectToAction("Show", "Topics", new { id = post.TopicId });
            }
            
            TempData["message"] = "You cannot delete someone else's post!";
            return RedirectToAction("Show", "Topics", new { id = post.TopicId });
        }
    }
}