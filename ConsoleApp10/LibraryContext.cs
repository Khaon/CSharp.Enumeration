using System.Reflection;
using Csharp.Enumeration.Entities;
using Microsoft.EntityFrameworkCore;

namespace Csharp.Enumeration;

public class LibraryContext : DbContext
{
    public DbSet<Book> Books { get; set; } = null!;

    protected LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyConfigurationToEntities();
    }
}