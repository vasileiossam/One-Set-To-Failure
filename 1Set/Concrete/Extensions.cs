using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Set.Concrete
{
    public static class Extensions
    {
        public static object GetId(this object model)
        {
			return model.GetType().GetRuntimeProperty(model.IdentifierPropertyName()).GetValue(model, new object[0]);
        }

        public static string IdentifierPropertyName(this Object model)
        {
            return IdentifierPropertyName(model.GetType());
        }

        public static string IdentifierPropertyName(this Type type)
        {
			if (type.GetRuntimeProperties().Any(info => info.PropertyType.AttributeExists<SQLite.Net.Attributes.PrimaryKeyAttribute>()))
            {
                return
					type.GetRuntimeProperties().First(
						info => info.PropertyType.AttributeExists<SQLite.Net.Attributes.PrimaryKeyAttribute>())
                        .Name;
            }
			else if (type.GetRuntimeProperties().Any(p => p.Name.EndsWith("id", StringComparison.CurrentCultureIgnoreCase)))
            {
                return
					type.GetRuntimeProperties().First(
                        p => p.Name.EndsWith("id", StringComparison.CurrentCultureIgnoreCase)).Name;
            }
            return "";
        }
    }

    public static class PropertyInfoExtensions
    {
        public static bool AttributeExists<T>(this PropertyInfo propertyInfo) where T : class
        {
            var attribute = propertyInfo.GetCustomAttributes(typeof(T), false)
                                .FirstOrDefault() as T;
            if (attribute == null)
            {
                return false;
            }
            return true;
        }

        public static bool AttributeExists<T>(this Type type) where T : class
        {
			var attribute = type.GetTypeInfo().GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
            if (attribute == null)
            {
                return false;
            }
            return true;
        }

        public static T GetAttribute<T>(this Type type) where T : class
        {
			return type.GetTypeInfo().GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }

        public static T GetAttribute<T>(this PropertyInfo propertyInfo) where T : class
        {
            return propertyInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }
    }
}