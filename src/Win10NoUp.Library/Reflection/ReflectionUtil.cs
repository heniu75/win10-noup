using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Win10NoUp.Library
{
    public static class ReflectionUtil
    {
        public static List<Type> GetImplementingTypes<T>()
        {
            var type = typeof(T);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(x => x.AssemblyQualifiedName != type.AssemblyQualifiedName);
            return types.ToList();
        }

        public static List<Type> GetImplementingTypesWithAttribute<T, TAttribute>()
        {
            var type = typeof(T);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(x => x.AssemblyQualifiedName != type.AssemblyQualifiedName)
                .Where(x => x.GetCustomAttributes(typeof(TAttribute), true).Length > 0);
            return types.ToList();
        }

        public static List<Type> GetTypesWithAttribute<TAttribute>()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(s => s.GetTypes())
                .Where(x => x.GetCustomAttributes(typeof(TAttribute), true).Length > 0);
            return types.ToList();
        }

        public static List<T> ActivateImplementingTypes<T>(params object[] ctorParams)
        {
            var types = GetImplementingTypes<T>();
            var retVal = new List<T>();
            foreach (var type in types)
            {
                try
                {
                    // we should not attempt to instantial generic types directly 
                    if (!type.IsGenericType)
                    {
                        if ((ctorParams != null) && (ctorParams.Length > 0) && (ctorParams[0] != null))
                        {
                            var instance = (T)Activator.CreateInstance(type, ctorParams);
                            retVal.Add(instance);
                        }
                        else
                        {
                            var instance = (T)Activator.CreateInstance(type);
                            retVal.Add(instance);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Could not activate default ctor for type " + type.AssemblyQualifiedName);
                }
            }
            return retVal;
        }

        public static void InvokeGenericMethod(Type typeWithMethod, Type typeToInvokeWith, string methodName, params object[] arguments)
        {
            // see https://msdn.microsoft.com/en-us/library/system.reflection.methodinfo.makegenericmethod.aspx
            // allows you to call the 'methodName' generic method on typeWithMethod when
            // you only have the type instance 'typeToInvokeWith'
            var typeArguments = arguments.Select(x => x.GetType()).ToArray();
            MethodInfo mi = typeWithMethod.GetMethod(methodName,  typeArguments);
            MethodInfo miConstructed = mi.MakeGenericMethod(typeToInvokeWith);
            miConstructed.Invoke(null, arguments);
        }
    }
}
