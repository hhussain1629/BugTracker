using BugTracker;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;



namespace BugTracker.Models
{
    public class TicketNotification
    {
        //public TicketNotification()
        //{
        //   this.UserId = new HashSet<string>();
        //}
        public int Id { get; set; }
        public virtual int TicketId { get; set; }
        public virtual string UserId { get; set; }
    }
}