using System.Collections.Generic;

namespace RedditImageDownloader.Data.Entities
{
    public class Source : EntityBase
    {
        public Source()
        {
            Entries = new HashSet<Entry>();
        }

        public string Name { get; set; }
        public string NiceName { get; set; }
        public string Url { get; set; }
        public string FeedUrl { get; set; }

        public ICollection<Entry> Entries { get; set; }

        public override string ToString()
        {
            return $"Name[{Name}], Url[{Url}], {base.ToString()}";
        }
    }
}
