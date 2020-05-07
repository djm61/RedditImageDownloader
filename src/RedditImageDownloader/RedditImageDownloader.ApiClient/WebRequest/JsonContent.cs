using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace RedditImageDownloader.ApiClient.WebRequest
{
    public class JsonContent : StringContent
    {
        public JsonContent(object value)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json")
        {
            StringValue = JsonConvert.SerializeObject(value);
        }

        public JsonContent(object value, string mediaType)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, mediaType)
        {
            StringValue = JsonConvert.SerializeObject(value);
        }

        public override string ToString()
        {
            return StringValue;
        }

        public string StringValue { get; private set; }
    }

}
