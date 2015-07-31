using BugTracker;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public virtual ICollection<Project>Projects { get; set; }

        public ApplicationUser()
        {
            this.Projects = new HashSet<Project>();
            //this.TicketNotifications = new HashSet<TicketNotification>();
            //this.Tickets = new HashSet<Ticket>();
            //this.Comments = new HashSet<TicketComment>();
            //this.Attachments = new HashSet<TicketAttachment>();
            //this.Histories = new HashSet<TicketHistory>();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        //public virtual ICollection<Project>Projects { get; set; }
        //public virtual ICollection<TicketNotification>TicketNotifications { get; set; }
        //public virtual ICollection<Ticket> Tickets { get; set; }
        //public virtual ICollection<TicketComment> Comments { get; set; }
        //public virtual ICollection<TicketAttachment> Attachments { get; set; }
        //public virtual ICollection<TicketHistory> Histories { get; set; }

        
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<TicketNotification> TicketNotifications { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}