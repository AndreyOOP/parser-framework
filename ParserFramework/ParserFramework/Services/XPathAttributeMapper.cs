using HtmlAgilityPack;
using ParserFramework.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ParserFramework
{
    public class XPathAttributeMapper : IXPathAttributeMapper
    {
        readonly IPropertyInfoService propertyInfoService;

        public XPathAttributeMapper(IPropertyInfoService propertyInfoService)
        {
            this.propertyInfoService = propertyInfoService;
        }

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

    public class ValueExtractorFactory
    {
        public IValueExctractor Create(PropertyInfo propertyInfo, bool returnHtml, HtmlNode node)
        {
            if (returnHtml && propertyInfo.PropertyType == typeof(string))
                return new RawHtmlExctractor(node);

            return null;
        }
    }

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


    public interface IPropertyInfoService
    {
        /// Get custom attribute from <param name="propertyInfo" /> of type <typeparam name="T" />. Returns default if could not found the attribute 
        CustomAttributeData GetCustomAttributeData<T>(PropertyInfo propertyInfo);

        // Get first attributes constructor argument value of type T
        CustomAttributeTypedArgument GetAttributeConstructorArgument<T>(CustomAttributeData propertyInfo);

        TArgument GetAttributesConstructorArgumentValue<TAttribute, TArgument>(PropertyInfo propertyInfo);

        T GetAttribute<T>(PropertyInfo propertyInfo) where T : Attribute;
    }

    public class PropertyInfoService : IPropertyInfoService
    {
        public TArgument GetAttributesConstructorArgumentValue<TAttribute, TArgument>(PropertyInfo propertyInfo)
        {
            var attributeData = GetCustomAttributeData<TAttribute>(propertyInfo);
            var argumentData = GetAttributeConstructorArgument<TArgument>(attributeData);
            return (TArgument)argumentData.Value;
        }

        public CustomAttributeTypedArgument GetAttributeConstructorArgument<T>(CustomAttributeData data)
            => data.ConstructorArguments.FirstOrDefault(a => a.ArgumentType == typeof(T));


        public CustomAttributeData GetCustomAttributeData<T>(PropertyInfo propertyInfo)
            => propertyInfo.CustomAttributes.FirstOrDefault(attribute => attribute.AttributeType == typeof(T));

        public T GetAttribute<T>(PropertyInfo propertyInfo) where T : Attribute
            => propertyInfo.GetCustomAttributes().OfType<T>().First();
    }
}
