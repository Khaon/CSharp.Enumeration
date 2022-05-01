using System.Collections.Generic;
using Ardalis.SmartEnum;
using CSharp.Enumeration;

namespace Smart.Common.Enumeration.UnitTests;

using FluentAssertions;
using System;
using Xunit;

public class EnumStringFromId
{
    public static TheoryData<string, TestStringEnum> TestStringEnumData =>
        new TheoryData<string, TestStringEnum>
        {
            { nameof(TestStringEnum.One), TestStringEnum.One },
            { nameof(TestStringEnum.Two), TestStringEnum.Two },
        };

    [Theory]
    [MemberData(nameof(TestStringEnumData))]
    public void ReturnsEnumGivenMatchingId(string id, TestStringEnum expected)
    {
        var result = TestStringEnum.FromId(id);

        result.Should().BeSameAs(expected);
    }

    [Fact]
    public void ThrowsGivenNonMatchingId()
    {
        var id = string.Empty;

        Action action = () => TestStringEnum.FromId(id);

        action.Should()
            .ThrowExactly<EnumerationNotFoundException>()
            .WithMessage($"No {typeof(TestStringEnum).Name} with Id {id} found.");
    }

    [Fact]
    public void ReturnsDefaultEnumGivenNonMatchingId()
    {
        var id = string.Empty;
        var defaultEnum = TestStringEnum.One;

        var result = TestStringEnum.FromId(id, defaultEnum);

        result.Should().BeSameAs(defaultEnum);
    }

    [Fact]
    public void ReturnsDerivedEnumById()
    {
        var result = DerivedTestEnumWithIds1.FromId(1);

        Assert.Equal(DerivedTestEnumWithIds1.A, result);
    }
}