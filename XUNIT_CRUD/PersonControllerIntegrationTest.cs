using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Entities;
using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;

// using FluentAssertions;
using FluentAssertions.Web;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace XUNIT_CRUD
{
    public class PersonControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        public PersonControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        private readonly HttpClient _client;

        [Fact]
        public async Task Index_ToReturnView()
        {
            HttpResponseMessage res = await _client.GetAsync("/People/index");
            // res.Should().Be2XXSuccessful();
            res.EnsureSuccessStatusCode();
            var content = await res.Content.ReadAsStringAsync();
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(content);
            var node = html.DocumentNode;
            var table =node.QuerySelectorAll(".zoz");
            Assert.NotNull(table);
            Assert.NotNull(content);

        }
    }
}