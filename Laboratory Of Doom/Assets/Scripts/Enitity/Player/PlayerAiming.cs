using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
	[Header("Player Graphics"), Space]
	[SerializeField] private Transform playerGraphics;
	[SerializeField] private Transform gunSprite;

	private bool _facingRight = true;
	private float _lookAngle;

	private void Update()
	{
		if (GameManager.Instance.GameDone)
			return;

		LookAtMouse();
		CheckFlip();
	}

	private void LookAtMouse()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 lookDirection = (mousePos - PlayerController.Position).normalized;

		_lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

		transform.eulerAngles = Vector3.forward * _lookAngle;
	}

	private void CheckFlip()
	{		
		bool mustFlip = (_facingRight && Mathf.Abs(_lookAngle) > 90f) || (!_facingRight && Mathf.Abs(_lookAngle) <= 90f);

		if (mustFlip)
		{
			playerGraphics.FlipByScale('x');
			gunSprite.FlipByScale('y');

			_facingRight = !_facingRight;
		}
	}
}