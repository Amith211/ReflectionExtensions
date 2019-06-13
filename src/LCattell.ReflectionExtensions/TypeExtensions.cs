using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LCattell.ReflectionExtensions
{
    /// <summary>
    /// Extenion methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets all public declared methods.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="extraBindingFlags">The extra binding flags to apply to the search.
        /// <para/> See also <seealso cref="Type.GetMethods(BindingFlags)"/>.
        /// </param>
        /// <returns>Collection of methods.</returns>
        public static IEnumerable<MethodInfo> GetAllDeclaredMethods(this Type type, BindingFlags extraBindingFlags = BindingFlags.Default)
        {
            IEnumerable<MethodInfo> output =
                type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | extraBindingFlags)
                .ToList();

            return output;
        }

        /// <summary>
        /// Gets all declared methods from assembly.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="extraBindingFlags">The extra binding flags.</param>
        /// <returns>Collection of methods.</returns>
        public static IEnumerable<MethodInfo> GetAllDeclaredMethodsFromAssembly(this Type type,
                                                                                Assembly assembly = null,
                                                                                BindingFlags extraBindingFlags = BindingFlags.Default)
        {
            assembly = assembly ?? Assembly.GetCallingAssembly();

            IEnumerable<MethodInfo> output = GetAllDeclaredMethods(type, extraBindingFlags)
                .Where(x => x.Module.Assembly == assembly);

            return output;
        }
    }
}
