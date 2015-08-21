using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class TicketNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketNotification
        [Authorize (Roles="Developer")]
        public ActionResult Index(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var notifications = db.TicketNotifications.Where(tn => tn.Ticket.AssignedToUserId == id).ToList();
            ViewBag.DisplayName = db.Users.FirstOrDefault(u => u.Id == id).DisplayName;
            return View(notifications);
        }
    }
}