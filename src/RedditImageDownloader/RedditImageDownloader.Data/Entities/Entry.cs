using System;

namespace RedditImageDownloader.Data.Entities
{
    public class Entry : EntityBase
    {
        public Entry()
        {
            Processed = EntityLiterals.No;
        }

        public long SourceId { get; set; }
        public string PostId { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string Link { get; set; }
        public string Processed { get; set; }
        public DateTime? Downloaded { get; set; }

        public virtual Source Source { get; set; }

        public override string ToString()
        {
            return
                $"PostId[{PostId}], SourceId[{SourceId}], Url[{Url}], Processed[{Processed}], Downloaded[{Downloaded:yyyy-MM-dd hh:mm:ss tt}], {base.ToString()}";
        }
    }
}
