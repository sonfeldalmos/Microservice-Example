using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataServiceInterface.Controllers
{
    /// <summary>
    /// Web API által generált objektum.
    /// A web service kezdőlapját kezeli.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Webservice kezdő lapja. Web API álatal generált.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = System.Configuration.ConfigurationManager.AppSettings["GV_HomePageTitle_GV"];

            return View();
        }
    }
}
