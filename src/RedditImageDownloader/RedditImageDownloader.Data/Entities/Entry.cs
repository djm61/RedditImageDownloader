namespace RedditImageDownloader.Data.Entities
{
    public class Entry : EntityBase
    {
        public Entry()
        {
            Processed = EntityLiterals.No;
            Downloaded = EntityLiterals.No;
        }

        public long SourceId { get; set; }
        public string PostId { get; set; }
        public string Url { get; set; }
        public string Processed { get; set; }
        public string Downloaded { get; set; }

        public virtual Source Source { get; set; }

        public override string ToString()
        {
            return
                $"PostId[{PostId}], SourceId[{SourceId}], Url[{Url}], Processed[{Processed}], Downloaded[{Downloaded}], {base.ToString()}";
        }
    }
}
