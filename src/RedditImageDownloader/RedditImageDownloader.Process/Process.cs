using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
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
                var source01 = new Source { Name = "ImaginaryTamriel", NiceName = "Imaginary Tamriel", Url = "https://www.reddit.com/r/ImaginaryTamriel/", FeedUrl = "https://www.reddit.com/r/ImaginaryTamriel/.rss" };
                var source02 = new Source { Name = "ImaginaryKnights", NiceName = "Imaginary Knights", Url = "https://www.reddit.com/r/ImaginaryKnights/", FeedUrl = "https://www.reddit.com/r/ImaginaryKnights/.rss" };
                var source03 = new Source { Name = "ImaginaryWarriors", NiceName = "Imaginary Warriors", Url = "https://www.reddit.com/r/ImaginaryWarriors/", FeedUrl = "https://www.reddit.com/r/ImaginaryWarriors/.rss" };
                var source04 = new Source { Name = "ImaginaryBattlefields", NiceName = "Imaginary Battlefields", Url = "https://www.reddit.com/r/ImaginaryBattlefields/", FeedUrl = "https://www.reddit.com/r/ImaginaryBattlefields/.rss" };
                var source05 = new Source { Name = "ImaginaryWizards", NiceName = "Imaginary Wizards", Url = "https://www.reddit.com/r/ImaginaryWizards/", FeedUrl = "https://www.reddit.com/r/ImaginaryWizards/.rss" };
                var source06 = new Source { Name = "ImaginaryNobles", NiceName = "Imaginary Nobles", Url = "https://www.reddit.com/r/ImaginaryNobles/", FeedUrl = "https://www.reddit.com/r/ImaginaryNobles/.rss" };
                var source07 = new Source { Name = "ImaginaryRuins", NiceName = "Imaginary Ruins", Url = "https://www.reddit.com/r/ImaginaryRuins/", FeedUrl = "https://www.reddit.com/r/ImaginaryRuins/.rss" };
                var source08 = new Source { Name = "ImaginaryHorrors", NiceName = "Imaginary Horrors", Url = "https://www.reddit.com/r/ImaginaryHorrors/", FeedUrl = "https://www.reddit.com/r/ImaginaryHorrors/.rss" };
                var source09 = new Source { Name = "ImaginaryVikings", NiceName = "Imaginary Vikings", Url = "https://www.reddit.com/r/ImaginaryVikings/", FeedUrl = "https://www.reddit.com/r/ImaginaryVikings/.rss" };
                var source10 = new Source { Name = "ImaginaryCosmere", NiceName = "Imaginary Cosmere", Url = "https://www.reddit.com/r/imaginarycosmere/", FeedUrl = "https://www.reddit.com/r/imaginarycosmere/.rss" };
                var source11 = new Source { Name = "ImaginaryBeasts", NiceName = "Imaginary Beasts", Url = "https://www.reddit.com/r/ImaginaryBeasts/", FeedUrl = "https://www.reddit.com/r/ImaginaryBeasts/.rss" };
                var source12 = new Source { Name = "ImaginaryTemples", NiceName = "Imaginary Temples", Url = "https://www.reddit.com/r/ImaginaryTemples/", FeedUrl = "https://www.reddit.com/r/ImaginaryTemples/.rss" };
                var source13 = new Source { Name = "ImaginaryTechnology", NiceName = "Imaginary Technology", Url = "https://www.reddit.com/r/ImaginaryTechnology/", FeedUrl = "https://www.reddit.com/r/ImaginaryTechnology/.rss" };
                var source14 = new Source { Name = "ImaginaryAdventurers", NiceName = "Imaginary Adventurers", Url = "https://www.reddit.com/r/ImaginaryAdventurers/", FeedUrl = "https://www.reddit.com/r/ImaginaryAdventurers/.rss" };
                var source15 = new Source { Name = "ImaginaryCastles", NiceName = "Imaginary Castles", Url = "https://www.reddit.com/r/ImaginaryCastles/", FeedUrl = "https://www.reddit.com/r/ImaginaryCastles/.rss" };
                var source16 = new Source { Name = "ImaginaryWorlds", NiceName = "Imaginary Worlds", Url = "https://www.reddit.com/r/ImaginaryWorlds/", FeedUrl = "https://www.reddit.com/r/ImaginaryWorlds/.rss" };
                var source17 = new Source { Name = "ImaginaryMonsterGirls", NiceName = "Imaginary Monster Girls", Url = "https://www.reddit.com/r/ImaginaryMonsterGirls/", FeedUrl = "https://www.reddit.com/r/ImaginaryMonsterGirls/.rss" };
                var source18 = new Source { Name = "Dragons", NiceName = "Dragons", Url = "https://www.reddit.com/r/Dragons/", FeedUrl = "https://www.reddit.com/r/Dragons/.rss" };
                var source19 = new Source { Name = "ImaginaryTaverns", NiceName = "Imaginary Taverns", Url = "https://www.reddit.com/r/ImaginaryTaverns/", FeedUrl = "https://www.reddit.com/r/ImaginaryTaverns/.rss" };
                var source20 = new Source { Name = "ImaginaryLeviathans", NiceName = "Imaginary Leviathans", Url = "https://www.reddit.com/r/ImaginaryLeviathans/", FeedUrl = "https://www.reddit.com/r/ImaginaryLeviathans/.rss" };
                var source21 = new Source { Name = "ImaginaryDerelicts", NiceName = "Imaginary Derelicts", Url = "https://www.reddit.com/r/ImaginaryDerelicts/", FeedUrl = "https://www.reddit.com/r/ImaginaryDerelicts/.rss" };
                var source22 = new Source { Name = "ImaginaryStarships", NiceName = "Imaginary Starships", Url = "https://www.reddit.com/r/ImaginaryStarships/", FeedUrl = "https://www.reddit.com/r/ImaginaryStarships/.rss" };
                var source23 = new Source { Name = "ReasonableFantasy", NiceName = "Reasonable Fantasy", Url = "https://www.reddit.com/r/ReasonableFantasy/", FeedUrl = "https://www.reddit.com/r/ReasonableFantasy/.rss" };
                var source24 = new Source { Name = "ImaginaryJedi", NiceName = "Imaginary Jedi", Url = "https://www.reddit.com/r/ImaginaryJedi/", FeedUrl = "https://www.reddit.com/r/ImaginaryJedi/.rss" };
                var source25 = new Source { Name = "ImaginaryFutureWar", NiceName = "Imaginary Future War", Url = "https://www.reddit.com/r/ImaginaryFutureWar/", FeedUrl = "https://www.reddit.com/r/ImaginaryFutureWar/.rss" };

                var existingSource = context.Sources.FirstOrDefault(s => s.Name == source01.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source01);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source02.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source02);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source03.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source03);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source04.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source04);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source05.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source05);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source06.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source06);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source07.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source07);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source08.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source08);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source09.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source09);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source10.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source10);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source11.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source11);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source12.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source12);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source13.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source13);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source14.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source14);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source15.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source15);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source16.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source16);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source17.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source17);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source18.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source18);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source19.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source19);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source20.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source20);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source21.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source21);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source22.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source22);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source23.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source23);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source24.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source24);
                }

                existingSource = context.Sources.FirstOrDefault(s => s.Name == source25.Name);
                if (existingSource == null)
                {
                    context.Sources.Add(source25);
                }

                context.SaveChanges();
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
                                entry = new Entry { SourceId = source.Id, PostId = item.Id, Url = imgSrc };
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
