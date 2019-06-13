using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LCattell.ReflectionExtensions
{
    /// <summary>
    /// Extenion methods for <see cref="IEnumerable{T}"/> (<see cref="CustomAttributeData"/>).
    /// </summary>
    public static class EnumerableCustomAttributeDataExtensions
    {
        /// <summary>
        /// Filters an enumerable based on an assembly. (Uses <see cref="Assembly.GetCallingAssembly"/> as default).
        /// </summary>
        /// <param name="attributeData">The attribute data.</param>
        /// <param name="assembly">The assembly to filter on.</param>
        /// <returns>Filtered <see cref="IEnumerable{T}"/><see cref="CustomAttributeData"/>.</returns>
        public static IEnumerable<CustomAttributeData> WhereAssembly(this IEnumerable<CustomAttributeData> attributeData, Assembly assembly = null)
        {
            assembly = assembly ?? Assembly.GetCallingAssembly();

            IList<CustomAttributeData> output = attributeData.Where(x => x.AttributeType.Assembly == assembly).ToList();

            return output;
        }

        /// <summary>
        /// Filters an enumerable based on a namespace/>
        /// (Uses shortest namespace path from Calling Assembly (See <see cref="Assembly.GetCallingAssembly"/>).
        /// </summary>
        /// <param name="attributeData">The attribute data.</param>
        /// <param name="namespace">The namespace to filter on.</param>
        /// <returns>Filtered enumerable containing namespce.</returns>
        public static IEnumerable<CustomAttributeData> WhereNamespace(this IEnumerable<CustomAttributeData> attributeData,
                                                                      string @namespace = null)
        {
            string GetNamespace()
            {
                IEnumerable<string> namespaces = Assembly.GetExecutingAssembly().GetTypes().ToList()
                    .GroupBy(x => x.Namespace, (y) => new { key = y.Namespace })
                    .Where(x => x.Key != null)
                    .OrderBy(x => x.Key.Length)
                    .Select(x => x.Key);

                return namespaces.FirstOrDefault();
            }

            @namespace = @namespace ?? GetNamespace();

            IList<CustomAttributeData> output = attributeData.Where(x => x.AttributeType.Namespace.Contains(@namespace)).ToList();

            return output;
        }
    }
}
