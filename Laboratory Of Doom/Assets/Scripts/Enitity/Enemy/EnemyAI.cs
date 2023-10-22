using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	private static List<Rigidbody2D> nearbyEnemies;

	public float aggroRange;
	public float spotTimer;

	[Header("Movement Settings"), Space]
	[SerializeField] private float moveSpeed;

	[Header("Repel Settings"), Space]
	[SerializeField] private float repelRange;
	[SerializeField] private float repelAmplitude;

	// Private fields.
	private Rigidbody2D _rb2D;
	private bool _facingRight = true;
	private bool _spottedPlayer;
	private float _spotTimer;

	private void Awake()
	{
		_rb2D = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		if (nearbyEnemies == null)
			nearbyEnemies = new List<Rigidbody2D>();

		nearbyEnemies.Add(_rb2D);
		_spotTimer = spotTimer;
	}

	private void OnDestroy()
	{
		nearbyEnemies.Remove(_rb2D);
	}

	private void FixedUpdate()
	{
		if (PlayerStats.IsDeath)
			return;

		if (!_spottedPlayer)
		{
			 float distanceToPlayer = Vector2.Distance(_rb2D.position, PlayerController.Position);

			if (distanceToPlayer <= aggroRange)
			{
				_spotTimer -= Time.deltaTime;

				if (_spotTimer <= 0)
					_spottedPlayer = true;
			}
			else
				_spotTimer = spotTimer;

			return;
		}

		Vector2 direction = (PlayerController.Position - _rb2D.position).normalized;
		Vector2 velocity = CalculateVelocity(direction);

		CheckFlip();

		_rb2D.velocity = velocity;
	}

	private void CheckFlip()
	{
		float sign = Mathf.Sign(PlayerController.Position.x -  _rb2D.position.x);
		bool mustFlip = (_facingRight && sign < 0f) || (!_facingRight && sign > 0f);

		if (mustFlip)
		{
			transform.Rotate(Vector3.up * 180f);
			_facingRight = !_facingRight;
		}
	}

	private Vector2 CalculateVelocity(Vector2 direction)
	{
		// Enemies will try to avoid each other.
		Vector2 repelForce = Vector2.zero;

		foreach (Rigidbody2D enemy in nearbyEnemies)
		{
			if (enemy == _rb2D)
				continue;

			if (Vector2.Distance(enemy.position, _rb2D.position) <= repelRange)
			{
				Vector2 repelDirection = (_rb2D.position - enemy.position).normalized;
				repelForce += repelDirection;
			}
		}

		Vector2 velocity = direction * moveSpeed;
		velocity += repelForce.normalized * repelAmplitude;

		return velocity;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, repelRange);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, aggroRange);
	}
}
