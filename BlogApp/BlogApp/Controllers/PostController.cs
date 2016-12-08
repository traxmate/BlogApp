using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Diagnostics;

namespace BlogApp.Models
{
	[Authorize]
	public class PostController : Controller
	{
		// GET: AddPost
		public ActionResult AddPost(int blogId)
		{
			Blog blog = null;
			using (var db = new ApplicationDbContext())
			{
				blog = db.Blogs.Include("Posts").Where(a => a.Id == blogId).Single();
			}

			if (blog != null && User.Identity.GetUserId() == blog.Author)
			{
				return View(blog);
			}
			else
			{
				return RedirectToAction("Index", "CreateBlogs");
			}
		}

		[HttpPost]
		public ActionResult AddPost(Post post, string tags, int blogId)
		{
			using (var db = new ApplicationDbContext())
			{
				// Set date
				post.Date = DateTime.UtcNow;

				// Insert <br> tags on new lines
				post.Body = post.Body.Replace("\n", "<br />");

				// Allow only <br> tags
				post.Body = post.Body
					.Replace("<", "&lt;")
					.Replace("&lt;br />", "<br />");

				/* Set tags. */
				SetPostTags(ref post, tags);

				// What blog is this post to
				post.Blog = db.Blogs.Include("Posts").Where(a => a.Id == blogId).Single();

				/* Only add post if it is the owner of the blog who posts.
				*/
				if (User.Identity.GetUserId() == post.Blog.Author)
				{
					// Add post to blog
					post.Blog.Posts.Add(post);

					// Add post to db context
					db.Entry(post).State = System.Data.Entity.EntityState.Added;

					// Save database
					db.SaveChanges();
				}
			}

			return RedirectToAction("ViewBlog", "Blogs", new { @blogId = blogId });
		}

		private static void SetPostTags(ref Post post, string tags)
		{
			post.Tags = new List<Tag>();
			foreach (var tag in tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
			{
				// Only add a tag once
				if (post.Tags.Where(a => a.Name == tag).Count() == 0)
				{
					post.Tags.Add(new Tag(tag));
				}
			}
		}

		public ActionResult Edit(int postId)
		{
			/* Only the author of the blog may edit posts.
			*/
			using (var db = ApplicationDbContext.Create())
			{
				var post = db.Posts.Where(a => a.Id == postId).Include("Tags").Single();
				var postAuthor = post.Blog.Author;

				if (User.Identity.GetUserId() != postAuthor)
				{
					return Redirect(Request.UrlReferrer.AbsoluteUri);
				}

				return View(post);
			}
		}

		[HttpPost]
		public ActionResult Edit(int postId, string title, string body, string tags)
		{
			using (var db = ApplicationDbContext.Create())
			{
				// Get post from db context
				var post = db.Posts.Find(postId);

				/* Only the author of the blog are allowed to edit the post.
				*/
				if (User.Identity.GetUserId() == post.Blog.Author)
				{
					/* Modify post values.
					*/
					post.Title = title;
					post.Body = body;
					SetPostTags(ref post, tags);

					// Insert <br> tags on new lines
					post.Body = post.Body.Replace("\n", "<br />");

					// Allow only <br> tags
					post.Body = post.Body
						.Replace("<", "&lt;")
						.Replace("&lt;br />", "<br />");
					
					// Set state to modified
					db.Entry(post).State = EntityState.Modified;

					/* Check for errors.
					*/
					var errors = ApplicationDbContext.SaveChanges(db);
					if (!String.IsNullOrEmpty(errors))
					{
						Debug.WriteLine(errors);
						ViewBag.ErrorMessage = errors;
					}
				}

				return RedirectToAction("ViewBlog", "Blogs", new { @blogId = post.Blog.Id });
			}
		}

		[HttpPost]
		public ActionResult AddComment(Comment comment)
		{
			var userName = "";
			using (var db = new ApplicationDbContext())
			{
				// Add date on creation
				comment.Created = DateTime.UtcNow;

				// Add comment to post
				var post = db.Posts
					.Include(x => x.Blog)
					.Where(a => a.Id == comment.PostId)
					.Single();
				post.Comments.Add(comment);

				userName = db.Users.Find(comment.AuthorId).GetName();

				// Add to db context
				db.Entry(post).State = System.Data.Entity.EntityState.Modified;

				// Save
				ApplicationDbContext.SaveChanges(db);
			}

			/* Create html code.
			*/
			string html =
				$@"	<div class='row well comment-content' id='{comment.Id}'>
						<div class='pull-left comment-text'>
							{comment.Body}
							<div class='pull-right text-right'>
								By <strong>{userName}</strong><br />
								{comment.Created.ToLocalTime().ToShortDateString()}
								{comment.Created.ToLocalTime().ToShortTimeString()}
							</div>
						</div>

						<a class='give-pointer' ng-model='deleteComment' ng-click='commentCtrl.Delete('{User.Identity.GetUserId()}', {comment.Id})'>Delete</a>
					</div>";

			return Json(new
			{
                Html = html,
			});
		}

		[HttpDelete]
		public ActionResult DeleteComment(string userId, int commentId)
		{
			try
			{
				using (var db = new ApplicationDbContext())
				{
					var user = db.Users.Find(userId);
					var comment = db.Comments.Find(commentId);
					var post = db.Posts.Find(comment.PostId);

					/* Only comment author and blog post author may delete a comment.
					*/
					if(user.Id == comment.AuthorId || user.Id == post.Blog.Author)
					{
						db.Entry(comment).State = EntityState.Deleted;
						string error = ApplicationDbContext.SaveChanges(db);

						if(!String.IsNullOrEmpty(error))
						{
							throw new Exception(error);
						}

						return Json(new { Success = true, });
					}
					else
					{
						throw new Exception("Only the user who posted the comment or the blog post author may delete this comment.");
					}
				}
			}
			catch(Exception e)
			{
				return Json(new { Success = false, Message = e.Message, });
			}
		}

		[HttpDelete]
		public ActionResult Delete(int postId)
		{
			int blogId = 0;
			using (var db = ApplicationDbContext.Create())
			{
				// Get post
				var post = db.Posts.Find(postId);

				// Get blog id
				blogId = post.Blog.Id;

				/* Only author of post may delete it.
				*/
				if(User.Identity.GetUserId() != post.Blog.Author)
				{
					return Redirect(Request.UrlReferrer.AbsoluteUri);
				}

				/* Mark post tags as deleted.
				*/
				post.Tags
					.ToList()
					.ForEach(x => db.Entry(x).State = EntityState.Deleted);

				// Mark post as deleted
				db.Entry(post).State = EntityState.Deleted;

				// Save
				db.SaveChanges();
			}

			return RedirectToAction("ViewBlog", "Blogs", new { @blogId = blogId });
		}
	}
}