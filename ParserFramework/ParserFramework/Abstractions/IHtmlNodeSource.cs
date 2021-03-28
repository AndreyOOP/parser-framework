using HtmlAgilityPack;

namespace ParserFramework.Abstractions
{
    /// <summary>
    /// Type knows how to get <see cref="HtmlNode"> from different sources, e.g uri source, html (string) source etc
    /// </summary>
    public interface IHtmlNodeSource
    {
        public HtmlNode HtmlDocument { get; }
    }

}
