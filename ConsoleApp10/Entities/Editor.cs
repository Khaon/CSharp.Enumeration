namespace Csharp.Enumeration.Entities;

public class Editor : Entity
{
    public string Name { get; set; } = null!;

    public List<Book> EditedBooks { get; set; }
}
