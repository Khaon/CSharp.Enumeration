// ------------------------------------------------
// Ported from https://github.com/ardalis/SmartEnum
// -------------------------------------------------

using System.Reflection;
using System.Runtime.CompilerServices;

namespace CSharp.Enumeration;

public abstract class Enumeration<TEnum> : Enumeration<TEnum, int>
    where TEnum : Enumeration<TEnum, int>
{
    protected Enumeration(int id, string name) : base(id, name)
    {
    }
}

/// <summary>
/// A base type to use for creating smart enums.
/// </summary>
/// <typeparam name="TEnum">The type that is inheriting from this class.</typeparam>
/// <typeparam name="TId">The type of the inner value.</typeparam>
/// <remarks></remarks>
public abstract class Enumeration<TEnum, TId> :
    IEnumeration,
    IEquatable<Enumeration<TEnum, TId>>,
    IComparable<Enumeration<TEnum, TId>>
    where TEnum : Enumeration<TEnum, TId>
    where TId : IEquatable<TId>, IComparable<TId>
{
    private static readonly Lazy<TEnum[]> LazyLoadedEnumerations = new Lazy<TEnum[]>(GetAllEnumerations);

    private static readonly Lazy<Dictionary<string, TEnum>> LazyLoadedNames =
        new Lazy<Dictionary<string, TEnum>>(() => LazyLoadedEnumerations.Value.ToDictionary(item => item.Name));

    private static readonly Lazy<Dictionary<string, TEnum>> LazyLoadedNamesIgnoreCase =
        new Lazy<Dictionary<string, TEnum>>(() => LazyLoadedEnumerations.Value.ToDictionary(item => item.Name, StringComparer.OrdinalIgnoreCase));

    private static readonly Lazy<Dictionary<TId, TEnum>> LazyLoadedIds =
        new Lazy<Dictionary<TId, TEnum>>(() =>
        {
            // multiple enums with same id are allowed but store only one per id
            var dictionary = new Dictionary<TId, TEnum>();
            foreach (var item in LazyLoadedEnumerations.Value)
            {
                if (!dictionary.ContainsKey(item.Id))
                    dictionary.Add(item.Id, item);
            }
            return dictionary;
        });

    private static TEnum[] GetAllEnumerations()
    {
        // We make sure to introspect partial classes too.
        var enumerationType = typeof(TEnum);
        return Assembly.GetAssembly(enumerationType)!
            .GetTypes()
            .Where(assemblyType => enumerationType.IsAssignableFrom(assemblyType))
            .SelectMany(enumTypeInAssembly => enumTypeInAssembly.GetFieldsOfType<TEnum>())
            .OrderBy(@enum => @enum.Name)
            .ToArray();
    }

    /// <summary>
    /// Gets the id.
    /// </summary>
    public TId Id { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>A <see cref="String"/> that is the name of the <see cref="Enumeration{TEnum}"/>.</value>
    public string Name { get; }

    /// <summary>
    /// Gets a collection containing all the instances of <see cref="Enumeration{TEnum}"/>.
    /// </summary>
    /// <value>A <see cref="IReadOnlyCollection{TEnum}"/> containing all the instances of <see cref="Enumeration{TEnum}"/>.</value>
    /// <remarks>Retrieves all the instances of <see cref="Enumeration{TEnum}"/> referenced by public static read-only fields in the current class or its bases.</remarks>
    public static IReadOnlyCollection<TEnum> All =>
        LazyLoadedNames.Value.Values
            .ToList()
            .AsReadOnly();

    protected Enumeration(TId id, string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
        Id = id;
    }

    /// <summary>
    /// Gets an item associated with the specified id.
    /// </summary>
    /// <param name="id">The id of the item to get.</param>
    /// <returns>
    /// The first item found that is associated with the specified id.
    /// If the specified id is not found, throws a <see cref="KeyNotFoundException"/>.
    /// </returns>
    /// <exception cref="EnumerationNotFoundException"><paramref name="id"/> does not exist.</exception>
    /// <seealso cref="FromId(TId,TEnum)"/>
    /// <seealso cref="TryFromId"/>
    public static TEnum FromId(TId? id)
    {
        if (id is null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (!LazyLoadedIds.Value.TryGetValue(id, out var result))
        {
            throw new EnumerationNotFoundException($"No {typeof(TEnum).Name} with Id {id} found.") ;
        }
        return result;
    }

    /// <summary>
    /// Gets an item associated with the specified id.
    /// </summary>
    /// <param name="id">The id of the item to get.</param>
    /// <param name="defaultIdToReturn">The id to return when item not found.</param>
    /// <returns>
    /// The first item found that is associated with the specified id.
    /// If the specified id is not found, returns <paramref name="defaultIdToReturn"/>.
    /// </returns>
    /// <seealso cref="FromId(TId)"/>
    /// <seealso cref="TryFromId"/>
    public static TEnum FromId(TId id, TEnum defaultIdToReturn)
    {
        if (!LazyLoadedIds.Value.TryGetValue(id, out var result))
        {
            return defaultIdToReturn;
        }
        return result;
    }

    /// <summary>
    /// Gets an item associated with the specified id.
    /// </summary>
    /// <param name="id">The id of the item to get.</param>
    /// <param name="result">
    /// When this method returns, contains the item associated with the specified id, if the id is found;
    /// otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <c>true</c> if the <see cref="Enumeration{TEnum}"/> contains an item with the specified name; otherwise, <c>false</c>.
    /// </returns>
    /// <seealso cref="FromId(TId)"/>
    /// <seealso cref="FromId(TId,TEnum)"/>
    public static bool TryFromId(TId id, out TEnum? result)
    {
        return LazyLoadedIds.Value.TryGetValue(id, out result);
    }

    /// <summary>
    /// Gets the item associated with the specified name.
    /// </summary>
    /// <param name="name">The name of the item to get.</param>
    /// <param name="ignoreCase"><c>true</c> to ignore case during the comparison; otherwise, <c>false</c>.</param>
    /// <returns>
    /// The item associated with the specified name.
    /// If the specified name is not found, throws a <see cref="KeyNotFoundException"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="name"/> does not exist.</exception>
    /// <seealso cref="Enumeration{TEnum}.TryFromName(string, out TEnum)"/>
    /// <seealso cref="Enumeration{TEnum}.TryFromName(string, bool, out TEnum)"/>
    public static TEnum FromName(string name, bool ignoreCase = false)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Argument cannot be null or empty.", nameof(name));

        return FromNameLocal(ignoreCase ? LazyLoadedNamesIgnoreCase.Value : LazyLoadedNames.Value);

        TEnum FromNameLocal(Dictionary<string, TEnum> dictionary)
        {
            if (!dictionary.TryGetValue(name, out var result))
            {
                throw new EnumerationNotFoundException($"No {typeof(TEnum).Name} with Name \"{name}\" found.");
            }
            return result;
        }
    }

    /// <summary>
    /// Gets the item associated with the specified name.
    /// </summary>
    /// <param name="name">The name of the item to get.</param>
    /// <param name="result">
    /// When this method returns, contains the item associated with the specified name, if the key is found;
    /// otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <c>true</c> if the <see cref="Enumeration{TEnum}"/> contains an item with the specified name; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <c>null</c>.</exception>
    /// <seealso cref="Enumeration{TEnum}.FromName(string, bool)"/>
    /// <seealso cref="Enumeration{TEnum}.TryFromName(string, bool, out TEnum)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFromName(string name, out TEnum? result) =>
        TryFromName(name, false, out result);

    /// <summary>
    /// Gets the item associated with the specified name.
    /// </summary>
    /// <param name="name">The name of the item to get.</param>
    /// <param name="ignoreCase"><c>true</c> to ignore case during the comparison; otherwise, <c>false</c>.</param>
    /// <param name="result">
    /// When this method returns, contains the item associated with the specified name, if the name is found;
    /// otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <c>true</c> if the <see cref="Enumeration{TEnum}"/> contains an item with the specified name; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <c>null</c>.</exception>
    /// <seealso cref="Enumeration{TEnum}.FromName(string, bool)"/>
    /// <seealso cref="Enumeration{TEnum}.TryFromName(string, out TEnum)"/>
    public static bool TryFromName(string name, bool ignoreCase, out TEnum? result)
    {
        if (string.IsNullOrEmpty(name))
        {
            result = default;
            return false;
        }

        if (ignoreCase)
            return LazyLoadedNamesIgnoreCase.Value.TryGetValue(name, out result);
        else
            return LazyLoadedNames.Value.TryGetValue(name, out result);
    }

    public override bool Equals(object? obj) =>
        obj is Enumeration<TEnum, TId> other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => Name;

    /// <summary>
    /// Returns a id indicating whether this instance is equal to a specified <see cref="Enumeration{TEnum}"/> id.
    /// </summary>
    /// <param name="other">An <see cref="Enumeration{TEnum}"/> id to compare to this instance.</param>
    /// <returns><c>true</c> if <paramref name="other"/> has the same id as this instance; otherwise, <c>false</c>.</returns>
    public virtual bool Equals(Enumeration<TEnum, TId>? other)
    {
        // check if same instance
        if (ReferenceEquals(this, other))
            return true;

        // it's not same instance so
        // check if it's not null and is same id
        if (other is null)
            return false;

        return Id.Equals(other.Id);
    }

    public static bool operator ==(Enumeration<TEnum, TId>? left, Enumeration<TEnum, TId>? right)
    {
        // Handle null on left side
        if (left is null)
            return right is null; // null == null = true

        // Equals handles null on right side
        return left.Equals(right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Enumeration<TEnum, TId> left, Enumeration<TEnum, TId> right) =>
        !(left == right);

    /// <summary>
    /// Compares this instance to a specified <see cref="Enumeration{TEnum}"/> and returns an indication of their relative ids.
    /// </summary>
    /// <param name="other">An <see cref="Enumeration{TEnum}"/> id to compare to this instance.</param>
    /// <returns>A signed number indicating the relative ids of this instance and <paramref name="other"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual int CompareTo(Enumeration<TEnum, TId> other) =>
        Id.CompareTo(other.Id);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Enumeration<TEnum, TId> left, Enumeration<TEnum, TId> right) =>
        left.CompareTo(right) < 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Enumeration<TEnum, TId> left, Enumeration<TEnum, TId> right) =>
        left.CompareTo(right) <= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Enumeration<TEnum, TId> left, Enumeration<TEnum, TId> right) =>
        left.CompareTo(right) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Enumeration<TEnum, TId> left, Enumeration<TEnum, TId> right) =>
        left.CompareTo(right) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator TId?(Enumeration<TEnum, TId>? @enum) =>
        @enum is not null
            ? @enum.Id
            : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Enumeration<TEnum, TId>(TId id) =>
        FromId(id);
}

internal static class Blbla
{
    public static List<TFieldType> GetFieldsOfType<TFieldType>(this Type type)
    {
        return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(p => type.IsAssignableFrom(p.FieldType))
            .Select(pi => (TFieldType)pi.GetValue(null)!)
            .ToList();
    }
}
