using System;
using System.Linq;
using System.Reflection;
using SQLite;

namespace OneSet.Extensions
{
    public static class EntityExtensions
    {
        public static object GetId(this object model)
        {
			var id = model.GetType().GetRuntimeProperty(IdentifierPropertyName(model)).GetValue(model, new object[0]);
			return id;
        }

        public static string IdentifierPropertyName(this object model)
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
			return attribute == null ? string.Empty : attribute.Name;
		}
    }
}