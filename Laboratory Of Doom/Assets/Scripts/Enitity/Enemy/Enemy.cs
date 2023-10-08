using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
	[Header("References"), Space]
	[SerializeField] private GameObject dmgTextPrefab;
	[SerializeField] private Transform dmgTextLoc;

	[Header("Enemy Stats"), Space]
	[SerializeField] private int damage;
	[SerializeField] private Vector2 attackRange;
	[SerializeField] private LayerMask hitLayer;

	[Space]
	[SerializeField] private float damageFlashTime;

	// Private fields.
	private Material _enemyMat;
	private Collider2D[] _hitObjects = new Collider2D[2];
	private ContactFilter2D _contactFilter;

	private void Awake()
	{
		_enemyMat = this.GetComponentInChildren<SpriteRenderer>("Graphics").material;
	}

	protected override void Start()
	{
		base.Start();

		_contactFilter.layerMask = hitLayer;
		_contactFilter.useLayerMask = true;
	}

	private void LateUpdate()
	{
		int colliders = Physics2D.OverlapBox(transform.position, attackRange, 0f, _contactFilter, _hitObjects);

		PlayerStats player = null;

		for (int i = 0; i < colliders; i++)
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
			player.TakeDamage(damage);
		}
	}

	public override void TakeDamage(int amount)
	{
		StartCoroutine(TriggerDamageFlash());

		DamageText.Generate(dmgTextPrefab, dmgTextLoc.position, amount.ToString());

		base.TakeDamage(amount);
	}

	private IEnumerator TriggerDamageFlash()
	{
		float flashIntensity;
		float elapsedTime = 0f;

		while (elapsedTime < damageFlashTime)
		{
			elapsedTime += Time.deltaTime;

			flashIntensity = Mathf.Lerp(1f, 0f, elapsedTime / damageFlashTime);
			_enemyMat.SetFloat("_FlashIntensity", flashIntensity);

			yield return null;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(transform.position, attackRange);
	}
}
