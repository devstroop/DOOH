using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DOOH.Adboard.Extensions
{

    public static class ODataJsonSerializer
    {
        //
        // Summary:
        //     Determines whether the specified type is complex.
        //
        // Parameters:
        //   type:
        //     The type.
        //
        // Returns:
        //     true if the specified type is complex; otherwise, false.
        private static bool IsComplex(Type type)
        {
            Type type2 = ((type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(type) : type);
            Type type3 = (type2.IsGenericType ? type2.GetGenericArguments().FirstOrDefault() : type2);
            if (!type3.IsPrimitive && !typeof(IEnumerable<>).IsAssignableFrom(type3) && type != typeof(string) && type != typeof(decimal))
            {
                return type.IsClass;
            }

            return false;
        }

        //
        // Summary:
        //     Determines whether the specified type is enumerable.
        //
        // Parameters:
        //   type:
        //     The type.
        //
        // Returns:
        //     true if the specified type is enumerable; otherwise, false.
        private static bool IsEnumerable(Type type)
        {
            if (!typeof(string).IsAssignableFrom(type))
            {
                if (!typeof(IEnumerable<>).IsAssignableFrom(type))
                {
                    return typeof(IEnumerable).IsAssignableFrom(type);
                }

                return true;
            }

            return false;
        }

        //
        // Summary:
        //     Serializes the specified value.
        //
        // Parameters:
        //   value:
        //     The value.
        //
        //   options:
        //     The options.
        //
        // Type parameters:
        //   TValue:
        //     The type of the t value.
        //
        // Returns:
        //     System.String.
        public static string Serialize<TValue>(TValue value, JsonSerializerOptions options = null)
        {
            if (options == null)
            {
                options = new JsonSerializerOptions();
            }

            IEnumerable<PropertyInfo> source = from p in typeof(TValue).GetProperties()
                                               where IsComplex(p.PropertyType) || IsEnumerable(p.PropertyType)
                                               select p;
            IEnumerable<PropertyInfo> source2 = from p in typeof(TValue).GetProperties()
                                                where p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?)
                                                select p;
            if (source.Any() || source2.Any())
            {
                options.Converters.Add(new ComplexPropertiesConverter<TValue>(source.Select((PropertyInfo p) => p.Name)));
            }

            return JsonSerializer.Serialize(value, options);
        }
    }
}
