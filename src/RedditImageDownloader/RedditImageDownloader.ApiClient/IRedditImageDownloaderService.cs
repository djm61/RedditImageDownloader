
using RedditImageDownloader.ApiClient.WebRequest;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditImageDownloader.ApiClient
{
    public interface IRedditImageDownloaderService
    {
        public Task<HttpResponseMessageTyped<List<Model.Entry>>> GetEntries(HttpRequestParameters parameters = null);
    }
}
