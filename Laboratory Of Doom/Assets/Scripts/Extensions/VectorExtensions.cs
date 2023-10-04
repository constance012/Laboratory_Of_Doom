using UnityEngine;

public static class VectorExtensions
{
	/// <summary>
	/// Flip the gameobject by alternating the scale between -1 and 1.
	/// </summary>
	/// <param name="scale"></param>
	/// <param name="axis"> A lowercase character represents an axis to flip. </param>
	/// <returns></returns>
	public static Vector3 FlipByScale(this Vector3 scale, char axis)
	{
		Vector3 temp = scale;
		axis = char.ToLower(axis);

		switch (axis)
		{
			case 'x':
				temp.x *= -1;
				break;

			case 'y':
				temp.y *= -1;
				break;

			case 'z':
				temp.z *= -1;
				break;

			default:
				return scale;
		}

		return temp;
	}
}