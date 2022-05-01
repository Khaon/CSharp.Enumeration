using CSharp.Enumeration;

namespace Smart.Common.Enumeration.UnitTests;

using System;
using FluentAssertions;
using Xunit;

public class EnumerationExtensionsTests
{
    public abstract class AbstractEnum : Enumeration<AbstractEnum, int>
    {
        protected AbstractEnum(int id, string name) : base(id, name) { }
    }

    public class GenericEnum<T> :
        Enumeration<GenericEnum<T>, T>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        protected GenericEnum(T id, string name) : base(id, name) { }
    }

    public static TheoryData<Type, bool, Type[]> IsSmartEnumData =>
        new TheoryData<Type, bool, Type[]>
        {
            { typeof(int), false, null },
            { typeof(AbstractEnum), false, null },
            { typeof(GenericEnum<>), false, null },
            { typeof(TestEnum), true, new Type[] { typeof(TestEnum), typeof(int) }},
        };

    [Theory]
    [MemberData(nameof(IsSmartEnumData))]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    public void IsSmartEnumReturnsExpected(Type type, bool expectedResult, Type[] _)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
    {
        var result = type.IsEnumeration();

        result.Should().Be(expectedResult);
    }

    [Theory]
    [MemberData(nameof(IsSmartEnumData))]
    public void IsSmartEnum2ReturnsExpected(Type type, bool expectedResult, Type[] expectedGenericArguments)
    {
        var result = type.IsEnumeration(out var genericArguments);

        result.Should().Be(expectedResult);
        if (result)
        {
            //FluentAssertions.Collections.
            genericArguments.Should().Equal(expectedGenericArguments);
        }
    }
}