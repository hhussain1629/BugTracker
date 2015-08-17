using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace BugTracker.Models
{
    public class UserRolesHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        
        public bool IsUserInRole(string userId, string roleName)
        {
            return manager.IsInRole(userId, roleName);
        }
        public IList<string> ListUserRoles(string userId)
        {
            return manager.GetRoles(userId);
        }
        public bool AddUserToRole (string userId, string roleName)
        {
            var result = manager.AddToRole(userId, roleName);
            return result.Succeeded;
        }
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = manager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }
        public IList<ApplicationUser> UsersInRole (string roleName)
        {
            var db = new ApplicationDbContext();
            var resultList = new List<ApplicationUser>();
            foreach (var user in db.Users)
            {
                if (IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
        public IList<ApplicationUser>UsersNotInRole (string roleName)
        {
            var resultList = new List<ApplicationUser>();
            foreach (var user in manager.Users)
                if (!IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            return resultList;
        }
        //public string DisplayRolesText (ICollection<IdentityUserRole>userRoles)
        //{
        //    var rolesText = "";
        //    foreach (var role in userRoles)
        //    {
        //        if (rolesText != "")
        //        {
        //            rolesText = rolesText + ", ";
        //        }
        //        rolesText = rolesText + db.Roles.SingleOrDefault(r => r.Id == role.RoleId).Name;
        //    }
        //    return rolesText;
        //}
            
       
    }
}