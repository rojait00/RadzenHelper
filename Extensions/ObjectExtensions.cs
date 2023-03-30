using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace RadzenHelper.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Cannot be converted to json so don't care...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        //public static bool IsIDictionaryWithKeyOfType<T>(this object input, [NotNullWhen(true)] out IDictionary<T, dynamic>? values) where T : class
        //{
        //    Type? innerType = input.GetType()
        //                           .GetInterfaces()
        //                           .FirstOrDefault(x => x.IsGenericType
        //                                                && x.GetGenericTypeDefinition() == typeof(IDictionary<,>))?
        //                           .GetGenericArguments()[0];

        //    if (typeof(T).IsAssignableFrom(innerType))
        //    {
        //        values = ((IDictionary<T, dynamic>)input).ToDictionary(x => x.Key, x => x.Value);
        //        return true;
        //    }

        //    values = null;

        //    return false;
        //}

        public static bool IsCollectionOfType(this object input, Type type)
        {
            return input.GetType()
                        .GetInterfaces()
                        .Any(x => x.IsGenericType
                                  && x.GetGenericTypeDefinition() == type);
        }

        public static bool IsIDictionaryWithValueOfType<T>(this object input, [NotNullWhen(true)] out IDictionary<dynamic, dynamic>? values)
        {
            Type? innerType = input.GetType()
                                   .GetInterfaces()
                                   .FirstOrDefault(x => x.IsGenericType
                                                        && x.GetGenericTypeDefinition() == typeof(IDictionary<,>))?
                                   .GetGenericArguments()[1];

            if (innerType != null && typeof(T).IsAssignableFrom(innerType))
            {
                values = ((IDictionary)input).Keys.ToDynamicList().ToDictionary(x => x, x => ((IDictionary)input)[x]);
                return true;
            }

            values = null;
            return false;
        }

        public static bool IsIEnumerableOfType<T>(this object input, [NotNullWhen(true)] out IEnumerable<T>? values)
        {
            Type? innerType = input.GetType()
                                   .GetInterfaces()
                                   .FirstOrDefault(x => x.IsGenericType
                                                        && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))?
                                   .GetGenericArguments()[0];

            if (innerType != null && typeof(T).IsAssignableFrom(innerType))
            {
                values = ((IEnumerable<object>)input).OfType<T>();
                return true;
            }

            values = null;
            return false;
        }

        public static bool IsListOfType<T>(this object input, [NotNullWhen(true)] out IList<T>? values)
        {
            Type? innerType = input.GetType()
                                   .GetInterfaces()
                                   .FirstOrDefault(x => x.IsGenericType
                                                        && x.GetGenericTypeDefinition() == typeof(IList<>))?
                                   .GetGenericArguments()[0];

            if (innerType != null && typeof(T).IsAssignableFrom(innerType))
            {
                values = ((IList<object>)input).OfType<T>().ToList();
                return true;
            }

            values = null;
            return false;
        }

    }
}
