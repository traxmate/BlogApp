using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models
{
	public class CommentsViewModel
	{
		public ICollection<Comment> Comments { get; set; }
		public int PostId { get; set; }
	}
}
