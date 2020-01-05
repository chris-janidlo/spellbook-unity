using System;
using System.Collections.Generic;

namespace crass
{
public static class EnumUtil
{
	// from https://stackoverflow.com/a/972323/5931898
	public static IEnumerable<T> AllValues<T> ()
	{
		return (T[]) Enum.GetValues(typeof(T));
	}
}
}
