using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BlogApp.Models
{
	public enum SearchTagsSorting
	{
		DateNewest = 0,
		DateOldest,
		Alphabetical,
		AlphabeticalReversed,
		Blog,
		Author,
	};

	public enum TagFilter
	{
		Any = 0,
		All,
	}

	public class SearchTagsModel
	{
		public string Tags { get; set; }
		public IEnumerable<Post> PostsFound { get; set; }
		public SearchTagsSorting Sorting { get; set; }

		public SearchTagsModel(string tags, SearchTagsSorting sorting, TagFilter filter)
		{
			// Remove # from string
			Tags = tags.Replace("#", "");

			/* Create array with separate tags
			*/
			var tagsArr = Tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			/* Search for posts with these tags.
			*/
			using (var db = ApplicationDbContext.Create())
			{
				var posts = new List<Post>();

				foreach (var post in db.Posts.Include(a => a.Tags).Include(a => a.Blog))
				{
					var _tags = post.Tags.Select(a => a.Name);
					if (filter == TagFilter.All && !tagsArr.Except(_tags).Any())
					{
						posts.Add(post);
					}
					else if(filter == TagFilter.Any)
					{

						foreach (var tag in post.Tags)
						{
							if (tagsArr.Contains(tag.Name))
							{
								posts.Add(post);
							}
						}
					}
				}

				/* Sort result.
				*/
				switch(sorting)
				{
					case SearchTagsSorting.DateNewest:
						PostsFound = posts.OrderByDescending(x => x.Date);
						break;

					case SearchTagsSorting.DateOldest:
						PostsFound = posts.OrderBy(x => x.Date);
						break;

					case SearchTagsSorting.Alphabetical:
						PostsFound = posts.OrderBy(x => x.Title);
						break;

					case SearchTagsSorting.AlphabeticalReversed:
						PostsFound = posts.OrderByDescending(x => x.Title);
						break;

					case SearchTagsSorting.Blog:
						PostsFound = posts.OrderBy(x => x.Blog.Title);
						break;

					case SearchTagsSorting.Author:
						PostsFound = posts.OrderBy(x => ApplicationDbContext.GetUserFromId(x.Blog.Author).GetName());
						break;
				}

				PostsFound = PostsFound.Distinct();
			}
		}
	}
}
