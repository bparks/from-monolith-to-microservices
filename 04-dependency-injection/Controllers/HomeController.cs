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
        private readonly BloggingContext _db;
        private readonly ILoggingStopwatch _stopwatch;

        public HomeController(ILogger<HomeController> logger, BloggingContext db, ILoggingStopwatch stopwatch)
        {
            _logger = logger;
            _db = db;
            _stopwatch = stopwatch;
        }

        public IActionResult Results(int threshold, int skip = 0, int take = 0)
        {
            return _stopwatch.Run(() =>
            {
                var query = _db.Posts.Where(post => post.PostId > threshold);

                if (skip > 0)
                    query = query.Skip(skip);

                if (take > 0)
                    query = query.Take(take);

                return Json(query.ToList());
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
