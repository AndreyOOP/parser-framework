using HtmlAgilityPack;
using System;
using ParserFramework.Extensions;
using System.Linq;
using ParserFramework.Models;

namespace ParserFramework.Services
{
    public interface IValueExtractorFactory
    {
        /// <summary>
        /// Get extractor for HtmlNode 
        /// </summary>
        IValueExctractor Create(PropertyType propertyType, HtmlNodeCollection nodes);
    }

    public class ValueExtractorFactory : IValueExtractorFactory
    {
        public IValueExctractor Create(PropertyType propertyType, HtmlNodeCollection nodes)
        {
            var node = nodes.First();
            return propertyType switch
            {
                PropertyType.Int                => new IntegerExtractor(node),
                PropertyType.Bool               => new BoolExtractor(node),
                PropertyType.Decimal            => new DecimalExtractor(node),
                PropertyType.String             => new StringExtractor(node),
                PropertyType.Html               => new HtmlExtractor(node),
                PropertyType.HtmlNode           => new HtmlNodeExtractor(node),
                PropertyType.IntCollection      => new IntegerEnumerableExtractor(nodes),
                PropertyType.BoolCollection     => new BoolEnumerableExtractor(nodes),
                PropertyType.DecimalCollection  => new DecimalEnumerableExtractor(nodes),
                PropertyType.StringCollection   => new StringEnumerableExtractor(nodes),
                PropertyType.HtmlCollection     => new HtmlEnumerableExtractor(nodes),
                PropertyType.HtmlNodeCollection => new HtmlNodeEnumerableExtractor(nodes),
                _ => throw new NotImplementedException(Messages.ExtractorNotImplemented.Format(propertyType))
            };
        }
    }
}
