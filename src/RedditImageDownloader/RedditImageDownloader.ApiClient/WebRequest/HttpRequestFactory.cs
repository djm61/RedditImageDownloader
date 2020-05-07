using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RedditImageDownloader.ApiClient.WebRequest
{
    public class HttpRequestFactory : IHttpRequestFactory
    {
        private readonly HttpClientHandler _httpClientHandler;
        private readonly HttpRequestCriteria _httpRequestCriteria;

        public HttpRequestFactory(HttpRequestCriteria httpRequestCriteria)
        {
            _httpRequestCriteria = httpRequestCriteria ?? throw new ArgumentNullException(nameof(httpRequestCriteria));
            _httpClientHandler = new HttpClientHandler { Credentials = _httpRequestCriteria.Credentials };
            if (_httpRequestCriteria.DisableSslErrors)
            {
                _httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;
            }
        }

        #region Get

        public HttpResponseMessage Get(string requestUri, HttpRequestParameters parameters = null, TimeSpan? timeout = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_httpRequestCriteria.BaseUri + requestUri)
            };

            //Check for Filter Parameters
            if (parameters == null) return Send(request);
            request.AddQueryString(parameters);

            //Check for Accept-Language Header for Localization
            if (string.IsNullOrEmpty(parameters.Headers["Accept-Language"])) return Send(request);
            var acceptLanguage = parameters.Headers["Accept-Language"].Split(',').FirstOrDefault();
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(acceptLanguage));

            return Send(request, timeout);
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri, HttpRequestParameters parameters = null, TimeSpan? timeout = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_httpRequestCriteria.BaseUri + requestUri)
            };

            //Check for Filter Parameters
            if (parameters == null) return await SendAsync(request);
            request.AddQueryString(parameters);

            //Check for Accept-Language Header for Localization
            if (string.IsNullOrEmpty(parameters.Headers["Accept-Language"])) return await SendAsync(request);
            var acceptLanguage = parameters.Headers["Accept-Language"].Split(',').FirstOrDefault();
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(acceptLanguage));

            return await SendAsync(request, timeout);
        }

        #endregion

        #region Post

        public HttpResponseMessage Post(string requestUri, object value = null, TimeSpan? timeout = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_httpRequestCriteria.BaseUri + requestUri),
            };
            if (value != null) request.Content = new JsonContent(value);

            return Send(request, timeout);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, object value = null, TimeSpan? timeout = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_httpRequestCriteria.BaseUri + requestUri),
            };
            if (value != null) request.Content = new JsonContent(value);

            return await SendAsync(request, timeout);
        }

        #endregion

        #region Put

        public HttpResponseMessage Put(string requestUri, object value = null, TimeSpan? timeout = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(_httpRequestCriteria.BaseUri + requestUri)
            };
            if (value != null) request.Content = new JsonContent(value);

            return Send(request, timeout);
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri, object value = null, TimeSpan? timeout = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(_httpRequestCriteria.BaseUri + requestUri)
            };
            if (value != null) request.Content = new JsonContent(value);

            return await SendAsync(request, timeout);
        }

        #endregion

        #region Delete

        public HttpResponseMessage Delete(string requestUri, TimeSpan? timeout = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_httpRequestCriteria.BaseUri + requestUri)
            };

            return Send(request, timeout);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri, TimeSpan? timeout = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_httpRequestCriteria.BaseUri + requestUri)
            };

            return await SendAsync(request, timeout);
        }

        #endregion

        #region Private Members

        //This is supposed to be deadlock free according to https://stackoverflow.com/questions/53529061/whats-the-right-way-to-use-httpclient-synchronously
        private HttpResponseMessage Send(HttpRequestMessage request, TimeSpan? timeout = null)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(_httpRequestCriteria.BearerToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _httpRequestCriteria.BearerToken);

            //var task = TaskEx.Run(() => SendAsync(request, timeout));  //uses the BCL library?
            var task = Task.Run(() => SendAsync(request, timeout));
            task.Wait();
            var response = task.Result;

            return response;
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, TimeSpan? timeout = null)
        {
            if (timeout == null) timeout = TimeSpan.FromSeconds(30);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(_httpRequestCriteria.BearerToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _httpRequestCriteria.BearerToken);

            var httpClient = new HttpClient(_httpClientHandler) { Timeout = timeout.Value };
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);
            return response;
        }

        #endregion
    }

}
