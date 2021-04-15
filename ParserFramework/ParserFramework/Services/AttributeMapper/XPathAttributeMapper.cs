using HtmlAgilityPack;
using ParserFramework.Models;
using ParserFramework.Services;
using System.Linq;

namespace ParserFramework
{
    /// <summary>
    /// Map via annotations <see cref="XPathAttribute"> <see cref="IHtmlNodeSource"/> to class
    /// </summary>
    public interface IXPathAttributeMapper
    {
        public T Map<T>(IHtmlNodeSource source) where T : class, new();
    }

    public class XPathAttributeMapper : IXPathAttributeMapper
    {
        readonly IPropertyInfoService propertyInfoService;
        readonly IValueExtractorFactory extractorFactory;
        readonly IPropertyTypeDefinder typeDefinder;

        public XPathAttributeMapper(IPropertyInfoService propertyInfoService, IValueExtractorFactory extractorFactory, IPropertyTypeDefinder typeDefinder)
        {
            this.propertyInfoService = propertyInfoService;
            this.extractorFactory = extractorFactory;
            this.typeDefinder = typeDefinder;
        }

        // Note: move model mapping to separate task, now - put in order for Enumerable & single properties
        // ToDo: add exception handling strategy - e.g. if can not parse some property set default instead of exception | pass such behaviour as object
        public T Map<T>(IHtmlNodeSource source) where T : class, new()
        {
            var model = new T();

            var propertyToAttributeMap = model.GetType().GetProperties()
                .ToDictionary(k => k, v => propertyInfoService.GetAttribute<XPathSourceAttribute>(v))
                .Where(p => p.Value != null);

            foreach (var(property, attribute) in propertyToAttributeMap)
            {
                var nodes = source.HtmlDocument.SelectNodes(attribute.XPath);
                var propertyType = typeDefinder.DefineType(property);
                var extractor =  extractorFactory.Create(propertyType, nodes);

                property.SetValue(model, extractor.Value);
            }

            return model;
        }
    }

}
