using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BlogApp.Models
{
    public class PostTag : Tag
    {
		public PostTag(string name) : base(name)
		{

		}
        //public int Id { get; set; }
        //public virtual Tag Tag { get; set; }
    }
}