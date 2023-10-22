using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float maxSpeed;
	[SerializeField] private float acceleration;
	[SerializeField] private float deceleration;

	public static Vector2 Position { get; private set; }

	// Private fields.
	private Rigidbody2D _rb2D;
	private Vector2 _movementDirection;
	private Vector2 _previousDirection;
	private float _currentSpeed;

	private void Awake()
	{
		_rb2D = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (GameManager.Instance.GameDone)
			return;

		_movementDirection.x = InputManager.Instance.GetAxisRaw("Horizontal");
		_movementDirection.y = InputManager.Instance.GetAxisRaw("Vertical");
		_movementDirection.Normalize();

		if (_movementDirection.magnitude > .1f)
			_previousDirection = _movementDirection;
	}
	
	private void FixedUpdate()
	{
		if (GameManager.Instance.GameDone)
			return;

		UpdateVelocity();

		Position = _rb2D.position;
	}

	private void UpdateVelocity()
	{
		if (_movementDirection.magnitude > .1f)
		{
			_currentSpeed += acceleration * Time.deltaTime;
			_currentSpeed = Mathf.Min(maxSpeed, _currentSpeed);
			
			_rb2D.velocity = _movementDirection * _currentSpeed;
		}

		else if (_currentSpeed > 0f)
		{
			_currentSpeed -= deceleration * Time.deltaTime;
			_currentSpeed = Mathf.Max(0f, _currentSpeed);

			_rb2D.velocity = _previousDirection * _currentSpeed;
		}
	}
}
