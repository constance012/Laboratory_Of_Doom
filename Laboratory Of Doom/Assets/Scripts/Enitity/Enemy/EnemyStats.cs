﻿using System;
using UnityEngine;

public class EnemyStats : Entity
{
	public string ID { get; private set; }

	[Header("Enemy Stats"), Space]
	[SerializeField] private int damage;
	[SerializeField] private float knockBackStrength;

	[Space]
	[SerializeField] private Vector2 attackRange;
	[SerializeField] private LayerMask hitLayer;

	[Header("Health Bar"), Space]
	[SerializeField] private WorldHealthBar healthBar;

	// Private fields.
	private Collider2D[] _hitObjects = new Collider2D[2];
	private ContactFilter2D _contactFilter;

	private void Awake()
	{
		_mat = this.GetComponentInChildren<SpriteRenderer>("Graphics").material;
		ID = Guid.NewGuid().ToString();
	}

	protected override void Start()
	{
		base.Start();

		_contactFilter.layerMask = hitLayer;
		_contactFilter.useLayerMask = true;

		healthBar.SetMaxHealth(maxHealth, true);
		healthBar.name = $"{gameObject.name} Health Bar";
	}

	private void LateUpdate()
	{
		int hitColliders = Physics2D.OverlapBox(transform.position, attackRange, 0f, _contactFilter, _hitObjects);

		PlayerStats player = null;

		for (int i = 0; i < hitColliders; i++)
		{
			if (_hitObjects[i] == null)
				continue;

			LevelsNavigationDoor door = _hitObjects[i].GetComponent<LevelsNavigationDoor>();
			player = _hitObjects[i].GetComponent<PlayerStats>();
			
			if (door != null && !door.isOpened)
				return;
		}

		if (player != null)
		{
			player.TakeDamage(damage, false, transform.position, knockBackStrength);
		}
	}

	public override void TakeDamage(int amount, bool weakpointHit, Vector3 attackerPos = default, float knockBackStrength = 0)
	{
		base.TakeDamage(amount, weakpointHit, attackerPos, knockBackStrength);

		healthBar.SetCurrentHealth(_currentHealth);
	}

	public override void Die()
	{
		Destroy(healthBar.gameObject);

		base.Die();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(transform.position, attackRange);
	}
}