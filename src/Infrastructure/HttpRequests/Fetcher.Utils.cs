using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HttpRequests;

partial class Fetcher
{
    private void ReadyRequest(string path)
    {
        _link = BuildUrl(path);
    }
    private string BuildUrl(string path)
    {
        bool check;
        string urlLink;

        if (path.Length != 0)
        {
            check = ContainsCharacterAtPos(BaseUrl, '/', BaseUrl.Length - 1);
            urlLink = check ? BaseUrl[0..^1] : BaseUrl;
            check = ContainsCharacterAtPos(path, '/', 0);
            urlLink += check ? path : $"/{path}";
        }
        else
        {
            urlLink = BaseUrl;
        }

        urlLink += GetQuery();

        return urlLink;
    }
    private string GetQuery()
    {
        string query = "";
        if (QueryParams.Count != 0)
        {
            query += "?";
            foreach (var param in QueryParams)
            {
                query += $"{param.Key}={param.Value}&";
            }
            query = query[0..^1];
        }

        return query;
    }
    private static bool ContainsCharacterAtPos(string text, char character, int pos)
    {
        return text.ElementAt(pos) == character;
    }
}
