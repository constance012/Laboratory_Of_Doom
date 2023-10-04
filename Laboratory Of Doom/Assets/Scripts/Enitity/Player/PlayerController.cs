using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float moveSpeed;
	[SerializeField] private float turnSpeed;

	public static Vector2 Position { get; private set; }

	// Private fields.
	private Rigidbody2D _rb2D;
	private Vector2 _movementVector;

	private void Awake()
	{
		_rb2D = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (GameManager.Instance.GameDone)
			return;

		_movementVector.x = InputManager.Instance.GetAxisRaw("Horizontal");
		_movementVector.y = InputManager.Instance.GetAxisRaw("Vertical");
		_movementVector.Normalize();
	}
	
	private void FixedUpdate()
	{
		if (GameManager.Instance.GameDone)
			return;

		_rb2D.MovePosition(_rb2D.position + _movementVector * moveSpeed * Time.deltaTime);

		Position = _rb2D.position;
	}
}
