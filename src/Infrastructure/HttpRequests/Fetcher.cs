using Application.Common.HttpRequests;
using System.Net;
using System.Net.Http.Headers;

namespace Infrastructure.HttpRequests
{
    internal partial class Fetcher: IHttpFetcher
    {
        private readonly HttpClient _client = new();
        private string _link = "";
        
        public string BaseUrl { get; set; } = string.Empty;
        public Dictionary<string, string> QueryParams { get; } = new();
        public HttpRequestHeaders DefaultHeaders
        {
            get { return _client.DefaultRequestHeaders; }
        }

        public void AddQueryParam(string query, string value)
        {
            QueryParams.Add(query, value);
        }
        public void AddHeader(string header, string value)
        {
            DefaultHeaders.Add(header, value);
        }
        public string ShowLink(string path = "")
        {
            ReadyRequest(path);
            return _link;
        }

        public void RemoveQueryParam(string key)
        {
            QueryParams.Remove(key);
        }
    }
}
