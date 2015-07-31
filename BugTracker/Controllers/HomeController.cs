using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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
        public ActionResult PersonnelView()
        {
            var db = new ApplicationDbContext();
            var personelView = new PersonnelView();
            foreach (var item in db.Users) {
                personelView.Id = item.Id;
                personelView.FirstName = item.FirstName;
                personelView.LastName = item.LastName;
                personelView.DisplayName = item.DisplayName;
                personelView.Role = item.Roles.; 



            }

            return View();
        }
    }
}