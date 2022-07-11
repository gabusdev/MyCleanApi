using System.Net;

namespace Application.Common.HttpRequests
{
    public interface IHttpFetcher
    {
        Task<T?> GetAsync<T>(string path = "") where T : IFetcheable;
        Task<string> GetAsync(string path = "");

        Task<T?> PostAsync<T>(object data, string path = "") where T : IFetcheable;
        Task<string> PostAsync(object data, string path = "");

        Task<T?> PutAsync<T>(object data, string path = "") where T : IFetcheable;
        Task<string> PutAsync(object data, string path = "");

        void AddQueryParam(string key, string value);
        void RemoveQueryParam(string key);
        void AddHeader(string header, string value);
        string ShowLink(string path = "");
    }
}
