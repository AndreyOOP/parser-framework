using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParserFramework.Models;
using System;
using System.Net.Http;

namespace Features
{
    [TestClass]
    public class NodeSource
    {
        [TestMethod]
        [DataRow("<html></html>", 1)]
        [DataRow("<html><head></head><body></body></html>", 3)]
        public void HtmlNodeSource(string html, int expectedNodesQty)
        {
            var source = new HtmlNodeSource(html);

            Assert.AreEqual(expectedNodesQty, source.HtmlDocument.SelectNodes("//*").Count);
        }

        [TestMethod]
        [DataRow("https://www.autoklad.ua/")]
        public void UriNodeSource(string uriStr)
        {
            var nodeSource = new UriNodeSource(new Uri(uriStr), new HttpClient());

            Assert.IsTrue(nodeSource.HtmlDocument.SelectNodes("//*").Count > 0);
        }
    }
}
