using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RedditImageDownloader.ApiClient.WebRequest
{
    public interface IHttpRequestFactory
    {
        #region Synchronous Calls

        HttpResponseMessage Get(string requestUri, HttpRequestParameters parameters = null, TimeSpan? timeout = null);
        HttpResponseMessage Post(string requestUri, object value = null, TimeSpan? timeout = null);
        HttpResponseMessage Put(string requestUri, object value = null, TimeSpan? timeout = null);
        HttpResponseMessage Delete(string requestUri, TimeSpan? timeout = null);

        #endregion

        #region Asynchronous Calls

        Task<HttpResponseMessage> GetAsync(string requestUri, HttpRequestParameters parameters = null, TimeSpan? timeout = null);
        Task<HttpResponseMessage> PostAsync(string requestUri, object value = null, TimeSpan? timeout = null);
        Task<HttpResponseMessage> PutAsync(string requestUri, object value = null, TimeSpan? timeout = null);
        Task<HttpResponseMessage> DeleteAsync(string requestUri, TimeSpan? timeout = null);

        #endregion
    }
}
