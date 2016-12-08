using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogApp.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Search(string filter)
        {
			// If filter starts with # we are looking for tags
			if(!String.IsNullOrEmpty(filter) && filter[0] == '#')
			{
				return RedirectToAction("SearchTags", "SearchTags", new { @tag = filter });
			}

			var model = new SearchModel(filter);
            return View(model);
        }
    }
}