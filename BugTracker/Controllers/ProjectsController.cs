using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserProjectsHelper PHelper = new UserProjectsHelper();
        private UserRolesHelper UHelper = new UserRolesHelper();
        //protected UserManager<ApplicationUser> UserManager { get; set; }
        //protected ICollection<Project>projectList {get; set;}


        // GET: Projects
        [Authorize(Roles = "Admin, Project Manager, Developer")]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var projectList = db.Projects.ToList();
                return View(projectList);
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var projectList = PHelper.ListProjectsForUser(userId);
                ViewBag.Tickets = db.Tickets;
                return View(projectList);
            }

        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            //ViewBag.UsersNotOnProj = new SelectList(PHelper.ListUsersNotOnProject(id), "Id", "DisplayName");
            //ViewBag.UsersOnProj = new SelectList(PHelper.ListUsersOnProject(id), "Id", "DisplayName");
            return View(project);
        }

        // POST: Projects/Details/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Details([Bind(Include ="Personnel")] Project project)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(project).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    return View(project);
        //}

        // GET: Projects/Manage
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Manage()
        {
            var dUsers = UHelper.UsersInRole("Developer");
            var rUsers = (IEnumerable<ApplicationUser>)dUsers;
            var c = User.IsInRole("Admin");
            if (User.IsInRole("Admin"))
            {
                var pmUsers = UHelper.UsersInRole("Project Manager");
                rUsers = dUsers.Union(pmUsers);
            }
            var UList = new SelectList(rUsers, "Id", "DisplayName");
            ViewBag.UserList = UList;
            ViewBag.ProjectList = new SelectList(db.Projects, "Id", "Name");
            return View();
        }

        //POST: Projects/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(int project, string user, string operation)
        {
            if (!(operation == null || project == 0 || user == ""))
            {
                if (operation == "Add")
                {
                    PHelper.AddUserToProject(user, project);
                }
                else
                {
                    PHelper.RemoveUserFromProject(user, project);
                }
                return RedirectToAction("Index", "Projects");
            }
            else
            {
                return RedirectToAction("Manage", "Projects");
            }
        }




        // GET: Projects/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Begun,Discontinued")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Begun,Discontinued")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        ////GET: Projects/AssignUserToProject
        //[Authorize(Roles = "Admin, Project Manager")]
        //public ActionResult AssignUserToProject()
        //{
        //    ViewBag.projectList = db.Projects.ToList();
        //    ViewBag.userList = db.Users.ToList();
        //    return View();
        //}

        ////POST: 
        //[HttpPost, ActionName("AssignUserToProject")]
        //[ValidateAntiForgeryToken]
        //public ActionResult AssignUserToProject(string userId, int projectId)
        //{
        //    PHelper.AddUserToProject(userId, projectId);
        //    return View(); //This will probably change.
        //}


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
