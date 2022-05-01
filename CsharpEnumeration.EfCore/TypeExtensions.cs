using CSharp.Enumeration;

namespace CsharpEnumeration.EfCore;

internal static class TypeExtensions
{
    internal static Type? GetEnumerationIdType(this Type objectType)
    {
        Type currentType = objectType.BaseType;

        if (currentType == null)
        {
            return null;
        }

        while (currentType != typeof(object))
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(Enumeration<,>))
                return currentType.GenericTypeArguments[1];

            currentType = currentType.BaseType;
        }

        return null;
    }

    internal static bool DerivesFromGenericType(this Type? givenType, Type genericType)
    {
        if (givenType is null)
        {
            return false;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        return DerivesFromGenericType(givenType.BaseType, genericType);
    }
}
