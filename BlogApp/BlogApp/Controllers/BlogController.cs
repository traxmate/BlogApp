using BlogApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogApp.Controllers
{
	public class BlogController : Controller
	{
		// GET: Blog
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Create()
		{
			var model = new Blog();
			return View(model);
		}

		[HttpPost]
		public ActionResult Create(Blog blog)
		{
			using (var db = new ApplicationDbContext())
			{
				// Get date of creation
				blog.Date = DateTime.UtcNow;

				// Set author of blog
				var userId = User.Identity.GetUserId();
				blog.Author = db.Users.Where(a => a.Id == userId).Select(b => b).Single();

				// Add blog object to the database
				db.Entry(blog).State = System.Data.Entity.EntityState.Added;

				// Save database
				db.SaveChanges();
			}

			// Return to index page
			return View("Index");
		}

		[HttpPost]
		public ActionResult Modify(Blog blog)
		{
			using (var db = new ApplicationDbContext())
			{
				// Get date
				blog.Date = DateTime.UtcNow;

				// Set blog state as modified
				db.Entry(blog).State = System.Data.Entity.EntityState.Modified;

				// Save database
				db.SaveChanges();
			}

			// Return to index page
			return View("Index");
		}
	}
}