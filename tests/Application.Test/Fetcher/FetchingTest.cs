using Application.Common.HttpRequests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.Test.Fetcher
{
    public class FetchingTest
    {
        private IHttpFetcher _fetcher;
        public FetchingTest(IHttpFetcher fetcher)
        {
            _fetcher = fetcher;
            _fetcher.BaseUrl = "http://www.boredapi.com/api/";
        }
        [Fact]
        public async Task GetTest()
        {
            _fetcher.ShowLink().Should().Be("http://www.boredapi.com/api/");
            var response = await _fetcher.GetAsync();

            response.Should().Be("{\"message\":\"Bored API\"}");
        }
    }
}
