using System.Collections.Generic;
using System.Linq;

namespace RedditImageDownloader.Api.Extensions
{
    public static class EntityExtensions
    {
        public static Model.Entry ConvertToModel(this Data.Entities.Entry data)
        {
            var model = new Model.Entry
            {
                Id = data.Id,
                SourceId = data.SourceId,
                PostId = data.PostId,
                Url = data.Url,
                FileName = data.FileName,
                Processed = data.Processed,
                Downloaded = data.Downloaded,
                Active = data.Active,
                Deleted = data.Deleted
            };

            if (data.Source != null)
            {
                model.Source = data.Source.ConvertToModel();
            }

            return model;
        }

        public static IList<Model.Entry> ConvertToModel(this IEnumerable<Data.Entities.Entry> data)
        {
            var items = data.Select(e => e.ConvertToModel()).ToList();
            return items;
        }
    }
}
