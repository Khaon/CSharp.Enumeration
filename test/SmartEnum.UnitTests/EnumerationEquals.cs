using CSharp.Enumeration;

namespace Smart.Common.Enumeration.UnitTests;

using FluentAssertions;
using Xunit;

public class EnumerationEquals
{
    private class TestEnum2 : Enumeration<TestEnum2, int>
    {
        public static TestEnum2 One = new TestEnum2(1, nameof(One));
        protected TestEnum2(int id, string name) : base(id, name)
        {
        }
    }

    public static TheoryData<TestEnum, object, bool> EqualsObjectData =>
        new TheoryData<TestEnum, object, bool>
        {
            { TestEnum.One, null, false },
            { TestEnum.One, TestEnum.One, true },
            { TestEnum.One, TestEnum2.One, false },
            { TestEnum.One, TestEnum.Two, false },
        };

    [Theory]
    [MemberData(nameof(EqualsObjectData))]
    public void EqualsObjectReturnsExpected(TestEnum left, object right, bool expected)
    {
        var result = left.Equals(right);

        result.Should().Be(expected);
    }

    public static TheoryData<TestEnum, TestEnum, bool> EqualsSmartEnumData =>
        new TheoryData<TestEnum, TestEnum, bool>
        {
            { TestEnum.One, null, false },
            { TestEnum.One, TestEnum.One, true },
            { TestEnum.One, TestEnum.Two, false },
        };

    [Theory]
    [MemberData(nameof(EqualsSmartEnumData))]
    public void EqualsSmartEnumReturnsExpected(TestEnum left, object right, bool expected)
    {
        var result = left.Equals(right);

        result.Should().Be(expected);
    }

    public static TheoryData<TestEnum, TestEnum, bool> EqualOperatorData =>
        new TheoryData<TestEnum, TestEnum, bool>
        {
            { null, null, true },
            { null, TestEnum.One, false },
            { TestEnum.One, null, false },
            { TestEnum.One, TestEnum.One, true },
            { TestEnum.One, TestEnum.Two, false },
        };

    [Theory]
    [MemberData(nameof(EqualOperatorData))]
    public void EqualOperatorReturnsExpected(TestEnum left, TestEnum right, bool expected)
    {
        var result = left == right;

        result.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(EqualOperatorData))]
    public void NotEqualOperatorReturnsExpected(TestEnum left, TestEnum right, bool expected)
    {
        var result = left != right;

        result.Should().Be(!expected);
    }
}