using System.Runtime.CompilerServices;
using CSharp.Enumeration;

namespace Smart.Common.Enumeration.UnitTests;

public class TestEnum : Enumeration<TestEnum>
{
    public static readonly TestEnum One = new TestEnum(1);
    public static readonly TestEnum Two = new TestEnum(2);
    public static readonly TestEnum Three = new TestEnum(3);

    protected TestEnum(int id, [CallerMemberName] string name = null) : base(id, name)
    {
    }
}

public abstract class TestBaseEnum : Enumeration<TestBaseEnum>
{
    public static TestBaseEnum One;

    internal TestBaseEnum(int id, string name) : base(id, name)
    {
    }


}

public sealed class TestDerivedEnum : TestBaseEnum
{
    private TestDerivedEnum(int @int, string name) : base(@int, name)
    {
    }

    static TestDerivedEnum()
    {
        One = new TestDerivedEnum(1, nameof(One));
    }
}

public class TestStringEnum : Enumeration<TestStringEnum, string>
{
    public static readonly TestStringEnum One = new TestStringEnum(nameof(One), nameof(One));
    public static readonly TestStringEnum Two = new TestStringEnum(nameof(Two), nameof(Two));
    public static readonly TestStringEnum Three = new TestStringEnum(nameof(Three), nameof(Three));

    protected TestStringEnum(string @int, string name) : base(@int, name)
    {
    }
}

public class TestBaseEnumWithDerivedIds : Enumeration<TestBaseEnumWithDerivedIds>
{
    protected TestBaseEnumWithDerivedIds(int id, string name) : base(id, name)
    { }
}

public class DerivedTestEnumWithIds1 : TestBaseEnumWithDerivedIds
{
    public static readonly DerivedTestEnumWithIds1 A = new DerivedTestEnumWithIds1(1, nameof(A));
    public static readonly DerivedTestEnumWithIds1 B = new DerivedTestEnumWithIds1(1, nameof(B));

    private DerivedTestEnumWithIds1(int id, string name) : base(id, name) { }
}

public class DerivedTestEnumWithIds2 : TestBaseEnumWithDerivedIds
{
    public static readonly DerivedTestEnumWithIds2 C = new DerivedTestEnumWithIds2(1, nameof(C));
    public static readonly DerivedTestEnumWithIds2 D = new DerivedTestEnumWithIds2(1, nameof(D));

    private DerivedTestEnumWithIds2(int id, string name) : base(id, name) { }
}

