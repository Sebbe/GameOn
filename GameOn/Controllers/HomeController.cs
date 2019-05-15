using GameOn.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameOn.Web.Controllers
{
    /// <summary>
    /// The home controller for this website
    /// </summary>
    public class HomeController : Controller
    {
        private ITimelineService TimelineService { get; set; }

        public HomeController()
        {
            TimelineService = new TimelineService();
        }

        /// <summary>
        /// Home (index) page controller method
        /// </summary>
        public IActionResult Index()
        {
            ViewData.Model = TimelineService.Get();
            return View("Index_m");
        }
    }
}
