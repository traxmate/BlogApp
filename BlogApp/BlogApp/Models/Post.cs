using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BlogApp.Models
{
    public class Post
    {
        public int Id { get; set; }

		[Required]
		public virtual Blog Blog { get; set; }
		
		public virtual ICollection<Comment> Comments { get; set; }

		[Required]
        [StringLength(512)]
        public string Title { get; set; }

        [Required]
		[AllowHtml]
        public string Body { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Views { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

    }
}