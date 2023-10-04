using UnityEngine;

public static class Physics2DExtensions
{
	public static bool Raycast(Ray2D ray, out RaycastHit2D hitInfo, float distance, int layerMask)
	{
		Vector2 origin = ray.origin;
		Vector2 direction = ray.direction;

		hitInfo = Physics2D.Raycast(origin, direction, distance, layerMask);

		return hitInfo.collider != null;
	}

	public static bool Raycast(Vector2 origin, Vector2 direction, out RaycastHit2D hitInfo, float distance, int layerMask)
	{
		hitInfo = Physics2D.Raycast(origin, direction, distance, layerMask);

		return hitInfo.collider != null;
	}
}
