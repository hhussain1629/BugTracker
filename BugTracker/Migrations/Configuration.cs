namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "BugTracker.Models.ApplicationDbContext";
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //Roles Seed
            //if (System.Diagnostics.Debugger.IsAttached == false)
             //  System.Diagnostics.Debugger.Launch();

            var roleManager = new Microsoft.AspNet.Identity.RoleManager<IdentityRole>(new Microsoft.AspNet.Identity.EntityFramework.RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

      

            //Users Seed
            var userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            ApplicationUser me = new ApplicationUser();

            //me = context.Users.FirstOrDefault(m => m.Email == "hhussain1629@gmail.com");

            if (!context.Users.Any(u => u.Email == "hhussain1629@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "hhussain1629@gmail.com",
                    Email = "hhussain1629@gmail.com",
                    FirstName = "Hammad",
                    LastName = "Hussain",
                    DisplayName = "Hammad"
                }, "password");
            }
            var userId = userManager.FindByEmail("hhussain1629@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            if (!context.Users.Any(u => u.Email == "ajensen@coderfoundry"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ajensen@coderfoundry",
                    Email = "ajensen@coderfoundry",
                    FirstName = "Andrew",
                    LastName = "Jensen",
                    DisplayName = "Andrew"
                }, "password");
            }
            userId = userManager.FindByEmail("ajensen@coderfoundry").Id;
            userManager.AddToRole(userId, "Project Manager");

            if (!context.Users.Any(u => u.Email == "araynor@coderfoundry"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "araynor@coderfoundry",
                    Email = "araynor@coderfoundry",
                    FirstName = "Antonio",
                    LastName = "Raynor",
                    DisplayName = "Antonio"
                }, "password");
            }
            userId = userManager.FindByEmail("araynor@coderfoundry").Id;
            userManager.AddToRole(userId, "Project Manager");

            //TicketStatus Seed
            if (!context.TicketStatus.Any(s => s.Name == "Open"))
            {
                TicketStatus ts = new TicketStatus();
                ts.Name = "Open";
                context.TicketStatus.Add(ts);
                context.SaveChanges();
            }
            if (!context.TicketStatus.Any(s => s.Name == "Rejected"))
            {
                TicketStatus ts = new TicketStatus();
                ts.Name = "Rejected";
                context.TicketStatus.Add(ts);
                context.SaveChanges();
            }
            if (!context.TicketStatus.Any(s => s.Name == "Assigned"))
            {
                TicketStatus ts = new TicketStatus(); 
                ts.Name = "Assigned";
                context.TicketStatus.Add(ts);
                context.SaveChanges();
            }
            if (!context.TicketStatus.Any(s => s.Name == "In process"))
            {
                TicketStatus ts = new TicketStatus();
                ts.Name = "In process";
                context.TicketStatus.Add(ts);
                context.SaveChanges();
            }
            if (!context.TicketStatus.Any(s => s.Name == "Unable to reproduce"))
            {
                TicketStatus ts = new TicketStatus();
                ts.Name = "Unable to reproduce";
                context.TicketStatus.Add(ts);
                context.SaveChanges();
            }
            if (!context.TicketStatus.Any(s => s.Name == "On hold"))
            {
                TicketStatus ts = new TicketStatus();
                ts.Name = "On hold";
                context.TicketStatus.Add(ts);
                context.SaveChanges();
            }
            if (!context.TicketStatus.Any(s => s.Name == "Done pending review"))
            {
                TicketStatus ts = new TicketStatus();
                ts.Name = "Done pending review";
                context.TicketStatus.Add(ts);
                context.SaveChanges();
            }
            if (!context.TicketStatus.Any(s => s.Name == "Terminate"))
            {
                TicketStatus ts = new TicketStatus();
                ts.Name = "Terminate";
                context.TicketStatus.Add(ts);
                context.SaveChanges();
            }
            if (!context.TicketStatus.Any(s => s.Name == "Resolved"))
            {
                TicketStatus ts = new TicketStatus();
                ts.Name = "Resolved";
                context.TicketStatus.Add(ts);
                context.SaveChanges();
            }

            //TicketPriorities Seed
            if (!context.TicketPriorities.Any(p => p.Name == "Urgent"))
            {
                TicketPriority tp = new TicketPriority();
                tp.Name = "Urgent";
                context.TicketPriorities.Add(tp);
                context.SaveChanges();
            }
            if (!context.TicketPriorities.Any(p => p.Name == "Essential"))
            {
                TicketPriority tp = new TicketPriority();
                tp.Name = "Essential";
                context.TicketPriorities.Add(tp);
                context.SaveChanges();
            }
            if (!context.TicketPriorities.Any(p => p.Name == "Desirable"))
            {
                TicketPriority tp = new TicketPriority();
                tp.Name = "Desirable";
                context.TicketPriorities.Add(tp);
                context.SaveChanges();
            }
            if (!context.TicketPriorities.Any(p => p.Name == "Optional"))
            {
                TicketPriority tp = new TicketPriority();
                tp.Name = "Optional";
                context.TicketPriorities.Add(tp);
                context.SaveChanges();
            }

            //TicketTypes Seed
            if (!context.TicketTypes.Any(t => t.Name == "Bug"))
            {
                TicketType tt = new TicketType();
                tt.Name = "Bug";
                context.TicketTypes.Add(tt);
                context.SaveChanges();
            }
            if (!context.TicketTypes.Any(t => t.Name == "Enhancement"))
            {
                TicketType tt = new TicketType();
                tt.Name = "Enhancement";
                context.TicketTypes.Add(tt);
                context.SaveChanges();
            }
            if (!context.TicketTypes.Any(t => t.Name == "Other modification"))
            {
                TicketType tt = new TicketType();
                tt.Name = "Other modification";
                context.TicketTypes.Add(tt);
                context.SaveChanges();
            }
            if (!context.TicketTypes.Any(t => t.Name == "To be determined"))
            {
                TicketType tt = new TicketType();
                tt.Name = "To be determined";
                context.TicketTypes.Add(tt);
                context.SaveChanges();
            }
        }



        //  This method will be called after migrating to the latest version.

        //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
        //  to avoid creating duplicate seed data. E.g.
        //
        //    context.People.AddOrUpdate(
        //      p => p.FullName,
        //      new Person { FullName = "Andrew Peters" },
        //      new Person { FullName = "Brice Lambson" },
        //      new Person { FullName = "Rowan Miller" }
        //    );
        //
    }
}

