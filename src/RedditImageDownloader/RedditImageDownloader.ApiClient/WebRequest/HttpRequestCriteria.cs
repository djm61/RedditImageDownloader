using System;
using System.Net;

namespace RedditImageDownloader.ApiClient.WebRequest
{
    public class HttpRequestCriteria
    {
        public HttpRequestCriteria()
        {
            DisableSslErrors = false;
        }

        public Uri BaseUri { get; set; }
        public int TimeOutInSeconds { get; set; }
        public string BearerToken { get; set; }
        public WebProxy Proxy { get; set; }
        public NetworkCredential Credentials { get; set; }
        public bool DisableSslErrors { get; set; }

    }
}
