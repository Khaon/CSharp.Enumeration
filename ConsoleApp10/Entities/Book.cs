using Csharp.Enumeration.Enumerations;

namespace Csharp.Enumeration.Entities;

public class Book : Entity
{
    public string Title { get; set; } = null!;

    public int EditorId { get; set; }

    public Editor? Editor { get; set; }

    public int Pages { get; set; }

    public BookGenre Genre { get; set; } = null!;
}
