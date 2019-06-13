using LCattell.ReflectionExtensions.Tests.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Xunit;
using Xunit.Categories;

namespace LCattell.ReflectionExtensions.Tests.Tests
{
    [UnitTest(nameof(TypeExtensionsFacts))]
    [UnitTest("MethodInfoExtensions")]
    public class TypeExtensionsFacts
    {
        [ExcludeFromCodeCoverage]
        private class ThisFactsMethods
        {
            public void TestMethod()
            {
                throw new NotImplementedException();
            }

            [DummyAttribute]
            public void TestMethodWithAttribute()
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void GetAllDeclaredMethods_ShouldReturnCorrectMethods()
        {
            // Arrange
            Type type = typeof(ThisFactsMethods);

            IEnumerable<MethodInfo> expected = new List<MethodInfo>()
            {
                type.GetMethod(nameof(ThisFactsMethods.TestMethod)),
                type.GetMethod(nameof(ThisFactsMethods.TestMethodWithAttribute)),
            };

            // Act
            IEnumerable<MethodInfo> actual = type.GetAllDeclaredMethods();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllDeclaredMethodsFromAssembly_ShouldReturnCorrectMethods()
        {
            // Arrange
            Type type = typeof(ThisFactsMethods);

            IEnumerable<MethodInfo> expected = new List<MethodInfo>()
            {
                type.GetMethod(nameof(ThisFactsMethods.TestMethod)),
                type.GetMethod(nameof(ThisFactsMethods.TestMethodWithAttribute)),
            };

            // Act
            IEnumerable<MethodInfo> actual = type.GetAllDeclaredMethodsFromAssembly();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllDeclaredMethodsFromAssembly_WithAssembly_ShouldReturnCorrectMethods()
        {
            // Arrange
            Type type = typeof(object);

            IEnumerable<MethodInfo> expected = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var assembly = Assembly.GetAssembly(type);

            // Act
            IEnumerable<MethodInfo> actual = type.GetAllDeclaredMethodsFromAssembly(assembly);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
