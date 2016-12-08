using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApp.Models;

namespace BlogApp.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var model = new HomeViewModel();
			return View(model);
		}

        public ActionResult BrowseBlogs()
        {
            var Browse = new BrowsePosts();
            return View(Browse);
        }
        
        [Authorize(Roles = "Admin")]
		public ActionResult Manage()
		{
			return View();
		}
	}
}