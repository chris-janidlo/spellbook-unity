using UnityEngine;

namespace crass
{
public static class VectorIntExtensions
{
	public static Vector2Int DivideBy (this Vector2Int dividend, float divisor)
	{
		return new Vector2Int
		(
			(int) (dividend.x / divisor), 
			(int) (dividend.y / divisor)
		);
	}

	public static Vector3Int DivideBy (this Vector3Int dividend, float divisor)
	{
		return new Vector3Int
		(
			(int) (dividend.x / divisor),
			(int) (dividend.y / divisor),
			(int) (dividend.z / divisor)
		);
	}
}
}
