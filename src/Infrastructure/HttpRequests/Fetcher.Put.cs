using Application.Common.Exceptions.Custom_Exceptions;
using Application.Common.HttpRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HttpRequests;

partial class Fetcher
{
    public async Task<string> PutAsync(object data, string path = "")
    {
        try
        {
            ReadyRequest(path);
            var response = await _client.PutAsJsonAsync(_link, data);
            response.EnsureSuccessStatusCode();
            var stringItem = await response.Content.ReadAsStringAsync();

            return stringItem;
        }
        catch (HttpRequestException e)
        {
            Log.Error($"Failed Getting Resource: {ShowLink(path)} with error {e.Message}");
            throw new HttpFetchRequestException("Something Went Wrong");
        }
    }

    public async Task<T?> PutAsync<T>(object data, string path = "") where T : IFetcheable
    {
        try
        {
            ReadyRequest(path);
            var response = await _client.PutAsJsonAsync(_link, data);
            response.EnsureSuccessStatusCode();
            var stringItem = await response.Content.ReadAsAsync<T>();

            return stringItem;
        }
        catch (HttpRequestException e)
        {
            Log.Error($"Failed Getting Resource: {ShowLink(path)} with error {e.Message}");
            throw new HttpFetchRequestException("Something Went Wrong");
        }
    }
}

