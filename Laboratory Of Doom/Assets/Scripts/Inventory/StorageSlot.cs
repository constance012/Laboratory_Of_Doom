using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class StorageSlot : MonoBehaviour
{
	[Header("Current Item")]
	[Space]
	public Item currentItem;

	// Protected fields.
	protected Image icon;
	protected TextMeshProUGUI quantity;

	protected TooltipTrigger tooltip;

	protected virtual void Awake()
	{
		icon = transform.GetComponentInChildren<Image>("Item Button/Icon");

		quantity = transform.GetComponentInChildren<TextMeshProUGUI>("Item Button/Quantity");
		tooltip = GetComponent<TooltipTrigger>();
	}

	public virtual void AddItem(Item newItem)
	{
		currentItem = newItem;

		icon.sprite = currentItem.icon;
		icon.enabled = true;

		if (currentItem.stackable)
		{
			quantity.text = currentItem.quantity.ToString();
			quantity.enabled = true;
		}
		else
		{
			quantity.text = "1";
			quantity.enabled = false;
		}

		tooltip.header = currentItem.itemName;
		tooltip.content = currentItem.ToString();
		tooltip.popupDelay = .5f;
	}

	public virtual void ClearItem()
	{
		currentItem = null;

		icon.sprite = null;
		icon.enabled = false;

		quantity.text = "";
		quantity.enabled = false;

		tooltip.header = "";
		tooltip.content = "";
		tooltip.popupDelay = 0f;
	}

	public void UseItem()
	{
		// Use the item if it's not null and be used.
		if (currentItem != null && currentItem.canBeUsed)
			currentItem.Use();
	}

	public void UpdateTooltipContent()
	{
		tooltip.content = currentItem.ToString();
	}

	public abstract void OnDrop(GameObject shipper);

	protected void SwapSlotIndexes<TSlot>(ClickableObject cloneData) where TSlot : StorageSlot
	{
		Item droppedItem = cloneData.dragItem;
		ItemStorage currentStorage = cloneData.currentStorage;

		int senderIndex = droppedItem.slotIndex;
		int destinationIndex = currentItem.slotIndex;

		// If swap within the current storage.
		if (cloneData.FromSameStorageSlot<TSlot>())
		{
			if (!currentStorage.IsExisting(droppedItem.id))
				currentStorage.Add(droppedItem, true);

			// Swap their slot indexes.
			currentStorage.UpdateSlotIndex(currentItem.id, senderIndex);
			currentStorage.UpdateSlotIndex(droppedItem.id, destinationIndex);
		}
	}
}
