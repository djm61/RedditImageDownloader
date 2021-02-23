using Microsoft.Extensions.Logging;

using RedditImageDownloader.ApiClient.WebRequest;

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RedditImageDownloader.ApiClient
{
    public class RedditImageDownloaderService : IRedditImageDownloaderService
    {
        private readonly ILogger<RedditImageDownloaderService> _logger;
        private readonly IHttpRequestFactory _httpRequestFactory;

        public RedditImageDownloaderService(ILoggerFactory loggerFactory, IHttpRequestFactory httpRequestFactory)
        {
            _logger = loggerFactory?.CreateLogger<RedditImageDownloaderService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _httpRequestFactory = httpRequestFactory ?? throw new ArgumentNullException(nameof(httpRequestFactory));

            _logger.LogDebug("RedditImageDownloaderService created");
        }

        public async Task<HttpResponseMessageTyped<List<Model.Entry>>> GetEntries(HttpRequestParameters parameters = null)
        {
            _logger.LogDebug($"GetEntries() | parameters[{parameters}]");

            var relativePath = "/home/entries";
            HttpResponseMessageTyped<List<Model.Entry>> apiResponse;
            try
            {
                var response = await _httpRequestFactory.GetAsync(relativePath, parameters);
                apiResponse = new HttpResponseMessageTyped<List<Model.Entry>>
                {
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                    StatusCode = response.StatusCode,
                    ResponseText = !response.IsSuccessStatusCode ? response.ContentAsString() : null,
                    Data = response.IsSuccessStatusCode ? response.ContentAsType<List<Model.Entry>>() : null
                };

                _logger.LogDebug($"GetEntries() | apiResponse StatusCode[{apiResponse.StatusCode}], ReasonPhrase[{apiResponse.ReasonPhrase}], RelativePath[{relativePath}]");
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetEntries() | error getting all content types [{ex}]", ex);

                apiResponse = new HttpResponseMessageTyped<List<Model.Entry>>
                {
                    IsSuccessStatusCode = false,
                    ReasonPhrase = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null
                };
            }

            return apiResponse;
        }
    }
}
