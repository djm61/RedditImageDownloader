using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using RedditImageDownloader.ApiClient;
using RedditImageDownloader.Web.Models;

namespace RedditImageDownloader.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRedditImageDownloaderService _redditImageDownloaderService;

        public HomeController(ILoggerFactory loggerFactory, IRedditImageDownloaderService service)
        {
            _logger = loggerFactory.CreateLogger<HomeController>() ?? throw new ArgumentNullException(nameof(LoggerFactory));
            _redditImageDownloaderService = service ?? throw new ArgumentNullException(nameof(service));
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogDebug("Index");
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var entries = await _redditImageDownloaderService.GetEntries();
                _logger.LogDebug($"Index() | [{entries.Data}] entries");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Index() | Error getting entries [{ex}]", ex);
                return Ok();
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"Index took [{stopwatch.Elapsed}]");
            }
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
    }
}
