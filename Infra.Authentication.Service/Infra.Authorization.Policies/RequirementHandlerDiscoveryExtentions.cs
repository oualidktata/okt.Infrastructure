using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Infra.Authorization.Policies.Requirements.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Authorization.Policies
{
  public static class RequirementHandlerDiscoveryExtentions
  {
    private static readonly Type BaseRequirementHandlerGenericType = typeof(BaseRequirementHandler<>);
    private static readonly Type BaseRequirementHandlerType = typeof(BaseRequirementHandler<BaseRequirement>);

    public static void AddPolicyHandlers(this IServiceCollection services, params Assembly[] policyAssemblies)
    {
      var policyAssembliesIncludingSelf = policyAssemblies.Append(Assembly.GetExecutingAssembly());
      foreach (var policyAssembly in policyAssembliesIncludingSelf)
      {
        foreach (var handlerType in FindHandlerTypes(policyAssembly))
        {
          services.AddScoped(typeof(IAuthorizationHandler), handlerType);
        }
      }
    }

    private static IEnumerable<Type> FindHandlerTypes(Assembly policyAssembly)
    {
      var assemblyTypes = policyAssembly.GetTypes();
      var handlerTypes = GetHandlerTypes(assemblyTypes);
      var nestedHandlerTypes = GetNestedHandlerTypes(assemblyTypes);
      return MergeAllHandlerTypes(handlerTypes, nestedHandlerTypes);
    }

    private static IEnumerable<Type> GetHandlerTypes(Type[] assemblyTypes)
    {
      return assemblyTypes.Where(t => IsRequirementHandler(t));
    }

    private static IEnumerable<Type> GetNestedHandlerTypes(Type[] assemblyTypes)
    {
      var candidates = assemblyTypes.Where(t => IsBaseRequirement(t)).ToList();
      var nestedHandlerTypes = candidates.SelectMany(t => t.GetNestedTypes()).Where(t => IsRequirementHandler(t));
      return nestedHandlerTypes;
    }

    private static IEnumerable<Type> MergeAllHandlerTypes(IEnumerable<Type> handlerTypes, IEnumerable<Type> nestedHandlerTypes)
    {
      return handlerTypes.Union(nestedHandlerTypes);
    }

    private static bool IsBaseRequirement(Type type)
    {
      var target = typeof(BaseRequirement);
      return type != target && type.IsAssignableTo(target);
    }

    private static bool IsRequirementHandler(Type type)
    {
      var target = GetBaseRequirementHandlerConcreteType(type);
      if (type.IsAssignableTo(target) && type != BaseRequirementHandlerType && type != target)
      {
        return true;
      }

      return false;
    }

    private static Type? GetBaseRequirementHandlerConcreteType(Type type)
    {
      if (type.BaseType != null)
      {
        var genericArguments = type.BaseType!.GetGenericArguments();
        if (genericArguments.Length == 1)
        {
          var genericType = type.BaseType.GetGenericTypeDefinition();
          if (genericType.IsAssignableTo(BaseRequirementHandlerGenericType))
          {
            return BaseRequirementHandlerGenericType.MakeGenericType(genericArguments);
          }
        }
      }

      return null;
    }
  }
}
