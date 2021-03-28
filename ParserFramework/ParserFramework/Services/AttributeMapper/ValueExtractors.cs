using HtmlAgilityPack;

namespace ParserFramework.Services
{
    public interface IValueExctractor
    {
        object Value { get; }
    }

    public class RawHtmlExctractor : IValueExctractor
    {
        HtmlNode node;

        public RawHtmlExctractor(HtmlNode node)
        {
            this.node = node;
        }

        public object Value => node.OuterHtml;
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

    public abstract class FromStringExtractorBase : IValueExctractor
    {
        protected StringExtractor stringExtractor;

        public FromStringExtractorBase(HtmlNode node)
        {
            stringExtractor = new StringExtractor(node);
        }

        public abstract object Value { get; }
    }

    public class BoolHtmlExtractor : FromStringExtractorBase
    {
        public BoolHtmlExtractor(HtmlNode node) : base(node) { }

        public override object Value => bool.Parse((string)stringExtractor.Value);
    }

    public class DecimalExtractor : FromStringExtractorBase
    {
        public DecimalExtractor(HtmlNode node) : base(node) { }

        public override object Value => decimal.Parse((string)stringExtractor.Value);
    }

    public class DoubleExtractor : FromStringExtractorBase
    {
        public DoubleExtractor(HtmlNode node) : base(node) { }

        public override object Value => double.Parse((string)stringExtractor.Value);
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
}
