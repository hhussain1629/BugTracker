using BugTracker;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Models
{
    public class PersonnelView
    {
        public string Id { get; set;}
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string DisplayName { get; set; }
        public string Email { get; set;}
        public IList<string> Roles { get; set; }
    }
}