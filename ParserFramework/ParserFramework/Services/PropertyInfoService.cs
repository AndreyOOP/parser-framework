using System;
using System.Linq;
using System.Reflection;

namespace ParserFramework.Services
{
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
