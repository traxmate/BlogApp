using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models
{
	public class ViewBlogsModel
	{
		public IEnumerable<Blog> Blogs { get; set; }

		/* Get all blogs from a specific user.
		*/
		public ViewBlogsModel(string userId)
		{
			using (var db = new ApplicationDbContext())
			{
				Blogs = db.Blogs.Where(a => a.Author == userId).Select(b => b).ToList();
			}
		}
	}
}
