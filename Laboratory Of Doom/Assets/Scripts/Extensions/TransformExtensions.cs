using UnityEngine;

public static class TransformExtensions
{
	/// <summary>
	/// Flip the gameobject by alternating the scale between -1 and 1.
	/// </summary>
	/// <param name="scale"></param>
	/// <param name="axis"> A lowercase character represents an axis to flip. </param>
	/// <returns></returns>
	public static void FlipByScale(this Transform transform, char axis)
	{
		Vector3 scale = transform.localScale;
		axis = char.ToLower(axis);

		switch (axis)
		{
			case 'x':
				scale.x *= -1;
				break;

			case 'y':
				scale.y *= -1;
				break;

			case 'z':
				scale.z *= -1;
				break;

			default:
				return;
		}

		transform.localScale = scale;
	}
}