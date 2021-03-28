using HtmlAgilityPack;
using System.Reflection;

namespace ParserFramework.Services
{
    public interface IValueExtractorFactory
    {
        IValueExctractor Create(PropertyInfo propertyInfo, bool returnHtml, HtmlNode node);
    }

    public class ValueExtractorFactory : IValueExtractorFactory
    {
        public IValueExctractor Create(PropertyInfo propertyInfo, bool returnHtml, HtmlNode node)
        {
            if (returnHtml && propertyInfo.PropertyType == typeof(string))
                return new RawHtmlExctractor(node);

            return null;
        }
    }
}
