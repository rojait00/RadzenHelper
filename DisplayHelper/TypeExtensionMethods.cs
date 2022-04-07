using System.Reflection;

namespace RadzenHelper
{
    public static class TypeExtensionMethods
    {

        public static Type GetNestedType<T>(this IEnumerable<T> enumerable)
        {
            return typeof(T);
        }

        public static List<(PropertyInfo Info, T Attribute)> GetPropertyWithAttribute<T>(this Type type) where T : Attribute
        {
#pragma warning disable CS8619 // Checked here: .Where(x => x.Value != default)

            return type.GetProperties()
                       .Select(x => (Info: x, Attribute: GetAttribute<T>(x)))
                       .Where(x => x.Attribute != default)
                       .ToList();

#pragma warning restore CS8619 // Checked here: .Where(x => x.Value != default) 
        }

        private static T? GetAttribute<T>(PropertyInfo info) where T : Attribute
        {
            return (T?)info.GetCustomAttributes(typeof(T), true)
                           .FirstOrDefault();
        }
    }
}
