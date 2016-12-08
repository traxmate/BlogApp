using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class BrowsePosts
    {
        public IList<Blog> FoundBlogs;
        public List<Tag> FoundTags = new List<Tag>();
        public BrowsePosts()
        {
            using (var db = new ApplicationDbContext())
            {
                FoundBlogs = db.Blogs.OrderByDescending(b => b.Views).ToList();

                foreach (var tag in db.Tags)
                {
                    if (FoundTags.Where(t => t.Name == tag.Name).Count() > 0)
                    {

                    }
                    else
                    {
                        FoundTags.Add(tag);
                    }
                }
            }
        }
    }
}
