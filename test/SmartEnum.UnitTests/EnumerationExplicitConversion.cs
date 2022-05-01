using Smart.Common.Enumeration.UnitTests;

namespace Smart.Common.Enumeration.UnitTestss;

using FluentAssertions;
using Xunit;

public class EnumerationExplicitConversion
{
    [Fact]
    public void ReturnsEnumFromGivenId()
    {
        int id = 1;

        var result = (TestEnum)id;

        result.Should().BeSameAs(TestEnum.One);
    }

    [Fact]
    public void ReturnsEnumFromGivenNullableId()
    {
        int? id = 1;

        var result = (TestEnum)id;

        result.Should().BeSameAs(TestEnum.One);
    }

    [Fact]
    public void ReturnsEnumFromGivenNullableIdAsNull()
    {
        int? id = null;

        var result = (TestEnum)id;

        result.Should().BeNull();
    }
}