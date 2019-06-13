using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Categories;
using Xunit.Sdk;

namespace LCattell.ReflectionExtensions.Tests.Tests
{
    [UnitTest(nameof(EnumerableMethodInfoExtensionsFacts))]
    [UnitTest("MethodInfoExtensions")]
    public class EnumerableMethodInfoExtensionsFacts
    {
        [ExcludeFromCodeCoverage]
        private class ThisFactsMethods
        {
            [IntegrationTest]
            public void MethodWithAttribute()

            {
                throw new NotImplementedException();
            }

            [Obsolete("Test")]
            public void MethodWithoutAttribute()
            {
                throw new NotImplementedException();
            }
        }

        public static Type DefaultType = typeof(ThisFactsMethods);

        public static IEnumerable<MethodInfo> AllTestMethods => DefaultType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);


        [Fact]
        public void WhereCustomAttribute_ShouldReturnCorrectMethods()
        {
            // Arrange
            var expected = new List<MethodInfo>()
            {
                DefaultType.GetMethod(nameof(ThisFactsMethods.MethodWithAttribute)),
            };

            // Act
            var actual = AllTestMethods.WhereCustomAttribute(typeof(ITraitAttribute));

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WhereCustomAttributes_ShouldReturnCorrectMethods()
        {
            Type type = typeof(ThisFactsMethods);

            // Arrange
            var expected = new List<MethodInfo>()
            {
                type.GetMethod(nameof(ThisFactsMethods.MethodWithAttribute)),
            };

            // Act
            var actual = AllTestMethods.WhereAnyCustomAttribute(typeof(ITraitAttribute), typeof(ObsoleteAttribute));

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExceptSelf_ShouldReturnCorrectMethods()
        {
            // Arrange
            MethodInfo thisMethod = GetType().GetMethod(MethodBase.GetCurrentMethod().Name);
            var mockGetMethods = new List<MethodInfo>() { thisMethod };

            var expected = new List<MethodInfo>() { };

            // Act
            var actual = mockGetMethods.ExceptCaller();

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}
