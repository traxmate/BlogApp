using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models
{
	public class SearchModel
	{
		public string SearchTerm { get; set; }
		public IList<Blog> BlogsFound { get; set; }
		public IList<Post> PostsFound { get; set; }
		public IList<ApplicationUser> UsersFound { get; set; }

		public SearchModel(string term)
		{
			SearchTerm = term;
			BlogsFound = new List<Blog>();

			if (term != null)
			{
				term = term.ToLower();
				using (var db = new ApplicationDbContext())
				{
					// Search for users
					UsersFound = new List<ApplicationUser>();
					db.Users.ToList().ForEach(a =>
					{
						if (a.GetName().ToLower().Contains(term))
						{
							UsersFound.Add(a);
						}
					});

					// Search blogs
					BlogsFound = db.Blogs
						.Where(a => a.Title.ToLower().Contains(term) || a.Body.ToLower().Contains(term))
						.ToList();

					// Search posts
					PostsFound = db.Posts
						.Include("Blog")
						.Where(a => a.Title.ToLower().Contains(term) || a.Body.ToLower().Contains(term))
						.ToList();
				}
			}
		}
	}
}
