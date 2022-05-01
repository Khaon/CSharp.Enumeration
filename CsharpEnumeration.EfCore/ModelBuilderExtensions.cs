using CSharp.Enumeration;
using Csharp.Enumeration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CsharpEnumeration.EfCore;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Apply automatic conversion between enumeration id and its type.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder" /> on which is applied the automatic conversion.</param>
    public static void ApplyConversionToEnumeration(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.FindLeastDerivedEntityTypes(typeof(Entity));

        foreach(var mutableEntityType in entities)
        {
            foreach(var mutableProperty in mutableEntityType.GetProperties())
            {
                if (mutableProperty.ClrType.DerivesFromGenericType(typeof(Enumeration<,>)))
                {
                    // Enable converter for enumeration.
                    var idType = mutableProperty.ClrType.GetEnumerationIdType();

                    var converterType = typeof(EnumerationConverter<,>).MakeGenericType(mutableProperty.ClrType, idType!);
                    var converter = (ValueConverter)Activator.CreateInstance(converterType)!;
                    mutableProperty.SetValueConverter(converter);
                }
            }
        }
    }
    /// <summary>
    /// Applies to every entities derived from <see cref="Enumeration{TEnum, TId}"/> generic naming of their column.
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void ApplyEnumerationAutoColumnNaming(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.FindLeastDerivedEntityTypes(typeof(Entity));

        foreach(var mutableEntityType in entities)
        {
            foreach(var mutableProperty in mutableEntityType.GetProperties())
            {
                if (mutableProperty.ClrType.DerivesFromGenericType(typeof(Enumeration<,>)))
                {
                    // Make the enumeration column name equal to the Property name suffixed with `Id`.
                    mutableProperty.SetColumnName(mutableProperty.Name + "Id");
                }
            }
        }
    }
}
