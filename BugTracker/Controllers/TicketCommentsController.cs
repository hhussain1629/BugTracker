using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    //[RequireHttps]
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserProjectsHelper PHelper = new UserProjectsHelper();
        private UserRolesHelper UHelper = new UserRolesHelper();

        // GET: Comments/Create
        [Authorize]
        public ActionResult Create(int id, bool allowed)
        {
            if (allowed)
            {
                TicketComment TicketComment = new TicketComment();
                TicketComment.Created = DateTimeOffset.UtcNow;
                TicketComment.TicketId = id;
                TicketComment.UserId = User.Identity.GetUserId();
                return View(TicketComment);
            }
            else
            {
                Dispose(true);
                RedirectToAction("Details", "Tickets", new { tId = id });
            }
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,UserId,Body,Created,Updated")] TicketComment comment)
        {
            if (ModelState.IsValid)
            {
                db.TicketComments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = comment.TicketId });
            }
            //ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
            //ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
            return View(comment);
        }

        //// GET: Comments
        //[Authorize]
        //public ActionResult Index(int? id)
        //{
        //    var comments = db.TicketComments.Where(c => c.TicketId == id);
        //    return View(comments.ToList());
        //}

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment comment = db.TicketComments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }






        //// GET: Comments/Edit/5
        //[Authorize(Roles="Admin, Mod")]
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Comment comment = db.Comments.Find(id);
        //    if (comment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
        //    ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
        //    return View(comment);
        //}

        //// POST: Comments/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,PostId,AuthorId,Body,Created,Updated")] Comment comment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        comment.Updated = DateTime.Now;
        //        db.Entry(comment).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Details", "Posts", new { id = comment.PostId });
        //        //return RedirectToAction("Index");
        //    }
        //    ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
        //    ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
        //    return View(comment);
        //}

        //// GET: Comments/Delete/5
        //[Authorize (Roles="Admin, Mod")]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Comment comment = db.Comments.Find(id);
        //    if (comment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(comment);
        //}

        //// POST: Comments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Comment comment = db.Comments.Find(id);
        //    int tempid = comment.PostId;
        //    db.Comments.Remove(comment);
        //    db.SaveChanges();
        //    return RedirectToAction("Details", "Posts", new { id = tempid });
        //    //return RedirectToAction("Index");
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
