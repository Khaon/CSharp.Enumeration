namespace Smart.Common.Enumeration.UnitTests;

using FluentAssertions;
using Xunit;

public class EnumerationAll
{
    [Fact]
    public void ReturnsAllDefinedSmartEnums()
    {
        var result = TestEnum.All;

        result.Should().BeEquivalentTo(new[] {
            TestEnum.One,
            TestEnum.Two,
            TestEnum.Three,
        });
    }

    [Fact]
    public void ReturnsAllBaseAndDerivedSmartEnums()
    {
        var result = TestBaseEnumWithDerivedIds.All;

        result.Should().BeEquivalentTo(new TestBaseEnumWithDerivedIds[] {
            DerivedTestEnumWithIds1.A,
            DerivedTestEnumWithIds1.B,
            DerivedTestEnumWithIds2.C,
            DerivedTestEnumWithIds2.D});
    }
}