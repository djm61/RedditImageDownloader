namespace RedditImageDownloader.Model
{
    public class Source
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string NiceName { get; set; }
        public string Url { get; set; }
        public string FeedUrl { get; set; }

        public override string ToString()
        {
            return $"Id[{Id}], Name[{Name}], NiceName[{NiceName}], Url[{Url}], FeedUrl[{FeedUrl}], {base.ToString()}";
        }
    }
}
