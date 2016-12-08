using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System;
using System.Data.Entity.Infrastructure;

namespace BlogApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

		[Required]
		[StringLength(64)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(64)]
		public string LastName { get; set; }
		
		[StringLength(64)]
		public string Nickname { get; set; }
		
		public DateTime Joined { get; set; }

		public string GetName()
		{
			if(!String.IsNullOrEmpty(Nickname))
			{
				return Nickname;
			}
			else
			{
				return FirstName + " " + LastName;
			}
		}
	}

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
		public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

		public static ApplicationUser GetUserFromId(string userId)
		{
			return Create().Users.Find(userId);
		}

		public static string SaveChanges(ApplicationDbContext db)
		{
			string message = "";

			try
			{
				db.SaveChanges();
			}
			catch (DbEntityValidationException e)
			{
				foreach (var item in e.EntityValidationErrors)
				{
					foreach (var error in item.ValidationErrors)
					{
						message += error.PropertyName + ": " + error.ErrorMessage;
					}
				}
			}
			catch(DbUpdateException e)
			{
				message += e.Message + ":" + e.InnerException.Message;
			}

			return message;
		}
	}
}