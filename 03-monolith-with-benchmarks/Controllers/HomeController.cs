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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Results(int threshold, int skip = 0, int take = 0)
        {
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();

                using (var context = new BloggingContext())
                {
                    var query = context.Posts.Where(post => post.PostId > threshold);

                    if (skip > 0)
                        query = query.Skip(skip);

                    if (take > 0)
                        query = query.Take(take);

                    return Json(query.ToList());
                }
            }
            finally
            {
                stopwatch.Stop();
                
                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
                _logger.LogInformation($"Method took {elapsedTime}");
            }
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
