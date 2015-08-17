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
using Microsoft.AspNet.Identity.EntityFramework;
using BugTracker.Models;
using BugTracker.Controllers;


namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserProjectsHelper PHelper = new UserProjectsHelper();
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));


        private void CreateHist(int newTicketId, string property, string oldValue, string newValue)
        {
            var tHist = new TicketHistory();
            tHist.TicketId = newTicketId;
            tHist.Property = property;
            tHist.OldValue = oldValue;
            tHist.NewValue = newValue;
            tHist.Changed = DateTimeOffset.UtcNow;
            tHist.UserId = User.Identity.GetUserId();
            db.Entry(tHist).State = EntityState.Added;
            db.SaveChanges();
            return;
        }

        private void SendNotification(string ATUId, int ticketId, string msgSubject)
        {
            var notification = new TicketNotification();
            var ticketTitle = db.Tickets.Find(ticketId).Title;
            var uEmail = db.Users.FirstOrDefault(u => u.Id == ATUId).Email;
            var udName = db.Users.FirstOrDefault(u => u.Id == ATUId).DisplayName;
            var body = "Dear " + udName + ", ";
            if (msgSubject == "A Ticket Has Been Assigned to You")
            {
                body += "A ticket entitled " + ticketTitle
                    + " has been assigned to you. ";
                notification.Purpose = "Assignment";
            }
            else
            {
                body += "A ticket to which you are assigned, entitled " + ticketTitle + ", has been modified by another user. ";
                notification.Purpose = "Ticket modified";
            }
            body += "Please log in to the application, click on 'Tickets', and then on the title of this ticket to see the details. "
                    + "This is an auto-generated e-mail. Please do not reply to it.";
            manager.SendEmail(ATUId, msgSubject, body);
            notification.TicketId = ticketId;
            notification.UserId = ATUId;
            notification.DateTime = DateTimeOffset.UtcNow;
            db.Entry(notification).State = EntityState.Added;
            db.SaveChanges();
        }


        // GET: Tickets
        [Authorize]
        public ActionResult Index(string lType)
        {

            var TT = db.TicketTypes.AsEnumerable();
            var TTDDList = new SelectList(TT, "Id", "Name");
            ViewBag.TTDDList = TTDDList;

            var TP = db.TicketPriorities.AsEnumerable();
            var TPDDList = new SelectList(TP, "Id", "Name");
            ViewBag.TPDDList = TPDDList;

            var TS = db.TicketStatus.AsEnumerable();
            var TSDDList = new SelectList(TS, "Id", "Name");
            ViewBag.TSDDList = TSDDList;

            if (lType == null)
            {
                var tList = db.Tickets.ToList();
                return View(tList);
            }
            else
            {
                Dispose(true);
                var db2 = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                if (lType == "assigned")
                {
                    var tList = db2.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                    return View(tList);
                }
                else
                {
                    var tList = db2.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    return View(tList);
                }
            }
        }


        ////POST: Tickets
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index(int? TicketTypeId, int? TicketPriorityId, int? TicketStatusId)
        //{
        //    var tList = (IQueryable<Ticket>)db.Tickets;
        //    if (TicketTypeId != null)
        //    {
        //        tList = tList.Where(t => t.TicketTypeId == TicketTypeId);
        //    }
        //    if (TicketPriorityId != null)
        //    {
        //        tList = tList.Where(t => t.TicketPriorityId == TicketPriorityId);
        //    }
        //    if (TicketStatusId != null)
        //    {
        //        tList = tList.Where(t => t.TicketStatusId == TicketStatusId);
        //    }
        //    return View(tList);
        //}





        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
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
            var attachments = db.TicketAttachments.Where(ta => ta.TicketId == id);
            ticket.Attachments = attachments.ToList();
            return View(ticket);
        }

        //POST: Tickets/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id, string allowed, string AttachDesc, HttpPostedFileBase fileUpload)
        {
            var ticket = db.Tickets.Find(Id);
            ViewBag.CreateAllowed = true;
            if (ModelState.IsValid)
            {
                if (fileUpload != null && fileUpload.ContentLength > 0 && allowed == "true")
                {
                    var attachment = new TicketAttachment();
                    var userId = User.Identity.GetUserId();
                    attachment.TicketId = Id;
                    attachment.Description = AttachDesc;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    attachment.Created = DateTimeOffset.UtcNow;
                    attachment.UserId = userId;
                    attachment.FilePath = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                    fileUpload.SaveAs(attachment.FilePath);
                    attachment.FileUrl = "~/Uploads/" + fileName;
                    db.TicketAttachments.Add(attachment);
                    db.SaveChanges();
                    if (ticket.AssignedToUserId != null && ticket.AssignedToUserId != userId)
                    {
                        var subject = "A Ticket Assigned to You Has Been Modified";
                        SendNotification(ticket.AssignedToUserId, ticket.Id, subject);
                    }
                }

            }
            return View(ticket);
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

            TempData["ticket"] = db.Tickets.Include("Project").Include("AssignedToUser")
                    .Include("TicketType").Include("TicketPriority").Include("TicketStatus")
                    .ToList().FirstOrDefault(t => t.Id == ticket.Id);

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


                //Make sure user role and ticket assignment status are consistent 
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


                //Create ticket history entry

                Ticket oldTicket = (Ticket)TempData["ticket"];

                var oldName = "";
                var newName = "";
                var subject = "";
                bool modified = false;

                if (oldTicket.Title != ticket.Title)
                {
                    CreateHist(oldTicket.Id, "Title", oldTicket.Title, ticket.Title);
                    modified = true;
                }
                if (oldTicket.Description != ticket.Description)
                {
                    CreateHist(oldTicket.Id, "Description", oldTicket.Description, ticket.Description);
                    modified = true;
                }
                //if (oldTicket.ProjectId != ticket.ProjectId)
                //{
                //    oldName = db.Projects.Find(oldTicket.ProjectId).Name;
                //    newName = db.Projects.Find(ticket.ProjectId).Name;
                //    CreateHist(oldTicket.Id, "Project", oldName, newName);
                //}
                if (oldTicket.TicketTypeId != ticket.TicketTypeId)
                {
                    oldName = db.TicketTypes.Find(oldTicket.TicketTypeId).Name;
                    newName = db.TicketTypes.Find(ticket.TicketTypeId).Name;
                    CreateHist(oldTicket.Id, "Ticket Type", oldName, newName);
                    modified = true;
                }
                if (oldTicket.TicketPriorityId != ticket.TicketPriorityId)
                {
                    oldName = db.TicketPriorities.Find(oldTicket.TicketPriorityId).Name;
                    newName = db.TicketPriorities.Find(ticket.TicketPriorityId).Name;
                    CreateHist(oldTicket.Id, "Ticket Priority", oldName, newName);
                    modified = true;
                }
                if (oldTicket.TicketStatusId != ticket.TicketStatusId)
                {
                    oldName = db.TicketStatus.Find(oldTicket.TicketStatusId).Name;
                    newName = db.TicketStatus.Find(ticket.TicketStatusId).Name;
                    CreateHist(oldTicket.Id, "Ticket Status", oldName, newName);
                    modified = true;
                }
                if (oldTicket.AssignedToUserId != ticket.AssignedToUserId)
                {
                    if (oldTicket.AssignedToUserId != null)
                    {
                        oldName = db.Users.Find(oldTicket.AssignedToUserId).DisplayName;
                    }
                    if (ticket.AssignedToUserId != null)
                    {
                        newName = db.Users.Find(ticket.AssignedToUserId).DisplayName;
                    }
                    CreateHist(oldTicket.Id, "Assigned To", oldName, newName);
                    subject = "A Ticket Has Been Assigned to You";
                    modified = true;
                }

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                //Notify assigned user of modification by others
                if (modified)
                {
                    
                    if (ticket.AssignedToUserId != null )
                    {
                        if (subject == "")
                        {
                            subject = "A Ticket Assigned to You Has Been Modified";
                        }
                        SendNotification(ticket.AssignedToUserId, ticket.Id, subject);
                    }
                }

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
