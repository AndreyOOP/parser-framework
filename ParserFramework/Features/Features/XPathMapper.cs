using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParserFramework;
using ParserFramework.Models;
using ParserFramework.Services;
using System.Collections.Generic;
using System.Linq;

// ToDo: try to organize code by feature + shared services

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
            [XPathSource("//span[@id='i4']")]
            public int Integer { get; set; }

            [XPathSource("//div[@id='i1']")]
            public bool Bool { get; set; }

            [XPathSource("//div[@id='i3']")]
            public decimal Decimal { get; set; }

            [XPathSource("//a")]
            public string Text { get; set; }

            [XPathSource("//a")]
            public Html Html { get; set; }

            [XPathSource("//span[@id='i2']")]
            public HtmlNode Node { get; set; }

            public int PopertyWithoutXPathSource { get; set; }
        }

        [TestMethod]
        [DataRow(@"
        <html>
          <div class='a'>
            <div class='b' id='i3'>1,25</div>
          </div>
          <div class='b'>100</div>
          <div id='i1' class='c'>true</div>
          <span id='i2'><a href='http://test.com'>Test</a></span>
          <span id='i4'>100</span>
        </html>")]
        public void ParsePrimitiveDataTypes(string html)
        {
            var mapper = new XPathAttributeMapper(new PropertyInfoService(), new ValueExtractorFactory(), new PropertyTypeDefinder());

            var model = mapper.Map<SimpleModel>(new HtmlNodeSource(html));

            Assert.AreEqual(100, model.Integer);
            Assert.AreEqual(true, model.Bool);
            Assert.AreEqual(1.25m, model.Decimal);
            Assert.AreEqual("Test", model.Text);
            Assert.AreEqual("<a href='http://test.com'>Test</a>", model.Html);
            Assert.AreEqual("<a href='http://test.com'>Test</a>", model.Node.InnerHtml);
        }
    }

    /// <summary>
    /// Collections parsing sample
    /// </summary>
    [TestClass]
    public class GetCollectionOfPrimitiveValues
    {
        class PrimitiveCollection
        {
            [XPathSource("(//li)[position() <= 3]")]
            public List<int> Integers { get; set; }

            [XPathSource("//div")]
            public List<bool> Bools { get; set; }

            [XPathSource("//ul[@class='d']")]
            public List<decimal> Decimals { get; set; }

            [XPathSource("//ul/li")]
            public List<string> Strings { get; set; }

            [XPathSource("//li")]
            public List<Html> Htmls { get; set; }

            [XPathSource("//ul[@class='d']")]
            public List<HtmlNode> Nodes { get; set; }

            [XPathSource("//ul/li")]
            public IEnumerable<string> StringsEnumerable { get; set; }

            public IEnumerable<int> NotAnnotatedIntegers { get; set; }

            public List<Html> NotAnnotatedHtmls { get; set; }
        }

        [TestMethod]
        [DataRow(@"
        <html>
          <li>1</li>
          <li>2</li>
          <li>3</li>
          <ul><li>a</li></ul>
          <ul><li>b</li></ul>
          <ul><li>c</li></ul>
          <div>true</div>
          <div>false</div>
          <ul class='d'>1,45</ul>
          <ul class='d'>3,64</ul>
        </html>")]
        public void CollectionParse(string html)
        {
            var mapper = new XPathAttributeMapper(new PropertyInfoService(), new ValueExtractorFactory(), new PropertyTypeDefinder());

            var model = mapper.Map<PrimitiveCollection>(new HtmlNodeSource(html));

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, model.Integers.ToArray());
            CollectionAssert.AreEqual(new List<bool>{ true, false }, model.Bools);
            CollectionAssert.AreEqual(new List<decimal> { 1.45m, 3.64m }, model.Decimals);
            CollectionAssert.AreEqual(new List<string> { "a", "b", "c" }, model.Strings);
            Assert.AreEqual(6, model.Htmls.Count);
            Assert.AreEqual("<li>1</li>", model.Htmls.First());
            Assert.AreEqual(2, model.Nodes.Count);
            Assert.AreEqual("1,45", model.Nodes.First().InnerHtml);
            CollectionAssert.AreEqual(new[] { "a", "b", "c"}, model.StringsEnumerable.ToArray());
        }
    }


    [Ignore("Not implemented")]
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
    
}
