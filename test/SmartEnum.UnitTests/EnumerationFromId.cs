using CSharp.Enumeration;
using Smart.Common.Enumeration.UnitTests;

namespace Ardalis.SmartEnum.UnitTests;

using System;
using FluentAssertions;
using Xunit;

public class EnumerationFromId
{
    [Fact]
    public void ReturnsEnumGivenMatchingId()
    {
        var result = TestEnum.FromId(1);

        result.Should().BeSameAs(TestEnum.One);
    }

    [Fact]
    public void ReturnsEnumGivenDerivedClass()
    {
        var result = TestDerivedEnum.FromId(1);

        result.Should().NotBeNull().And.BeSameAs(TestDerivedEnum.One);
    }

    [Fact]
    public void ThrowsGivenNonMatchingId()
    {
        var id = -1;

        Action action = () => TestEnum.FromId(id);

        action.Should()
            .ThrowExactly<EnumerationNotFoundException>()
            .WithMessage($"No {typeof(TestEnum).Name} with Id {id} found.");
    }

    [Fact]
    public void ReturnsDefaultEnumGivenNonMatchingId()
    {
        var id = -1;
        var defaultEnum = TestEnum.One;

        var result = TestEnum.FromId(id, defaultEnum);

        result.Should().BeSameAs(defaultEnum);
    }
}