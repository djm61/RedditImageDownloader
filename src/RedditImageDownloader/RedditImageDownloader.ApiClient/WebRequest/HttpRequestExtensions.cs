using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web;

namespace RedditImageDownloader.ApiClient.WebRequest
{
    public static class HttpRequestExtensions
    {
        public static HttpRequestMessage AddQueryString(this HttpRequestMessage request, HttpRequestParameters parameters)
        {
            var uriBuilder = new UriBuilder(request.RequestUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            if (!string.IsNullOrEmpty(parameters.Scope)) { query["scope"] = parameters.Scope; }
            if (!string.IsNullOrEmpty(parameters.Role)) { query["role"] = parameters.Role; }
            if (!string.IsNullOrEmpty(parameters.Sort)) { query["sort"] = parameters.Sort; }
            if (parameters.Limit != 0) { query["limit"] = parameters.Limit.ToString(); }
            if (parameters.Offset != 0) { query["offset"] = parameters.Offset.ToString(); }
            uriBuilder.Query = query.ToString();
            //request.RequestUri = uriBuilder.Uri;
            request.RequestUri = new Uri(uriBuilder.ToString());
            return request;
        }

        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value)) { return uri; }
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);
            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, HttpUtility.UrlEncode(value));
            var uriBuilder = new UriBuilder(uri) { Query = httpValueCollection.ToString() };
            return uriBuilder.Uri;
        }

        public static T ContentAsType<T>(this HttpResponseMessage response)
        {
            var data = (response != null && response.Content != null)
                ? response.Content.ReadAsStringAsync().Result
                : null;
            return string.IsNullOrEmpty(data) ? default(T) : JsonConvert.DeserializeObject<T>(data);
        }

        public static string ContentAsJson(this HttpResponseMessage response)
        {
            var data = (response != null && response.Content != null)
                ? response.Content.ReadAsStringAsync().Result
                : null;
            return string.IsNullOrEmpty(data) ? string.Empty : JsonConvert.SerializeObject(data);
        }

        public static string ContentAsString(this HttpResponseMessage response)
        {
            var data = (response != null && response.Content != null)
                ? response.Content.ReadAsStringAsync().Result
                : null;
            return string.IsNullOrEmpty(data) ? string.Empty : JsonConvert.DeserializeObject<string>(data);
        }

        //public static string GetClientIpAddress(this HttpRequest request)
        //{
        //    if (request == null || request.ServerVariables.Count == 0)
        //    {
        //        return string.Empty;
        //    }

        //    string ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    if (!string.IsNullOrWhiteSpace(ipAddress))
        //    {
        //        string[] addresses = ipAddress.Split(',');
        //        if (addresses.Length > 0)
        //        {
        //            return addresses[0];
        //        }
        //    }

        //    return request.ServerVariables["REMOTE_ADDR"];
        //}
    }
}
