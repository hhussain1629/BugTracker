using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.IO;
using Microsoft.AspNet.Identity;
using BugTracker.Models;
using BugTracker.Controllers;


namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserProjectsHelper PHelper = new UserProjectsHelper();

        // GET: Tickets
        [Authorize]
        public ActionResult Index(string lType)
        {
            if (lType == null)
            {
                return View(db.Tickets.ToList());
            }
            else
            {
                Dispose(true);
                var db = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                if (lType == "assigned")
                {
                    var tList = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                    return View(tList);
                }
                else
                {
                    var tList = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    return View(tList);
                }
            }
        }





        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            ViewBag.CreateAllowed = false;
            if (User.IsInRole("Admin"))
            {
                ViewBag.CreateAllowed = true;
            }
            if (User.IsInRole("Project Manager"))
            {
                var tpId = ticket.ProjectId;
                var project = db.Projects.Find(tpId);
                if (project.Users.Any(u => u.Id == userId))
                {
                    ViewBag.CreateAllowed = true;
                }
            }
            if (User.IsInRole("Developer"))
            {
                if (ticket.AssignedToUserId == userId)
                {
                    ViewBag.CreateAllowed = true;
                }
            }
            if (User.IsInRole("Submitter"))
            {
                if (ticket.OwnerUserId == userId)
                {
                    ViewBag.CreateAllowed = true;
                }
            }
            ViewBag.UserId = userId;
            var comments = db.TicketComments.Where(c => c.TicketId == id);
            ticket.Comments = comments.ToList();
            return View(ticket);
        }

        //POST: Tickets/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Details([Bind(Include = "TicketId, Description")] TicketAttachment attachment, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    var fileName = attachment.Id.ToString();
                    attachment.FilePath = "C:\\Users\\Hammad\\Documents\\Visual Studio 2013\\Projects\\BugTracker\\BugTracker\\Views\\"
                        + fileName;
                    attachment.Created = DateTimeOffset.UtcNow;
                    attachment.UserId = User.Identity.GetUserId();
                    attachment.FileUrl = "~/Views/UserAttachments/" + fileName;
                    fileUpload.SaveAs(Path.Combine(Server.MapPath("~/Views/UserAttachments"), fileName));
                    db.TicketAttachments.Add(attachment);
                    db.SaveChanges();
                }
                
            }
            return View();
        }


        // GET: Tickets/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ProjectList = new SelectList(PHelper.ListProjects(), "Id", "Name");
            ViewBag.TypeList = new SelectList(db.TicketTypes.ToList(), "Id", "Name");
            ViewBag.PriorityList = new SelectList(db.TicketPriorities.ToList(), "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title, Description, ProjectId, TicketTypeId, TicketPriorityId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //To be modified to allow certain users to set ticket Priority, Status, and Type, and to 
                //assign a ticket to a user.
                ticket.Created = DateTimeOffset.Now.UtcDateTime;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.TicketStatusId = db.TicketStatus.SingleOrDefault(ts => ts.Name == "Open").Id;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, Project Manager, Developer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            var TT = db.TicketTypes.AsEnumerable();
            var TTDDList = new SelectList(TT, "Id", "Name");
            ViewBag.TTDDList = TTDDList;

            var TP = db.TicketPriorities.AsEnumerable();
            var TPDDList = new SelectList(TP, "Id", "Name");
            ViewBag.TPDDList = TPDDList;

            var TS = db.TicketStatus.AsEnumerable();
            var TSDDList = new SelectList(TS, "Id", "Name");
            ViewBag.TSDDList = TSDDList;

            var devRoleId = db.Roles.SingleOrDefault(r => r.Name == "Developer").Id;
            var D = db.Users.Where(u => u.Roles.Any(r => r.RoleId == devRoleId)).AsEnumerable();
            var DDDList = new SelectList(D, "Id", "DisplayName");
            ViewBag.DDDList = DDDList;

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {

                var openStatusId = db.TicketStatus.SingleOrDefault(ts => ts.Name == "Open").Id;
                var assignedStatusId = db.TicketStatus.SingleOrDefault(ts => ts.Name == "Assigned").Id;
                if (!(ticket.AssignedToUserId == null) && ticket.TicketStatusId == openStatusId)
                {
                    ticket.TicketStatusId = assignedStatusId;
                }
                if ((ticket.AssignedToUserId == null) && ticket.TicketStatusId == assignedStatusId)
                {
                    ticket.TicketStatusId = openStatusId;
                }
                ticket.Updated = DateTimeOffset.Now.UtcDateTime;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
