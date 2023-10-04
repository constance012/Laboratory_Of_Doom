using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Base Weapon", menuName = "Inventory/Weapons/Base Weapon")]
public class Weapon : Item
{
	public enum WeaponType { Melee, Ranged }
	public enum WeaponSlot { Primary, Secondary }
	public enum UseType { Automatic, Single, Burst }

	[Header("Weapon Type"), Space]
	public WeaponType type;
	public WeaponSlot weaponSlot;
	public UseType useType;

	[Header("Basic Stats"), Space]
	public int baseDamage;
	public float range;

	[Tooltip("An interval in seconds between each use.")]
	public float useSpeed;

	public ParticleSystem impactEffect;

	public virtual bool FireBullet(Vector2 rayOrigin, Vector2 rayDestination)
	{
		return false;
	}

	public virtual bool FireBullet(Ray2D shootRay)
	{
		return false;
	}

	public override string ToString()
	{
		return base.ToString() + "\n" +
				$"{weaponSlot} weapon.\n" +
				$"Base Damage: {baseDamage}";
	}
}
