using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Proiect_Forum_Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace Proiect_Forum_Users.Controllers
{
    [Authorize(Roles = "User,Moderator,Admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Users
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var users = from user in db.Users
                        orderby user.UserName
                        select user;

            ViewBag.UsersList = users;
            return View();
        }

        [Authorize(Roles = "User,Moderator,Admin")]
        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            string currentRole = user.Roles.FirstOrDefault().RoleId;
            var userRoleName = (from role in db.Roles
                                where role.Id == currentRole
                                select role.Name).First();

            ViewBag.roleName = userRoleName;
            return View(user);
        }

        [Authorize(Roles = "User,Moderator,Admin")]
        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            var userRoleName = (from role in db.Roles
                                where role.Id == userRole.RoleId
                                select role.Name).First();

            ViewBag.roleName = userRoleName;


            if (id == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(user);
            }
            else
            {
                TempData["message"] = "You cannot edit someone else's profile!";
                return RedirectToAction("Index", "Categories");
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();
            var roles = from role in db.Roles select role;
            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }

        [HttpPut]
        [Authorize(Roles = "User,Moderator,Admin")]
        public ActionResult Edit(string id, ApplicationUser newData)
        {
            ApplicationUser user = db.Users.Find(id);
            newData.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                if (ModelState.IsValid && TryUpdateModel(user))
                {
                    user.UserName = newData.UserName;
                    user.Email = newData.Email;
                    user.PhoneNumber = newData.PhoneNumber;
                    if (User.IsInRole("Admin"))
                    {
                        var roles = from role in db.Roles select role;
                        foreach (var role in roles)
                        {
                            UserManager.RemoveFromRole(id, role.Name);
                        }
                        var selectedRole = db.Roles.Find(HttpContext.Request.Params.Get("newRole"));
                        UserManager.AddToRole(id, selectedRole.Name);
                    }
                    db.SaveChanges();
                    if (User.IsInRole("Admin"))
                        return RedirectToAction("Index", "Users");
                    return RedirectToAction("Index", "Categories");
                }
                newData.Id = id;
                return View(newData);
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                newData.Id = id;
                return View(newData);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new
                UserStore<ApplicationUser>(context));
            var user = UserManager.Users.FirstOrDefault(u => u.Id == id);
            // de verificat stergerea adminului
            var topics = db.Topics.Where(t => t.UserId == id);
            foreach (var topic in topics)
            {
                db.Topics.Remove(topic);
            }
            var posts = db.Posts.Where(post => post.UserId == id);
            foreach (var post in posts)
            {
                db.Posts.Remove(post);
            }

            db.SaveChanges();
            UserManager.Delete(user);
            return RedirectToAction("Index");
        }
    }
}