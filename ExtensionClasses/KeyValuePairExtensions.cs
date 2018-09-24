using System.Collections.Generic;

namespace crass
{
public static class KeyValuePairExtensions
{
	public static KeyValuePair<TValue, TKey> Reverse<TKey, TValue>(this KeyValuePair<TKey, TValue> @this)
	{
		return new KeyValuePair<TValue, TKey>(@this.Value, @this.Key);
	}
}
}
