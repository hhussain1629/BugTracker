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
                    DisplayName = "Hammad Hussain"
                }, "password");
            }
            var userId = userManager.FindByEmail("hhussain1629@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            if (!context.Users.Any(u => u.Email == "ajensen@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ajensen@coderfoundry.com",
                    Email = "ajensen@coderfoundry.com",
                    FirstName = "Andrew",
                    LastName = "Jensen",
                    DisplayName = "Andrew Jensen"
                }, "password");
            }
            userId = userManager.FindByEmail("ajensen@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Project Manager");

            if (!context.Users.Any(u => u.Email == "araynor@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "araynor@coderfoundry.com",
                    Email = "araynor@coderfoundry.com",
                    FirstName = "Antonio",
                    LastName = "Raynor",
                    DisplayName = "Antonio Raynor"
                }, "password");
            }
            userId = userManager.FindByEmail("araynor@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Project Manager");

            if (!context.Users.Any(u => u.Email == "jsmith@anaddress.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jsmith@anaddress.com",
                    Email = "jsmith@anaddress.com",
                    FirstName = "John",
                    LastName = "Smith",
                    DisplayName = "John Smith"
                }, "password");
            }
            userId = userManager.FindByEmail("jsmith@anaddress.com").Id;
            userManager.AddToRole(userId, "Developer");

            if (!context.Users.Any(u => u.Email == "aguy@address2.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "aguy@address2.com",
                    Email = "aguy@address2.com",
                    FirstName = "Another",
                    LastName = "Guy",
                    DisplayName = "Another Guy"
                }, "password");
            }
            userId = userManager.FindByEmail("aguy@address2.com").Id;
            userManager.AddToRole(userId, "Submitter");

            if (!context.Users.Any(u => u.Email == "mthomas@anaddress.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "mthomas@anaddress.com",
                    Email = "mthomas@anaddress.com",
                    FirstName = "Mark",
                    LastName = "Thomas",
                    DisplayName = "Mark Thomas"
                }, "password");
            }
            userId = userManager.FindByEmail("mthomas@anaddress.com").Id;
            userManager.AddToRole(userId, "Submitter");

            if (!context.Users.Any(u => u.Email == "pjenkins@anaddress.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "pjenkins@anaddress.com",
                    Email = "pjenkins@anaddress.com",
                    FirstName = "Paul",
                    LastName = "Jenkins",
                    DisplayName = "Paul Jenkins"
                }, "password");
            }
            userId = userManager.FindByEmail("pjenkins@anaddress.com").Id;
            userManager.AddToRole(userId, "Developer");

            if (!context.Users.Any(u => u.Email == "jbrown@anaddress.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jbrown@anaddress.com",
                    Email = "jbrown@anaddress.com",
                    FirstName = "Jennifer",
                    LastName = "Brown",
                    DisplayName =  "Jennifer Brown"
                }, "password");
            }
            userId = userManager.FindByEmail("jbrown@anaddress.com").Id;
            userManager.AddToRole(userId, "Developer");

            if (!context.Users.Any(u => u.Email == "bli@anaddress.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "bli@anaddress.com",
                    Email = "bli@anaddress.com",
                    FirstName = "Bruce",
                    LastName = "Li",
                    DisplayName = "Bruce Li"
                }, "password");
            }
            userId = userManager.FindByEmail("bli@anaddress.com").Id;
            userManager.AddToRole(userId, "Developer");

            if (!context.Users.Any(u => u.Email == "gcromwell@anaddress.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "gcromwell@anaddress.com",
                    Email = "gcromwell@anaddress.com",
                    FirstName = "George",
                    LastName = "Cromwell",
                    DisplayName = "George Cromwell"
                }, "password");
            }
            userId = userManager.FindByEmail("gcromwell@anaddress.com").Id;
            userManager.AddToRole(userId, "Submitter");

            if (!context.Users.Any(u => u.Email == "ajones@anaddress.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ajones@anaddress.com",
                    Email = "ajones@anaddress.com",
                    FirstName = "Allison",
                    LastName = "Jones",
                    DisplayName = "Allison Jones"
                }, "password");
            }
            userId = userManager.FindByEmail("ajones@anaddress.com").Id;
            userManager.AddToRole(userId, "Submitter");

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

        //  You can use the DbSet<T>.AddOrUpdate() UHelper extension method 
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

