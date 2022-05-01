namespace Smart.Common.Enumeration.UnitTests;

using FluentAssertions;
using Xunit;

public class EnumerationToString
{
    public static TheoryData<TestEnum> NameData =>
        new TheoryData<TestEnum>
        {
            TestEnum.One,
            TestEnum.Two,
            TestEnum.Three,
        };

    [Theory]
    [MemberData(nameof(NameData))]
    public void ReturnsFormattedNameAndId(TestEnum smartEnum)
    {
        var result = smartEnum.ToString();

        result.Should().Be(smartEnum.Name);
    }
}