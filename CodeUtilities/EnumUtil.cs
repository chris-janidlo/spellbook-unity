using System;
using System.Linq;
using System.Collections.Generic;

namespace crass
{
public static class EnumUtil
{
	// sorts alphabetically
	public static IEnumerable<T> AllValues<T> () where T : Enum
	{
		// I think this implementation covers cases where an enum has two members with the same values (ie public enum Blah {a = 1, b = 1}). GetValues wouldn't work because "If multiple members have the same value, the returned array includes duplicate values. In this case, calling the GetName method with each value in the returned array does not restore the unique names assigned to members that have duplicate values" (https://docs.microsoft.com/en-us/dotnet/api/system.enum.getvalues?view=netframework-4.8). need to test
		// sort alphabetically because Microsoft specifically dos not specify sort order when two members have the same value and that ain't cool. not at all. not when we want to use this for serialization
		Type t = typeof(T);
		return Enum.GetNames(t).OrderBy(n => n).Select(n => (T) Enum.Parse(t, n));
	}

	public static int NameCount<T> () where T : Enum
	{
		return Enum.GetValues(typeof(T)).Length;
	}
}
}
