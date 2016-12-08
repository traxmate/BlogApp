using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogApp.Models;
using Microsoft.AspNet.Identity;

namespace BlogApp.Controllers
{
	public class CreateBlogsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: CreateBlogs
		public ActionResult Index()
		{
			/* Only display this users blogs.
			*/
			string author = User.Identity.GetUserId();
			//return View("_ViewUserBlogs",
			//	db.Blogs
			//	.Where(x => x.Author == author)
			//	.ToList()
			//	);

			return RedirectToAction("ViewUserBlogs", "Blogs", new { @userId = author, });
		}

		// GET: CreateBlogs/Create
		[Authorize]
		public ActionResult Create()
		{
			return View();
		}

		// POST: CreateBlogs/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Blog blog)
		{
			if (ModelState.IsValid)
			{
				db.Blogs.Add(blog);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(blog);
		}

		// GET: CreateBlogs/Edit/5
		[Authorize]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Blog blog = db.Blogs.Find(id);
			if (blog == null)
			{
				return HttpNotFound();
			}

			/* Only blog owner can edit the blog.
			*/
			if(blog.Author != User.Identity.GetUserId())
			{
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}

			return View(blog);
		}

		// POST: CreateBlogs/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Blog blog)
		{
			if (blog.Author != User.Identity.GetUserId())
			{
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			}
			else
			{
				if (ModelState.IsValid)
				{
					db.Entry(blog).State = EntityState.Modified;
					var message = ApplicationDbContext.SaveChanges(db);
					return RedirectToAction("Index");
				}
			}
			return View(blog);
		}

		// GET: CreateBlogs/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Blog blog = db.Blogs.Find(id);
			if (blog == null)
			{
				return HttpNotFound();
			}
			return View(blog);
		}

		// POST: CreateBlogs/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			// Get blog from db context
			var blog = db.Blogs.Find(id);

			/* Only blog author can delete the blog.
			*/
			if(User.Identity.GetUserId() != blog.Author)
			{
				return Redirect(Request.UrlReferrer.AbsoluteUri);
			}

			blog.Posts.ToList().ForEach(a =>
			{
				a.Comments.ToList().ForEach(b =>
				{
					db.Entry(b).State = EntityState.Deleted;
				});
				a.Tags.ToList().ForEach(b =>
				{
					db.Entry(b).State = EntityState.Deleted;
				});
				db.Entry(a).State = EntityState.Deleted;
			});

			db.Entry(blog).State = EntityState.Deleted;
			var message = ApplicationDbContext.SaveChanges(db);

			//Blog blog = db.Blogs.Find(id);
			//db.Blogs.Remove(blog);
			//db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
