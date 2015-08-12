using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace BugTracker.Models
{
    public class UserProjectsHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IList<Project> ListProjects()
        {
            return db.Projects.ToList();
        }

        public bool IsUserOnProject(string userId, int projectId)
        {
            return db.Projects.SingleOrDefault(p => p.Id == projectId).Users.Any(u => u.Id == userId);
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                var project = db.Projects.Find(projectId);
                project.Users.Add(db.Users.Find(userId));
                db.SaveChanges();
                //db.Entry(project).State = EntityState.Modified; ---> Does above line, but faster.
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                var project = db.Projects.Find(projectId);
                project.Users.Remove(db.Users.Find(userId));
                db.SaveChanges();
                //db.Entry(project).State = EntityState.Modified; ---> Does above line, but faster.
            }
        }

        public ICollection<Project> ListProjectsForUser(string userId)
        {
            return db.Users.Find(userId).Projects;
        }

        public ICollection<ApplicationUser> ListUsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }

        public ICollection<Project> ListProjectsNotForUser(string userId)
        {
            //var projectList = ListProjects();
            //var result = new List<Project>();
            //foreach (var project in projectList)
            //{
            //    if (!IsUserOnProject(userId, project.Id))
            //        result.Add(project);
            //}
            //return result;
            return db.Projects.Where(p => p.Users.All(u => u.Id != userId)).ToList();
        }

        public ICollection<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            //var userList = new List<ApplicationUser>();
            //var result = new List<ApplicationUser>();
            //foreach (var user in userList)
            //{
            //    if (!IsUserOnProject(user.Id, projectId))
            //        result.Add(user);
            //}
            //return result;
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }
    }
}