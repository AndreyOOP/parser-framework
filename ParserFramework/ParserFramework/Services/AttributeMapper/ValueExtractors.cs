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
}
