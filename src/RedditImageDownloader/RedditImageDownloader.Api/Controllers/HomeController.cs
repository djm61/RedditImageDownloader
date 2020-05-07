using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedditImageDownloader.Api.Extensions;
using RedditImageDownloader.Data;

namespace RedditImageDownloader.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RedditImageDownloaderContext _context;

        public HomeController(ILoggerFactory loggerFactory, RedditImageDownloaderContext context)
        {
            _logger = loggerFactory?.CreateLogger<HomeController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _context = context ?? throw new ArgumentNullException(nameof(context));

            _logger.LogDebug("Home Controller created");
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

        [HttpGet("entries")]
        public async Task<IActionResult> GetListOfDownloadedImages()
        {
            _logger.LogDebug("GetListOfDownloadedImages");
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var dataItems = await _context.Entries
                    .Include(x => x.Source)
                    .Where(x => x.Active == EntityLiterals.Yes && x.Deleted == EntityLiterals.No)
                    .OrderBy(x => x.Source)
                    .ThenByDescending(x => x.Downloaded)
                    .ToListAsync();

                var items = dataItems.ConvertToModel();

                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetListOfDownloadedImages() | Error getting list of downloaded images [{ex}]", ex);
                return BadRequest(ex.Message);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"GetListOfDownloadedImages() took [{stopwatch.Elapsed}]");
            }
        }
    }
}