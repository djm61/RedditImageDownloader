using System.Collections.Generic;
using System.Linq;

namespace RedditImageDownloader.Api.Extensions
{
    public static class SourceExtensions
    {
        public static Model.Source ConvertToModel(this Data.Entities.Source data)
        {
            var model = new Model.Source
            {
                Id = data.Id,
                Name = data.Name,
                NiceName = data.NiceName,
                Url = data.Url,
                FeedUrl = data.FeedUrl
            };

            return model;
        }

        public static IList<Model.Source> ConvertToModel(this IEnumerable<Data.Entities.Source> data)
        {
            var items = data.Select(s => s.ConvertToModel()).ToList();
            return items;
        }
    }
}
