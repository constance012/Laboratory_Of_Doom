using System.Collections;
using UnityEngine;

public class TooltipHandler : MonoBehaviour
{
	private static TooltipHandler Instance;

	[SerializeField] private Tooltip tooltip;
	private bool isShowed;
	
	private void Awake()
	{
		tooltip = GetComponentInChildren<Tooltip>(true);
		
		if (Instance == null)
			Instance = this;
		else
		{
			Debug.LogWarning("More than one Instance of Tooltip Handler found!! Destroy the newest one.");
			Destroy(gameObject);
			return;
		}
	}

	public static IEnumerator Show(string contentText, string headerText = "", float delay = .2f)
	{
		if (!Instance.isShowed)
		{
			Instance.tooltip.SetText(contentText, headerText);

			yield return new WaitForSeconds(Mathf.Clamp(delay, .2f, delay));

			Instance.tooltip.gameObject.SetActive(true);
			Instance.isShowed = true;
		}
	}

	public static void Hide()
	{
		if (Instance.isShowed)
		{
			Instance.tooltip.gameObject.SetActive(false);
			Instance.isShowed = false;
		}
	}
}
