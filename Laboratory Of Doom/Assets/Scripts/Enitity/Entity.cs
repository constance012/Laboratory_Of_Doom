using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("General Stats"), Space]
    public int maxHealth;
    public GameObject deathEffect;

    protected int _currentHealth;

    protected virtual void Start()
    {
        _currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
		AudioManager.Instance.PlayWithRandomPitch("Taking Damage", .7f, 1.2f);
		_currentHealth -= amount;

        if (_currentHealth <= 0)
            Die();
    }

    public virtual void Die()
    {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            effect.transform.localScale = transform.localScale;
            
            // Destroy effect here.
        }

        Destroy(gameObject);
    }
}
