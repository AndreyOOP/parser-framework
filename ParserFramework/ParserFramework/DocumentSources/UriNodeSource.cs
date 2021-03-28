using HtmlAgilityPack;
using ParserFramework.Abstractions;
using System;
using System.Net.Http;

namespace ParserFramework
{
    public class UriNodeSource : IHtmlNodeSource
    {
        private Uri uri;
        private HttpClient httpClient;
        private HtmlNode htmlNode;

        public UriNodeSource(Uri uri, HttpClient httpClient)
        {
            this.uri = uri;
            this.httpClient = httpClient;
        }

        public HtmlNode HtmlDocument
        {
            get
            {
                if (htmlNode != null)
                    return htmlNode;

                var response = httpClient.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var html = response.Content.ReadAsStringAsync().Result;
                    htmlNode = new HtmlNodeSource(html).HtmlDocument;
                    return htmlNode;
                }
                throw new HttpRequestException($"Request to {uri} return {response.StatusCode} status code");
            }
        }
    }
}
