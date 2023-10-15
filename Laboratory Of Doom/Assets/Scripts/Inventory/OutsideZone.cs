using UnityEngine;
using UnityEngine.EventSystems;

public class OutsideZone : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private ClickableObject heldItem;

	public void OnPointerClick(PointerEventData eventData)
	{
		switch (eventData.button)
		{
			case PointerEventData.InputButton.Left:
				heldItem.DisposeItem();
				break;

			case PointerEventData.InputButton.Right:
				ClickableObject.UseCurrentlyHoldingItem();
				break;
		}
	}
}
