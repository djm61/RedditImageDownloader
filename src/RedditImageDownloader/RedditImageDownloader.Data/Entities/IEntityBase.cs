using System;

namespace RedditImageDownloader.Data.Entities
{
    public interface IEntityBase
    {
        public long Id { get; set; }
        public string Active { get; set; }
        public string Deleted { get; set; }
        public DateTime RowVersion { get; set; }
    }
}