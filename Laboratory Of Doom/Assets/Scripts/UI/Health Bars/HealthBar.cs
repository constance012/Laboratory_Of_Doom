using System.Collections;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private Slider mainSlider;
	[SerializeField] private Slider fxSlider;

	[Space, SerializeField] private TextMeshProUGUI displayText;

	[Header("Configuration"), Space]
	[SerializeField] private Gradient healthGradient;

	[Header("Effect Settings"), Space]
	[SerializeField] private float fxDelay;
	[SerializeField] private float fxDuration;

	
	[Space, SerializeField] private Color healthIncreaseColor;
	[SerializeField] private Color healthDecreaseColor;

	// Private fields.
	private float _fxSmoothVel;
	private Image _mainFillRect;
	private Image _fxFillRect;
	private Coroutine _fxCoroutine;

	protected virtual void Awake()
	{
		_mainFillRect = mainSlider.fillRect.GetComponent<Image>();
		_fxFillRect = fxSlider.fillRect.GetComponent<Image>();
	}

	public void SetCurrentHealth(int current)
	{
		if (_fxCoroutine != null)
			StopCoroutine(_fxCoroutine);

		// Health decreasing.
		if (current <= mainSlider.value)
		{
			_fxFillRect.color = healthDecreaseColor;

			mainSlider.value = current;
			_mainFillRect.color = healthGradient.Evaluate(mainSlider.normalizedValue);
		}

		// Health increasing.
		else
		{
			_fxFillRect.color = healthIncreaseColor;
			fxSlider.value = current;
		}

		displayText.text = $"{current} / {mainSlider.maxValue}";

		_fxCoroutine = StartCoroutine(PerformEffect());
	}

	public void SetMaxHealth(int max, bool initialize = true)
	{
		mainSlider.maxValue = max;
		fxSlider.maxValue = max;
		
		if (initialize)
		{
			mainSlider.value = max;
			fxSlider.value = max;
			displayText.text = $"{max} / {max}";
		}
	}

	private IEnumerator PerformEffect()
	{
		yield return new WaitForSeconds(fxDelay);

		if (_fxFillRect.color == healthIncreaseColor)
		{
			while (fxSlider.value != mainSlider.value)
			{
				yield return null;

				mainSlider.value = Mathf.SmoothDamp(mainSlider.value, fxSlider.value, ref _fxSmoothVel, fxDuration);
				_mainFillRect.color = healthGradient.Evaluate(mainSlider.normalizedValue);
			}
		}

		else
		{
			while (fxSlider.value != mainSlider.value)
			{
				yield return null;

				fxSlider.value = Mathf.SmoothDamp(fxSlider.value, mainSlider.value, ref _fxSmoothVel, fxDuration);
			}
		}
	}
}
