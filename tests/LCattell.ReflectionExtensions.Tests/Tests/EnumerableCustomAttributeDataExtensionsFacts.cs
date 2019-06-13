using LCattell.ReflectionExtensions.Tests.Data;
using System.Reflection;
using Xunit;
using Xunit.Categories;

namespace LCattell.ReflectionExtensions.Tests
{
    [UnitTest(nameof(EnumerableCustomAttributeDataExtensionsFacts))]
    public class EnumerableCustomAttributeDataExtensionsFacts
    {
        [Fact]
        public void WhereAssembly_WithAssembly_ShouldReturnMemberMethodsFromAssembly()
        {

            // Arrange
            var methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name);
            var expected = MethodBase.GetCurrentMethod().GetCustomAttributesData();

            // Act
            var actual = methodInfo.GetCustomAttributesData().WhereAssembly(typeof(FactAttribute).Assembly);

            // Assert
            Assert.Contains(actual, x => x.AttributeType.Assembly.FullName.Contains($@"xunit.core"));
        }

        [Fact]
        [DummyAttribute]
        public void WhereAssembly_ShouldReturnMemberMethodsFromAssembly()
        {

            // Arrange
            var methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name);
            var expected = MethodBase.GetCurrentMethod().GetCustomAttributesData();

            // Act
            var actual = methodInfo.GetCustomAttributesData().WhereAssembly();

            // Assert
            Assert.Contains(actual, x => x.AttributeType.Assembly == Assembly.GetExecutingAssembly());
        }

        [Fact]
        [DummyAttribute]
        public void WhereNamespace_ShouldReturnMemberMethodsFromNamespace()
        {

            // Arrange
            var methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name);
            var expected = MethodBase.GetCurrentMethod().GetCustomAttributesData();

            // Act
            var actual = methodInfo.GetCustomAttributesData().WhereNamespace();

            // Assert
            Assert.Collection(actual, x => Assert.Contains(actual, y => y.AttributeType == typeof(DummyAttribute)));
        }

        [Fact]
        [DummyAttribute]
        public void WhereNamespace_WithNamespace_ShouldReturnMemberMethodsFromNamespace(/*MethodInfo methodInfo*/)
        {
            // Arrange
            var methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name);
            var expected = MethodBase.GetCurrentMethod().GetCustomAttributesData();

            // Act
            var actual = methodInfo.GetCustomAttributesData().WhereNamespace(
                $@"{nameof(LCattell)}.{nameof(ReflectionExtensions)}.{nameof(Tests)}.{nameof(Data)}");

            // Assert
            Assert.Collection(actual, x => Assert.Contains(actual, y => y.AttributeType == typeof(DummyAttribute)));
        }
    }
}
