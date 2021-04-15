using HtmlAgilityPack;
using ParserFramework.Models;
using System.Linq;

namespace ParserFramework.Services
{
    public interface IValueExctractor
    {
        object Value { get; }
    }

    public abstract class FromStringExtractorBase : IValueExctractor
    {
        protected StringExtractor stringExtractor;

        public FromStringExtractorBase(HtmlNode node)
        {
            stringExtractor = new StringExtractor(node);
        }

        public abstract object Value { get; }
    }

    public class IntegerExtractor : FromStringExtractorBase
    {
        public IntegerExtractor(HtmlNode node) : base(node) 
        {
        }

        public override object Value => int.Parse((string)stringExtractor.Value);
    }

    public class BoolExtractor : FromStringExtractorBase
    {
        public BoolExtractor(HtmlNode node) : base(node) { }

        public override object Value => bool.Parse((string)stringExtractor.Value);
    }

    public class DecimalExtractor : FromStringExtractorBase
    {
        public DecimalExtractor(HtmlNode node) : base(node) { }

        public override object Value => decimal.Parse((string)stringExtractor.Value);
    }

    public class StringExtractor : IValueExctractor
    {
        HtmlNode node;

        public StringExtractor(HtmlNode node)
        {
            this.node = node;
        }

        public object Value => node.InnerText;
    }   

    public class HtmlExtractor : IValueExctractor
    {
        HtmlNode node;

        public HtmlExtractor(HtmlNode node)
        {
            this.node = node;
        }

        public object Value => new Html(node.OuterHtml);
    }

    public class HtmlNodeExtractor : IValueExctractor
    {
        HtmlNode node;

        public HtmlNodeExtractor(HtmlNode node)
        {
            this.node = node;
        }

        public object Value => node.Clone();
    }

    public class IntegerEnumerableExtractor : IValueExctractor
    {
        HtmlNodeCollection nodes;

        public IntegerEnumerableExtractor(HtmlNodeCollection nodes)
        {
            this.nodes = nodes;
        }

        public object Value => nodes.Select(n => (int)new IntegerExtractor(n).Value).ToList();
    }

    public class BoolEnumerableExtractor : IValueExctractor
    {
        HtmlNodeCollection nodes;

        public BoolEnumerableExtractor(HtmlNodeCollection nodes)
        {
            this.nodes = nodes;
        }

        public object Value => nodes.Select(n => (bool)new BoolExtractor(n).Value).ToList();
    }

    public class DecimalEnumerableExtractor : IValueExctractor
    {
        HtmlNodeCollection nodes;

        public DecimalEnumerableExtractor(HtmlNodeCollection nodes)
        {
            this.nodes = nodes;
        }

        public object Value => nodes.Select(n => (decimal)new DecimalExtractor(n).Value).ToList();
    }

    public class HtmlEnumerableExtractor : IValueExctractor
    {
        HtmlNodeCollection nodes;

        public HtmlEnumerableExtractor(HtmlNodeCollection nodes)
        {
            this.nodes = nodes;
        }

        public object Value => nodes.Select(n => (Html)new HtmlExtractor(n).Value).ToList();
    }

    public class StringEnumerableExtractor : IValueExctractor
    {
        HtmlNodeCollection nodes;

        public StringEnumerableExtractor(HtmlNodeCollection nodes)
        {
            this.nodes = nodes;
        }

        public object Value => nodes.Select(n => (string)new StringExtractor(n).Value).ToList();
    }

    public class HtmlNodeEnumerableExtractor : IValueExctractor
    {
        HtmlNodeCollection nodes;

        public HtmlNodeEnumerableExtractor(HtmlNodeCollection nodes)
        {
            this.nodes = nodes;
        }

        public object Value => nodes.Select(n => (HtmlNode)new HtmlNodeExtractor(n).Value).ToList();
    }

}
