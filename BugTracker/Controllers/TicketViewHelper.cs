using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class TicketViewHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public TicketViewModel ViewModelFromDataModel(Ticket ticket)
        {
            var ticketView = new TicketViewModel(); 
            ticketView.Id = ticket.Id;
            ticketView.Title = ticket.Title;
            ticketView.Description = ticket.Description;
            ticketView.Created = ticket.Created;
            ticketView.Updated = ticket.Updated;
            ticketView.ProjectName = db.Projects.Find(ticket.ProjectId).Name;
            ticketView.TicketTypeName = db.TicketTypes.Find(ticket.TicketTypeId).Name;
            ticketView.TicketPriorityName = db.TicketPriorities.Find(ticket.TicketPriorityId).Name;
            ticketView.TicketStatusName = db.TicketStatus.Find(ticket.TicketStatusId).Name;
            ticketView.OwnerUserName = db.Users.Find(ticket.OwnerUserId).UserName;
            ticketView.AssignedToUserName = db.Users.Find(ticket.AssignedToUserId).UserName;
            return ticketView;
        }
    }
}