using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;

namespace BlogApp.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string Name { get; set; }

		public Tag()
		{
			Name = "";
		}

		public Tag(string name)
		{
			Name = name;
		}
    }
}