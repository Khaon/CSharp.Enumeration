using CSharp.Enumeration;

namespace Csharp.Enumeration.Enumerations;

public class BookGenre : Enumeration<BookGenre>
{
    public static readonly BookGenre Anthology = new(1, nameof(Anthology));
    public static readonly BookGenre Biography = new(2, nameof(Biography));

    protected BookGenre(int id, string name) : base(id, name)
    {
    }
}
