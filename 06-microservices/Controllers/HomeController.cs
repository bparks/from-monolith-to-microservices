using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonolithToMicroservices.Models;

namespace MonolithToMicroservices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _posts;
        private readonly ILoggingStopwatch _stopwatch;

        public HomeController(ILogger<HomeController> logger, IPostService posts, ILoggingStopwatch stopwatch)
        {
            _logger = logger;
            _posts = posts;
            _stopwatch = stopwatch;
        }

        public IActionResult Results(int threshold, int skip = 0, int take = 0)
        {
            return _stopwatch.Run(() =>
            {
                var query = _posts.List(threshold, skip, take);

                return Json(query);
            });
        }

        #region ...

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion ...
    }
}
