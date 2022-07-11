using Application.Common.HttpRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HttpRequests;

partial class Fetcher
{
    public async Task<string> PostAsync(object data, string path = "")
    {
        try
        {
            ReadyRequest(path);
            var response = await _client.PostAsJsonAsync(_link, data);
            response.EnsureSuccessStatusCode();
            var stringItem = await response.Content.ReadAsStringAsync();

            return stringItem;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message + "Type: " + e.GetType().Name);
            return string.Empty;
        }
    }

    public async Task<T?> PostAsync<T>(object data, string path = "") where T : IFetcheable
    {
        try
        {
            ReadyRequest(path);
            var response = await _client.PostAsJsonAsync(_link, data);
            response.EnsureSuccessStatusCode();
            var stringItem = await response.Content.ReadAsAsync<T>();

            return stringItem;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message + "Type: " + e.GetType().Name);
            return default;
        }
    }
}

