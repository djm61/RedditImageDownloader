using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedditImageDownloader.Data;
using RedditImageDownloader.Data.Entities;


namespace RedditImageDownloader.Process
{
    public class Process
    {
        private const string BaseImageFolder = @"d:\Images";
        private const string RedditImageDownloaderFolder = @"d:\Images\RedditImageDownloader";

        private static readonly HttpClient Client = new HttpClient();

        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<Process> _logger;
        private readonly ConnectionStrings _connectionStrings;

        public Process(ILoggerFactory loggerFactory, IOptions<ConnectionStrings> connectionStrings)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger<Process>();
            //_context = context ?? throw new ArgumentNullException(nameof(context));
            _connectionStrings = connectionStrings.Value ?? throw new ArgumentNullException(nameof(connectionStrings));
        }

        public void ParseSources()
        {
            _logger.LogDebug("ParseSources()");

            var options = RedditImageDownloaderContext.BuildContextOptions(_loggerFactory, _connectionStrings.Default);
#if DEBUG
            using (var context = new RedditImageDownloaderContext(_loggerFactory, options))
            {
                var existingSource = context.Sources
                    .FirstOrDefault(s => s.Active == EntityLiterals.Yes && s.Deleted == EntityLiterals.No);

                if (existingSource == null)
                {
                    var source1 = new Source { Name = "ImaginaryTamriel", NiceName = "Imaginary Tamriel", Url = "https://www.reddit.com/r/ImaginaryTamriel/", FeedUrl = "https://www.reddit.com/r/ImaginaryTamriel/.rss" };
                    var source2 = new Source { Name = "ImaginaryKnights", NiceName = "Imaginary Knights", Url = "https://www.reddit.com/r/ImaginaryKnights/", FeedUrl = "https://www.reddit.com/r/ImaginaryKnights/.rss" };
                    var source3 = new Source { Name = "ImaginaryWarriors", NiceName = "Imaginary Warriors", Url = "https://www.reddit.com/r/ImaginaryWarriors/", FeedUrl = "https://www.reddit.com/r/ImaginaryWarriors/.rss" };
                    var source4 = new Source { Name = "ImaginaryBattlefields", NiceName = "Imaginary Battlefields", Url = "https://www.reddit.com/r/ImaginaryBattlefields/", FeedUrl = "https://www.reddit.com/r/ImaginaryBattlefields/.rss" };
                    var source5 = new Source { Name = "ImaginaryWizards", NiceName = "Imaginary Wizards", Url = "https://www.reddit.com/r/ImaginaryWizards/", FeedUrl = "https://www.reddit.com/r/ImaginaryWizards/.rss" };
                    var source6 = new Source { Name = "ImaginaryNobles", NiceName = "Imaginary Nobles", Url = "https://www.reddit.com/r/ImaginaryNobles/", FeedUrl = "https://www.reddit.com/r/ImaginaryNobles/.rss" };
                    context.Sources.Add(source1);
                    context.Sources.Add(source2);
                    context.Sources.Add(source3);
                    context.Sources.Add(source4);
                    context.Sources.Add(source5);
                    context.Sources.Add(source6);
                    context.SaveChanges();
                }
            }
#endif

            using (var context = new RedditImageDownloaderContext(_loggerFactory, options))
            {
                var sources = context.Sources
                    .Where(s => s.Active == EntityLiterals.Yes && s.Deleted == EntityLiterals.No)
                    .ToList();

                _logger.LogDebug($"ParseSources() | Processing [{sources.Count}] sources");
                Parallel.ForEach(sources, GetFromSource);
            }
        }

        public void DownloadImages()
        {
            _logger.LogDebug("DownloadImages()");

            CheckDirectories();

            var options = RedditImageDownloaderContext.BuildContextOptions(_loggerFactory, _connectionStrings.Default);
            using (var context = new RedditImageDownloaderContext(_loggerFactory, options))
            {
                var entries = context.Entries
                    .Include(e => e.Source)
                    .Where(e => e.Downloaded == null && e.Processed == EntityLiterals.No)
                    .ToList();

                Parallel.ForEach(entries, DownloadImage);
                context.SaveChanges();
            }
        }

        public void AddSource(string name, string niceName, string url, string feed)
        {
            var options = RedditImageDownloaderContext.BuildContextOptions(_loggerFactory, _connectionStrings.Default);
            using (var context = new RedditImageDownloaderContext(_loggerFactory, options))
            {
                var source = new Source
                {
                    Name = name,
                    NiceName = niceName,
                    Url = url,
                    FeedUrl = feed
                };
                context.Sources.Add(source);
                context.SaveChanges();
            }
        }

        private void GetFromSource(Source source)
        {
            _logger.LogDebug($"GetFromSource() | source[{source}]");

            var result = Client.GetStreamAsync(source.FeedUrl).Result;
            _logger.LogDebug("GetFromSource() | Got data from feed url");

            using (var xmlReader = XmlReader.Create(result))
            {
                _logger.LogDebug("GetFromSource() | Created XML Reader, loading into syndication feed");
                var feed = SyndicationFeed.Load(xmlReader);
                if (feed != null)
                {
                    _logger.LogDebug($"GetFromSource() | Created syndication feed, processing [{feed.Items.Count()}] items");
                    var options =
                        RedditImageDownloaderContext.BuildContextOptions(_loggerFactory, _connectionStrings.Default);
                    using (var context = new RedditImageDownloaderContext(_loggerFactory, options))
                    {
                        foreach (var item in feed.Items)
                        {
                            var entry = context.Entries.FirstOrDefault(x => x.PostId == item.Id && x.SourceId == source.Id);
                            if (entry != null)
                            {
                                _logger.LogWarning($"GetFromSource() | Item with Id[{item.Id}] exists, skipping");
                                continue;
                            }

                            if (item.Content is TextSyndicationContent text)
                            {
                                var imgSrc = ParseContent(text.Text);
                                if (string.IsNullOrWhiteSpace(imgSrc))
                                {
                                    _logger.LogWarning("GetFromSource() | no img src, skipping");
                                    continue;
                                }

                                _logger.LogDebug("GetFromSource() | creating entry and adding");
                                entry = new Entry {SourceId = source.Id, PostId = item.Id, Url = imgSrc};
                                context.Entries.Add(entry);
                                context.SaveChanges();
                            }
                            else
                            {
                                _logger.LogWarning(
                                    $"GetFromSource() | item.Content was not a TextSyndicationContent, it is [{item.Content.GetType()}]");
                            }
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("GetFromSource() | Error loading content into syndication feed");
                }
            }
        }

        private void DownloadImage(Entry entry)
        {
            var path = BuildFolder(entry);
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(new Uri(entry.Url), path);
                }

                var filename = Path.GetFileName(path);
                entry.FileName = filename;
                entry.Processed = EntityLiterals.Yes;
                entry.Downloaded = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Not Found"))
                {
                    //do nothing
                    entry.Processed = EntityLiterals.Yes;
                }
                else
                {
                    _logger.LogError($"DownloadImage() | Error downloading image with entity [{entry}], [{ex}]", ex);
                }
            }
        }

        private static string BuildFolder(Entry entry)
        {
            if (entry.Source == null)
            {
                throw new ArgumentNullException(nameof(entry.Source));
            }

            var path = Path.Combine(RedditImageDownloaderFolder, entry.Source.Name);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filename = entry.Url.Split('/').LastOrDefault();
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new NullReferenceException($"Filename not found in url");
            }

            if (filename.Contains("?"))
            {
                filename = filename.Split('?').FirstOrDefault();
            }

            path = Path.Combine(path, filename);
            var count = 1;
            var filenameOnly = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);
            var directory = Path.GetDirectoryName(path);
            var newPath = path;
            while (File.Exists(newPath))
            {
                var tempFilename = $"{filenameOnly} ({count++})";
                newPath = Path.Combine(directory, tempFilename + extension);
            }

            return newPath;
        }

        private static string ParseContent(string content)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);

            //thumbnail
            //<img src="https://b.thumbs.redditmedia.com/iak5HfbfpMe3Eutm7exnssL919nXB4fM-7af5PVqtpg.jpg" alt="Ashlander Zealot by Dima Krainuk" title="Ashlander Zealot by Dima Krainuk">
            //image
            //<a href="https://i.redd.it/z379hz7uws741.png">[link]</a>
            //<a href="https://cdnb.artstation.com/p/assets/images/images/022/757/205/large/dima-krainuk-ashlander-zealot-3.jpg?1576589004">[link]</a>

            var img = document.DocumentNode.SelectSingleNode("(//table//tr[1]//td//div[contains(@class, 'md')]//p//a)");
            if (img == null)
            {
                img = document.DocumentNode.SelectSingleNode("(//table//tr[1]//td//span//a)");
            }

            if (img != null)
            {
                var src = img.GetAttributeValue("href", string.Empty);
                return src;
            }

            return string.Empty;
        }

        private static void CheckDirectories()
        {
            if (!Directory.Exists(BaseImageFolder))
            {
                Directory.CreateDirectory(BaseImageFolder);
            }

            if (!Directory.Exists(RedditImageDownloaderFolder))
            {
                Directory.CreateDirectory(RedditImageDownloaderFolder);
            }
        }
    }
}
