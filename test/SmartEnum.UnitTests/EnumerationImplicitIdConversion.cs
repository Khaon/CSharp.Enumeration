namespace Smart.Common.Enumeration.UnitTests;

using FluentAssertions;
using Xunit;

public class EnumerationImplicitIdConversion
{
    [Fact]
    public void ReturnsIdOfGivenEnum()
    {
        var smartEnum = TestEnum.One;

        int result = smartEnum;

        result.Should().Be(smartEnum.Id);
    }
}