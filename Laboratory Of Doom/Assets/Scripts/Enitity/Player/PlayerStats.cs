using System.Collections;
using UnityEngine;

public class PlayerStats : Entity
{
	[Header("References"), Space]
	[SerializeField] private GameObject dmgTextPrefab;
	[SerializeField] private Transform dmgTextLoc;

	[Header("Player Stats"), Space]
	[SerializeField] private float invincibilityTime;
	[SerializeField] private float damageFlashTime;

	public static bool IsDeath { get; set; }

	// Private fields.
	private Material _playerMat;
	private float _invincibilityTime;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStatic()
	{
		IsDeath = false;
	}

	private void Awake()
	{
		_playerMat = this.GetComponentInChildren<SpriteRenderer>("Graphics").material;
	}

	protected override void Start()
	{
		base.Start();
		_invincibilityTime = invincibilityTime;
	}

	private void Update()
	{
		if (_invincibilityTime > 0f)
			_invincibilityTime -= Time.deltaTime;
	}

	public override void TakeDamage(int amount)
	{
		if (_currentHealth > 0 && _invincibilityTime <= 0f)
		{
			_currentHealth -= amount;

			AudioManager.Instance.PlayWithRandomPitch("Taking Damage", .7f, 1.2f);
			GameManager.Instance.UpdatePlayerHealth(_currentHealth, maxHealth);

			StartCoroutine(TriggerDamageFlash());

			DamageText.Generate(dmgTextPrefab, dmgTextLoc.position, amount.ToString());

			if (_currentHealth <= 0)
				Die();

			_invincibilityTime = invincibilityTime;
		}
	}

	public void Heal(int amount)
	{
		if (_currentHealth > 0)
		{
			_currentHealth += amount;
			_currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);

			GameManager.Instance.UpdatePlayerHealth(_currentHealth, maxHealth);

			DamageText.Generate(dmgTextPrefab, dmgTextLoc.position, Color.green, false, amount.ToString());
		}
	}

	public override void Die()
	{
		if (deathEffect != null)
		{
			IsDeath = true;

			GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
			effect.transform.localScale = transform.localScale;

			// Destroy effect here.
		}

		GameManager.Instance.ShowGameOverScreen();
		Debug.Log("Player Hit");

		gameObject.SetActive(false);
	}

	private IEnumerator TriggerDamageFlash()
	{
		float flashIntensity;
		float elapsedTime = 0f;

		while (elapsedTime < damageFlashTime)
		{
			elapsedTime += Time.deltaTime;

			flashIntensity = Mathf.Lerp(1f, 0f, elapsedTime / damageFlashTime);
			_playerMat.SetFloat("_FlashIntensity", flashIntensity);

			yield return null;
		}
	}
}
