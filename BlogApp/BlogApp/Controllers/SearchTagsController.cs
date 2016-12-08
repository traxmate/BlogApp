using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogApp.Controllers
{
    public class SearchTagsController : Controller
    {
        // GET: SearchTags
        public ActionResult SearchTags(string tag)
        {
			var model = new SearchTagsModel(tag, SearchTagsSorting.DateNewest, TagFilter.Any);
            return View(model);
        }

		// GET: SearchTags/OrderBy?=tags
		public ActionResult OrderBy(string tags, int sort)
		{
			var model = new SearchTagsModel(tags, (SearchTagsSorting)sort, TagFilter.Any);
			return View("SearchTags", model);
		}

		public ActionResult Filter(string tags, int filter)
		{
			var model = new SearchTagsModel(tags, SearchTagsSorting.DateNewest, (TagFilter)filter);
			return View("SearchTags", model);
		}
    }
}