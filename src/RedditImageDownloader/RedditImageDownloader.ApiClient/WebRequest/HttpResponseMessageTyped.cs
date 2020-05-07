using System.Net;

namespace RedditImageDownloader.ApiClient.WebRequest
{
    public class HttpResponseMessageTyped<T>
    {
        public bool IsSuccessStatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        //public HttpContent Content { get; set; }
        public string ResponseText { get; set; }
    }
}
