using BugTracker;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;



namespace BugTracker.Models
{
    public class Ticket
    {
        public Ticket()
        {
            this.Comments = new HashSet<TicketComment>();
            this.Attachments = new HashSet<TicketAttachment>();
            this.Histories = new HashSet<TicketHistory>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public Nullable<DateTimeOffset> Updated { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual int TicketTypeId { get; set; }
        public virtual int TicketPriorityId { get; set; }
        public virtual int TicketStatusId { get; set; }
        public virtual string OwnerUserId { get; set; }
        public virtual string AssignedToUserId { get; set; }
        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketAttachment> Attachments { get; set; }
        public virtual ICollection<TicketHistory> Histories { get; set; }
    }
}