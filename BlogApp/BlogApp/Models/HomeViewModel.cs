using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models
{
	public class HomeViewModel
	{
		// 10 latest posts
		public IEnumerable<Post> Posts { get; set; }

		// 10 newest blogs
		public IEnumerable<Blog> Blogs { get; set; }

		// 10 newest users
		public IEnumerable<ApplicationUser> Users { get; set; }

		public HomeViewModel()
		{
			using (var db = ApplicationDbContext.Create())
			{
				Posts = db.Posts.OrderByDescending(a => a.Date).Take(10).ToList();
				Blogs = db.Blogs.OrderByDescending(a => a.Date).Take(10).ToList();
				Users = db.Users.OrderByDescending(a => a.Id).Take(10).ToList();
			}
		}
	}
}
