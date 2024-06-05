using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.OData.Extensions;

namespace DOOH.Server.Data
{
    public static class EntityPatch
    {
        public static void Apply(object obj, object source)
        {
            foreach (PropertyInfo property in obj.GetType().GetProperties().Where(p => p.CanWrite && 
                !p.GetCustomAttributes(typeof(DatabaseGeneratedAttribute), false).Cast<DatabaseGeneratedAttribute>().Any()))
            {
                var value = property.GetValue(source, null);
                if (value != null)
                {
                    property.SetValue(obj, value, null);
                }
            }
        }


        public static Expression ToNullable(Expression expression)
        {
            if (!IsNullable(expression.Type))
            {
                return Expression.Convert(expression, ToNullable(expression.Type));
            }

            return expression;
        }

        public static bool IsNullable(Type clrType)
        {
            if (clrType.IsValueType)
            {
                return clrType.IsGenericType && clrType.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
            else
            {
                return true;
            }
        }

        public static Type ToNullable(Type clrType)
        {
            if (IsNullable(clrType))
            {
                return clrType;
            }
            else
            {
                return typeof(Nullable<>).MakeGenericType(clrType);
            }
        }
    }
}
