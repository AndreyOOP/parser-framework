using HtmlAgilityPack;
using ParserFramework.DocumentSources;
using ParserFramework.Services;
using System;

namespace ParserFramework
{
    /// <summary>
    /// Map via annotations <see cref="XPathAttribute"> <see cref="IHtmlNodeSource"/> to class
    /// </summary>
    public interface IXPathAttributeMapper
    {
        public T Map<T>(IHtmlNodeSource source) where T : class, new();

        public T Map<T>(HtmlNode node) where T : class;
    }

    public class XPathAttributeMapper : IXPathAttributeMapper
    {
        readonly IPropertyInfoService propertyInfoService;

        public XPathAttributeMapper(IPropertyInfoService propertyInfoService)
        {
            this.propertyInfoService = propertyInfoService;
        }

        // ToDo: add exception handling strategy - e.g. if can not parse some property set default instead of exception | pass such behaviour as object
        public T Map<T>(IHtmlNodeSource source) where T : class, new()
        {
            //var source = source.HtmlDocument;
            var factory = new ValueExtractorFactory();

            var model = new T();
            foreach (var property in model.GetType().GetProperties())
            {
                // get xpath & returnHtml
                var attribute = propertyInfoService.GetAttribute<XPathSourceAttribute>(property); // property.GetCustomAttributes().OfType<XPathSourceAttribute>().First();
                //var xPath = attribute.XPath;
                //var returnHtml = attribute.ReturnHtml;

                // select node
                var node = source.HtmlDocument.SelectSingleNode(attribute.XPath);

                // factory: create value getter
                var extractor = factory.Create(property, attribute.ReturnHtml, node);

                // get value
                var value = extractor.Value;

                // property - set value property.SetValue(tOut, convertedValue);
                property.SetValue(model, value);
                //factory
            }
            // define type - collection, primitive, another object

            return model;
        }

        public T Map<T>(HtmlNode node) where T : class
        {
            throw new NotImplementedException();
        }
    }

}
