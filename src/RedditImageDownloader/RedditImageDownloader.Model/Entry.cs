using System;

namespace RedditImageDownloader.Model
{
    public class Entry
    {
        public long Id { get; set; }
        public long SourceId { get; set; }
        public string PostId { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string Processed { get; set; }
        public DateTime? Downloaded { get; set; }
        public string Active { get; set; }
        public string Deleted { get; set; }

        public Source Source { get; set; }

        public override string ToString()
        {
            return $"Id[{Id}], SourceId[{SourceId}], PostId[{PostId}], Url[{Url}], FileName[{FileName}], Processed[{Processed}], Downloaded[{Downloaded}], Active[{Active}], Deleted[{Deleted}], Source[{Source}], {base.ToString()}";
        }
    }
}
