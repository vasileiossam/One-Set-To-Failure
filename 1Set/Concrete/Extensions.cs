using System;
using System.Linq;
using System.Reflection;
using SQLite;

namespace Set.Concrete
{
    public static class Extensions
    {
        public static object GetId(this object model)
        {
			var id = model.GetType().GetRuntimeProperty(model.IdentifierPropertyName()).GetValue(model, new object[0]);
			return id;
        }

        public static string IdentifierPropertyName(this Object model)
        {
            return IdentifierPropertyName(model.GetType());
        }

        public static string IdentifierPropertyName(this Type type)
        {
			if (type.GetRuntimeProperties().Any(info => info.PropertyType.AttributeExists<PrimaryKeyAttribute>()))
            {
                return
					type.GetRuntimeProperties().First(
						info => info.PropertyType.AttributeExists<PrimaryKeyAttribute>())
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

		public static string GetTableName(Type type)
		{
			var attribute = type.GetAttribute<TableAttribute> ();

			if (attribute == null) return string.Empty;
			return (attribute as TableAttribute).Name;
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