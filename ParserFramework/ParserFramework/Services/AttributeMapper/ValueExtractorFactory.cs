using HtmlAgilityPack;
using System;
using System.Reflection;
using ParserFramework.Extensions;

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
            var propertyType = propertyInfo.PropertyType;
            //var typeCode = Type.GetTypeCode(propertyType);
            
            return Type.GetTypeCode(propertyType) switch {
                TypeCode.String 
                    => returnHtml ? (IValueExctractor)new RawHtmlExctractor(node) : new StringExtractor(node),
                TypeCode.Object 
                    => propertyType == typeof(HtmlNode) ? 
                        new HtmlNodeExtractor(node) : 
                        throw new NotImplementedException(Messages.ExtractorNotImplemented.Format(propertyType.Name)),
                TypeCode.Boolean
                    => new BoolHtmlExtractor(node),
                TypeCode.Decimal
                    => new DecimalExtractor(node),
                TypeCode.Double
                    => new DoubleExtractor(node),
                _ 
                    => throw new NotImplementedException(Messages.ExtractorNotImplemented.Format(propertyType.Name))
            };
        }
    }
}
