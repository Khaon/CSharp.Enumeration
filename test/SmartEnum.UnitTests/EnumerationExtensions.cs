using System;
using CSharp.Enumeration;

namespace Smart.Common.Enumeration;


public static class EnumerationExtensions
{
    public static bool IsEnumeration(this Type type) =>
        IsEnumeration(type, out var _);

    public static bool IsEnumeration(this Type? type, out Type[]? genericArguments)
    {
        if (type is null || type.IsAbstract || type.IsGenericTypeDefinition)
        {
            genericArguments = null;
            return false;
        }

        do
        {
            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Enumeration<,>))
            {
                genericArguments = type.GetGenericArguments();
                return true;
            }

            type = type.BaseType;
        }
        while (type is not null);

        genericArguments = null;
        return false;
    }
}