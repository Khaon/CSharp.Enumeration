using Csharp.Enumeration.Entities;
using Csharp.Enumeration.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csharp.Enumeration.Configurations;

public class BookTypeConfiguration : BaseEntityTypeConfiguration<Book>
{
    public override void Configure(EntityTypeBuilder<Book> builder)
    {
        base.Configure(builder);
        //builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.Genre).HasConversion(e => e.Id, id => BookGenre.FromId(id));
    }
}

internal class EditorConfig : BaseEntityTypeConfiguration<Editor>
{
    public override void Configure(EntityTypeBuilder<Editor> builder)
    {
        base.Configure(builder);
        //builder.Property(e => e.Id).ValueGeneratedOnAdd();
    }
}

public class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.Id);
    }
}
