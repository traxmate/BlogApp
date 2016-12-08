using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models
{
	public class Comment
	{
		public int Id { get; set; }

		[Required]
		[StringLength(512)]
		public string Body { get; set; }

		[Required]
		public DateTime Created { get; set; }

		public string AuthorId { get; set; }
        public int PostId { get; set; }
    }
}
