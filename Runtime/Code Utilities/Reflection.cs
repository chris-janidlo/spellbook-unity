using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace crass
{
    public static class Reflection
    {
        public static IEnumerable<Type> GetImplementations(Type t)
        {
            return Assembly
                .GetAssembly(t)
                .GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(t));
        }

        public static IEnumerable<Type> GetImplementations<T>()
        {
            return GetImplementations(typeof(T));
        }
    }
}
