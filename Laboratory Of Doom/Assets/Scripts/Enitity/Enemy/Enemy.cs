using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
	[Header("References"), Space]
	[SerializeField] private GameObject dmgTextPrefab;
	[SerializeField] private Transform dmgTextLoc;

	[Header("Enemy Stats"), Space]
	[SerializeField] private int damage;
	[SerializeField] private float damageFlashTime;

	// Private fields.
	private Material _enemyMat;

	private void Awake()
	{
		_enemyMat = this.GetComponentInChildren<SpriteRenderer>("Graphics").material;
	}

	public override void TakeDamage(int amount)
	{
		StartCoroutine(TriggerDamageFlash());

		DamageText.Generate(dmgTextPrefab, dmgTextLoc.position, amount.ToString());

		base.TakeDamage(amount);
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		PlayerStats player = collider.GetComponent<PlayerStats>();

		if (player != null)
		{
			player.TakeDamage(damage);
		}
	}

	private void OnTriggerStay2D(Collider2D collider)
	{
		PlayerStats player = collider.GetComponent<PlayerStats>();

		if (player != null)
		{
			player.TakeDamage(damage);
		}
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
}
