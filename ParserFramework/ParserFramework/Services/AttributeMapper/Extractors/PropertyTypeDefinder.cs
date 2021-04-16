using HtmlAgilityPack;
using ParserFramework.Models;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace ParserFramework.Services
{
    public interface IPropertyTypeDefinder
    {
        /// <summary>
        /// Find category of property - primitive (bool, decimal), collection of primitives, model for reccursive parsing, HtmlNode
        /// </summary>
        /// <returns><see cref="PropertyType"/></returns>
        PropertyType DefineType(PropertyInfo propertyInfo);
    }

    public class PropertyTypeDefinder : IPropertyTypeDefinder
    {
        public PropertyType DefineType(PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;

            // ToDo: find better IEnumerable check
            if (propertyType.GetInterfaces().FirstOrDefault(i => i.Name == nameof(IEnumerable)) != null && propertyType != typeof(string))
            {
                var genericType = propertyType.GetGenericArguments()?.First();

                if (genericType == typeof(int)) return PropertyType.IntCollection;
                if (genericType == typeof(bool)) return PropertyType.BoolCollection;
                if (genericType == typeof(decimal)) return PropertyType.DecimalCollection;
                if (genericType == typeof(string)) return PropertyType.StringCollection;
                if (genericType == typeof(Html)) return PropertyType.HtmlCollection;
                if (genericType == typeof(HtmlNode)) return PropertyType.HtmlNodeCollection;
                return PropertyType.ModelCollection;
            }

            if (propertyType == typeof(int)) return PropertyType.Int;
            if (propertyType == typeof(bool)) return PropertyType.Bool;
            if (propertyType == typeof(string)) return PropertyType.String;
            if (propertyType == typeof(decimal)) return PropertyType.Decimal;
            if (propertyType == typeof(Html)) return PropertyType.Html;
            if (propertyType == typeof(HtmlNode)) return PropertyType.HtmlNode;
            return PropertyType.Model;
        }
    }
}
