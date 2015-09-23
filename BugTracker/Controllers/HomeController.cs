using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using SendGrid;
using System.Net.Mail;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper UHelper = new UserRolesHelper();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Project Manager, Developer")]
        public ActionResult Email()
        {
            ViewBag.Title = "Email";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Email(string emailTo, string emailFrom, string subject, string body)
        {
            var appSettings = ConfigurationManager.AppSettings;
            var credentials = new NetworkCredential(appSettings["SendGridUserName"], appSettings["SendGridPassword"]);
            SendGridMessage mymessage = new SendGridMessage();
            mymessage.AddTo(emailTo);
            mymessage.From = new MailAddress(emailFrom, "Hammad Hussain");
            mymessage.Subject = subject;
            mymessage.Text = body;
            var transportWeb = new Web(credentials);
            await transportWeb.DeliverAsync(mymessage);
            return RedirectToAction("EmailSent", "Home", null); 
        }

        public ActionResult EmailSent()
        {
            ViewBag.Title = "Email Sent";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        //GET: Home/Personnel
        [Authorize(Roles = "Admin")]
        public ActionResult Personnel()
        {
            var currentUsers = db.Users;


            //foreach (var person in currentUsers)
            //{

            //var roles = person.Roles.ToList();
            //var rolesList = String.Join(" | ", roles);

            //}


            var userList = currentUsers.ToList();
            ViewBag.Length = userList.Count();

            return View(userList);
        }


        //GET: Home/EditRoles
        [Authorize(Roles = "Admin")]
        public ActionResult EditRoles(string id)
        {
            var person = db.Users.Find(id);
            var roles = db.Roles;
            var userRoles = roles.Where(r => r.Users.Any(u => u.UserId == id));
            var userRolesList = (IEnumerable<IdentityRole>)userRoles;
            var roleList = (IEnumerable<IdentityRole>)roles;
            ViewBag.MyRoles = new SelectList(userRolesList, "Id", "Name");
            ViewBag.Roles = new SelectList(roleList, "Id", "Name");
            //var userRoles = person.Roles.Where(r => r.)
            //var urList = db.Roles.Where(r => r.Id == userRoles.);
            //ViewBag.MyRoles = urList;
            return View(person);
        }

        //POST: Home/EditRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoles(string user, string role, string operation)
        {
            if (!(operation == null || role == ""))
            {
                var r = db.Roles.Find(role).Name;
                if (operation == "Add")
                {
                    UHelper.AddUserToRole(user, r);
                }
                else
                {
                    UHelper.RemoveUserFromRole(user, r);
                }
            }
            db.SaveChanges();
            return RedirectToAction("EditRoles", "Home", user);
        }
    }
}
