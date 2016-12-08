using BlogApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace BlogApp.Controllers
{
	public class HitCounterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			/* Increase view for blog.
			*/
			using (var db = ApplicationDbContext.Create())
			{
				/* Get blog id from action parameter 'blogId'
				*/
				var blogIdParam = filterContext.Controller.ValueProvider.GetValue("blogId");
				int blogId = -1;
				if (int.TryParse(blogIdParam.AttemptedValue, out blogId))
				{
					// Increase view counter
					db.Blogs.Find(blogId).Views++;

					// Save changes.
					var message = ApplicationDbContext.SaveChanges(db);
				}
			}

			base.OnActionExecuted(filterContext);
		}
	}

	public class BlogsController : Controller
	{
		// GET: Blogs/ViewBlog?blogId=
		[HitCounter]
		public ActionResult ViewBlog(int blogId, int? year, int? month, int? from)
		{
			var model = new ViewBlogModel(blogId, year, month);
			if (model.Blog != null)
			{
				/* How many posts should be displayed.
				*/
				var page = from.HasValue ? from.Value : 0;
				var numPages = model.Blog.Posts.Count / 10;

				ViewBag.PostsToView = new KeyValuePair<int, int>(page, 10);
				ViewBag.Prev = 0;
				ViewBag.Next = numPages * 10;

				if (page != 0) ViewBag.Prev = page - 10;
				if (page != numPages * 10) ViewBag.Next = page + 10;

				return View(model);
			}
			else
			{
				return RedirectToAction("ViewBlogs");
			}
		}

		public ActionResult ViewUserBlogs(string userId)
		{
			using (var db = ApplicationDbContext.Create())
			{
				var userBlogs = db.Blogs
					.Include(a => a.Posts)
					.Where(a => a.Author == userId)
					.ToList();

				ViewBag.IsBlogOwner = User.Identity.GetUserId() == userId;
				ViewBag.BlogOwner = ApplicationDbContext.GetUserFromId(userId).GetName();

				return View("_ViewUserBlogs", userBlogs);
			}
		}

		public ActionResult Details(int blogId)
		{
			var model = new ViewBlogModel(blogId, null, null);

			ViewBag.IsBlogOwner = User.Identity.GetUserId() == model.Blog.Author;
			return View(model.Blog);
		}

		public ActionResult BrowseBlogs()
		{
			var model = new BrowseBlogsModel();
			return View(model);
		}

		public ActionResult GetAllTags()
		{
			var model = new BrowseBlogsModel();
			return Json(model.Tags, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetAllBlogs()
		{
			var model = new BrowseBlogsModel();
			return Json(model.Blogs, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetBlogPosts(int blogId)
		{
			using (var db = new ApplicationDbContext())
			{
				return Json(db.Blogs.Include(a => a.Posts).Where(b => b.Id == blogId).ToList(), JsonRequestBehavior.AllowGet);
			}
		}
	}
}