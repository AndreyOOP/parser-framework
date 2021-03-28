using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParserFramework;
using ParserFramework.DocumentSources;
using ParserFramework.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;

// should be simple but real test, in acceptance - more detailed cases; unit tests if required
namespace Features
{
    // Primitive data type parsing sample
    // Model properties annotated via XPathSource attribute whith xPath directing to element 
    // based on context tried to parse elemnt's inner text, or return complete html or HtmlNode
    [TestClass]
    public class GetPrimitiveValue
    {
        class SimpleModel
        {
            //[XPathSource("/div[@class='b']")]
            //public double Double { get; set; }

            //[XPathSource("/div[@id='i3']")]
            //public decimal Decimal { get; set; }

            //[XPathSource("/div[@id='i1']")]
            //public bool Bool { get; set; }

            //[XPathSource("/span[@id='i2']")]
            //public HtmlNode Node { get; set; }

            //[XPathSource("/a")]
            //public string Text { get; set; }

            [XPathSource("//a", returnHtml: true)]
            public string Html { get; set; }
        }

        [TestMethod]
        [DataRow(@"
        <html>
          <div class='a'>
            <div class='b' id='i3'>1.25</div>
          </div>
          <div class='b'>100</div>
          <div id='i1' class='c'>true</div>
          <span id='i2'><a href='http://test.com'>Test</a></span>
        </html>")]
        public void ParsePrimitiveDataTypes(string html)
        {
            var mapper = new XPathAttributeMapper(new PropertyInfoService());
            var model = mapper.Map<SimpleModel>(new HtmlNodeSource(html));

            //Assert.AreEqual(1.25, model.Double);
            //Assert.AreEqual(1.25m, model.Decimal);
            //Assert.AreEqual(true, model.Bool);
            //Assert.AreEqual("<a href='http://test.com'>Test</a>", model.Node.InnerHtml);
            //Assert.AreEqual("Test", model.Text);
            Assert.AreEqual("<a href='http://test.com'>Test</a>", model.Html);
        }
    }


    [TestClass]
    public class GetCollectionOfPrimitiveValues
    {
        [TestMethod]
        [DataRow(@"
        <html>
          <li>1</li>
          <li>2</li>
          <li>3</li>
          <ul><li>a</li></ul>
          <ul><li>b</li></ul>
          <ul><li>c</li></ul>
        </html>")]
        public void CollectionParse()
        {
            // ToDo:
        }

        class PrimitiveCollection
        {
            [XPathSource("/li")] // select list of elements, try to parse inner text
            public IEnumerable<int> Numbers { get; set; }

            [XPathSource("/ul", "//li")] // select list of elements, selector inside the list
            public IEnumerable<string> Strings { get; set; }

            [XPathSource("/ul")]
            public IEnumerable<HtmlNode> Nodes { get; set; }
        }
    }

    [TestClass]
    public class ParseInnerTypesTillPrimitiveValue
    {
        class Outer
        {
            [XPathSource("/div[@class='a']")] // select list of elements, try to parse inner text
            public Inner InnerModel { get; set; }

            [XPathSource("/span")]
            public string Text { get; set; }
        }

        class Inner // parsed as normal model but xPath related to Attribute in Outer model, works recursively
        {
            [XPathSource("//div[@id='i1']")]
            public double Double { get; set; }

            [XPathSource("//div[@id='i2']")]
            public bool Bool { get; set; }

            [XPathSource("//div[@id='i3']")]
            public HtmlNode Node { get; set; }
        }

        [TestMethod]
        [DataRow(@"
        <html>
          <div class='a'>
            <div id='i1'>1.25</div>
            <div id='i2'>true</div>
            <div id='i3'><span>txt</span></div>
          </div>
          <span>some text</span>
        </html>")]
        public void ComplexModel()
        {

        }
    }
    // then similar feature but for collections XPath("collection selector", "element selector")

    [TestClass]
    public class DocumentSourceFeatures
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
