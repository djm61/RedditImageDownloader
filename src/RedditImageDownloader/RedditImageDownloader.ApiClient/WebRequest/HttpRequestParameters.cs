using System.Collections.Specialized;

namespace RedditImageDownloader.ApiClient.WebRequest
{
    public class HttpRequestParameters
    {
        public HttpRequestParameters()
        {
            Headers = new NameValueCollection();
            //Headers = new HttpRequestMessage().Headers;
        }

        //Filtering
        public string Scope { get; set; }
        public string Role { get; set; }
        //Sorting
        public string Sort { get; set; }
        //Pagination
        public int Limit { get; set; }
        public int Offset { get; set; }

        public NameValueCollection Headers { get; set; }

        //public HttpRequestHeaders Headers { get; set; }
    }

}
