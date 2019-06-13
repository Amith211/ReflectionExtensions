using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LCattell.ReflectionExtensions
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>(<see cref="MethodInfo"/>).
    /// </summary>
    public static class EnumerableMethodInfoExtensions
    {
        /// <summary>
        /// Produces the set difference of two sequences excluding those which satifies a predicate.
        /// <para/> See also <see cref="Enumerable.Except{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource})" />.
        /// </summary>
        /// <param name="methods">The method info collection.</param>
        /// <param name="exceptPredicate">The except predicate. Elements satisfying the predicate are excluded from mthe result set.</param>
        /// <param name="equalityComparer">An <see cref="IEqualityComparer{MethodInfo}" /> to compare values.</param>
        /// <returns>
        /// An sequence that contains the set difference of the elements of two sequences.
        /// </returns>
        /// <exception cref="ArgumentNullException">exceptPredicate.</exception>
        public static IEnumerable<MethodInfo> Except(this IEnumerable<MethodInfo> methods,
                                                     Func<MethodInfo, bool> exceptPredicate,
                                                     IEqualityComparer<MethodInfo> equalityComparer = null)
        {
            IEnumerable<MethodInfo> except = new List<MethodInfo>();
            if (exceptPredicate != null)
            {
                except = methods.Where(exceptPredicate);
            }
            else
            {
                throw new ArgumentNullException(nameof(exceptPredicate));
            }

            IEnumerable<MethodInfo> output = methods.Except(except, equalityComparer);

            return output;
        }

        /// <summary>
        /// Filters an enumerable based on the presence of an <see cref="Attribute" />.
        /// </summary>
        /// <param name="methods">The methodinfo collection.</param>
        /// <param name="typeOfAttribute">The type of <see cref="Attribute" />.</param>
        /// <returns>A collection of methods that have the attribute.</returns>
        /// <exception cref="ArgumentNullException">typeOfAttribute.</exception>
        public static IEnumerable<MethodInfo> WhereCustomAttribute(this IEnumerable<MethodInfo> methods, Type typeOfAttribute)
        {
            if (typeOfAttribute == null)
            {
                throw new ArgumentNullException(nameof(typeOfAttribute));
            }

            IEnumerable<MethodInfo> output = methods.Where(
                x => x.GetCustomAttributes(typeOfAttribute, false).Length > 0);

            return output;
        }

        /// <summary>
        /// Filters an enumerable based on the presence of any <see cref="Attribute" />(s).
        /// </summary>
        /// <param name="methods">The methods.</param>
        /// <param name="attributeTypes">The attribute types.</param>
        /// <returns>A collection of methods that have any of the attributes.</returns>
        /// <exception cref="ArgumentNullException">attributeTypes.</exception>
        public static IEnumerable<MethodInfo> WhereAnyCustomAttribute(this IEnumerable<MethodInfo> methods, params Type[] attributeTypes)
        {
            if (attributeTypes == null)
            {
                throw new ArgumentNullException(nameof(attributeTypes));
            }

            IEnumerable<MethodInfo> output = methods.Where(
                x =>
                {
                    bool result = false;

                    foreach (Type type in attributeTypes)
                    {
                        result = x.GetCustomAttributes(type, false).Count() > 0;
                        break;
                    }

                    return result;
                });

            return output;
        }

        /// <summary>
        /// Produces the set difference of two sequences excluding the calling method.
        /// </summary>
        /// <param name="methods">The methods.</param>
        /// <param name="callerName">The name of the caller.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns> An sequence that contains the set difference of the elements of two sequences.</returns>
        public static IEnumerable<MethodInfo> ExceptCaller(this IEnumerable<MethodInfo> methods,
                                                           [CallerMemberName] string callerName = "",
                                                           IEqualityComparer<MethodInfo> equalityComparer = null)
            => methods.Except(x => x.Name == callerName, equalityComparer);
    }
}
