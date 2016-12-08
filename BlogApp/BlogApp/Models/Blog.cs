using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace BlogApp.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Required]
		public string Author { get; set; }

		[Required]
        [StringLength(32)]
        public string Title { get; set; }

        [Required]
        [StringLength(512)]
		[Display(Name = "Description")]
        public string Body { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Views { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

		public static IEnumerable<Blog> GetBlogsFromUser(string userId, ApplicationDbContext db)
		{
			return db.Blogs.Where(a => a.Author == userId).Select(b => b);
		}
	}
}