using BugTracker;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;



namespace BugTracker.Models
{
    [Table("TicketHistories")]
    public class TicketHistory
    {
        public int Id { get; set; }
        public virtual int TicketId { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTimeOffset Changed { get; set; }
        public virtual string UserId { get; set; }
    }
}