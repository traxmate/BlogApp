using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models
{
	public class BlogData
	{
		public string Title { get; set; }
		public string AuthorName { get; set; }
		public string AuthorId { get; set; }
		public int Views { get; set; }
		public int BlogId { get; set; }
		public string LocalLongDate { get; set; }
	};

	public class BrowseBlogsModel
	{
		public IEnumerable<BlogData> Blogs { get; set; }
		public IEnumerable<KeyValuePair<Tag, int>> Tags { get; set; }
		
		public BrowseBlogsModel()
		{
			using (var db = new ApplicationDbContext())
			{
				//// Get all blogs ordered by their view count
				//Blogs = db.Blogs.OrderByDescending(a => a.Views).ToList();

				var blogs = new List<BlogData>();
				foreach (var blog in db.Blogs.OrderByDescending(a => a.Views))
				{
					blogs.Add(new BlogData
					{
						AuthorName = ApplicationDbContext.GetUserFromId(blog.Author).GetName(),
						AuthorId = blog.Author,
						Title = blog.Title,
						Views = blog.Views,
						BlogId = blog.Id,
						LocalLongDate = blog.Date.ToLocalTime().ToLongDateString(),
					});
				}
				Blogs = blogs;

				/* Get all tags sorted by most used
				*/
				var tags = new List<KeyValuePair<Tag, int>>();
				foreach (var tag in db.Tags.Distinct().ToList())
				{
					if (tags.Where(a => a.Key.Name == tag.Name).Count() == 0)
					{
						tags.Add(new KeyValuePair<Tag, int>(tag, db.Tags.Count(a => a.Name == tag.Name)));
					}
				}
				Tags = tags.OrderByDescending(a => a.Value).ToList();
			}
		}
	}
}
