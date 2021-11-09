using System;
using System.Collections.Generic;

namespace Infra.ExceptionHandling.Extensions
{
  internal static class GenericTypeUtils
  {
    internal static IEnumerable<Type> BaseTypes(this Type type)
    {
      Type? t = type;
      while (true)
      {
        t = t.BaseType;
        if (t == null) break;
        yield return t;
      }
    }

    internal static bool IsParticularGeneric(this Type type, Type generic) =>
      type.IsGenericType && type.GetGenericTypeDefinition() == generic;
  }
}
