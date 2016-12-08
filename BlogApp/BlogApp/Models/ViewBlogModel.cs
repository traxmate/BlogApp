using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BlogApp.Models
{
	public class ViewBlogModel
	{
		public Blog Blog { get; set; }
		public IEnumerable<Post> FilteredPosts { get; set; }

		public ViewBlogModel(int blogId, int? year, int? month)
		{
			using (var db = new ApplicationDbContext())
			{
				try
				{
					Blog = db.Blogs
						.Where(a => a.Id == blogId)
						.Include(b => b.Posts.Select(c => c.Tags))
						.Include(a => a.Posts.Select(b => b.Comments))
						.Single();

					/* Filter posts by year and month.
					*/
					if(year.HasValue && month.HasValue)
					{
						var dt = new DateTime(year.Value, month.Value, 1);
						FilteredPosts = Blog.Posts.Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month).ToList();
					}
				}
				catch (Exception e)
				{
					Debug.WriteLine(e.Message);
				}
			}
		}
	}
}
