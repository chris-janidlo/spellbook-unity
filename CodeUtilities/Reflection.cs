using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace crass
{
public static class Reflection
{
	public static IEnumerable<Type> GetImplementations<T> ()
	{
		return Assembly.GetAssembly(typeof(T))
			.GetTypes()
			.Where
			(
				myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))
			);
	}
}
}
