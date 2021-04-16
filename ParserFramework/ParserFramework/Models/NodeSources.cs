using HtmlAgilityPack;
using System;
using System.Net.Http;

namespace ParserFramework.Models
{
    /// <summary>
    /// Type knows how to get <see cref="HtmlNode"> from different sources, e.g uri source, html (string) source etc
    /// </summary>
    public interface IHtmlNodeSource
    {
        public HtmlNode HtmlDocument { get; }
    }

    public class HtmlNodeSource : IHtmlNodeSource
    {
        private string html;
        private HtmlNode htmlNode;

        public HtmlNodeSource(string html)
        {
            this.html = html;
        }

        public HtmlNode HtmlDocument
        {
            get
            {
                if (htmlNode == null)
                {
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);
                    htmlNode = htmlDocument.DocumentNode;
                }
                // ToCheck: maybe return clone - otherwise it may have inpact on original node
                return htmlNode;
            }
        }
    }

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
                throw new HttpRequestException($"'{uri}' request returns {response.StatusCode} status code");
            }
        }
    }
}
