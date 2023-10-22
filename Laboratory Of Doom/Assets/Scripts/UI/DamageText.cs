using UnityEngine;
using TMPro;

/// <summary>
/// A class to generates an UI popup text.
/// </summary>
public class DamageText : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private TextMeshProUGUI displayText;
	
	[Header("Configurations"), Space]
	public float disappearSpeed = 3f;
	[Min(1f)] public float maxLifeTime = 1f;

	[Space, Min(.5f)] public float maxVelocity;
	public AnimationCurve velocityCurve;

	[Header("Font and Colors"), Space]
	public float startFontSize = .15f;
	
	// Private fields.
	private Color _currentTextColor;
	
	private float _smoothVel;
	private float _currentLifeTime;
	private bool _weakpointHit;

	private void Update()
	{
		if (_currentLifeTime >= maxLifeTime)
		{
			Destroying();
			return;
		}

		float lifeTimePassed = Mathf.Min(_currentLifeTime / maxLifeTime, 1f);
		float yVelocity = velocityCurve.Evaluate(lifeTimePassed) * maxVelocity;

		float selectedFontSize = _weakpointHit ? startFontSize * 1.5f : startFontSize;
		float selectedVelocity = _weakpointHit ? yVelocity * 1.5f : yVelocity;

		// Gradually move up.
		transform.position += selectedVelocity * Time.deltaTime * Vector3.up;

		displayText.fontSize = Mathf.SmoothDamp(displayText.fontSize, selectedFontSize, ref _smoothVel, .05f);

		_currentLifeTime += Time.deltaTime;
	}

	#region Generate Method Overloads
	// Default color is red, and parent is world canvas.
	public static DamageText Generate(GameObject prefab, Vector3 pos, bool weakpointHit, string textContent)
	{
		Transform canvas = GameObject.FindWithTag("WorldCanvas").transform;
		GameObject dmgTextObj = Instantiate(prefab, pos, Quaternion.identity);
		dmgTextObj.transform.SetParent(canvas, true);

		DamageText dmgText = dmgTextObj.GetComponent<DamageText>();

		Color textColor = weakpointHit ? new Color(.821f, .546f, .159f) : Color.red;

		dmgText.Setup(textColor, textContent, weakpointHit);
		return dmgText;
	}

	// Default parent is world canvas.
	public static DamageText Generate(GameObject prefab, Vector3 pos, Color txtColor, bool weakpointHit, string textContent)
	{
		Transform canvas = GameObject.FindWithTag("WorldCanvas").transform;

		GameObject dmgTextObj = Instantiate(prefab, pos, Quaternion.identity);
		dmgTextObj.transform.SetParent(canvas, true);

		DamageText dmgText = dmgTextObj.GetComponent<DamageText>();

		dmgText.Setup(txtColor, textContent, weakpointHit);
		return dmgText;
	}
	#endregion

	private void Setup(Color txtColor, string textContent, bool weakpointHit)
	{
		displayText.text = textContent.ToUpper();
		_currentTextColor = txtColor;

		displayText.color = _currentTextColor;
		displayText.fontSize = 0f;

		_weakpointHit = weakpointHit;
	}

	private void Destroying()
	{
		_currentTextColor.a -= disappearSpeed * Time.deltaTime;
		displayText.color = _currentTextColor;

		if (_currentTextColor.a < 0f)
			Destroy(gameObject);
	}
}
