using UnityEngine;

public class InventorySlot : StorageSlot
{
	/// <summary>
	/// This method used to catch the dragged item from another slot.
	/// </summary>
	/// <param name="eventData"></param>
	public override void OnDrop(GameObject shipper)
	{
		if (shipper == null)
			return;

		ClickableObject cloneData = shipper.GetComponent<ClickableObject>();
		
		Item droppedItem = cloneData.dragItem;

		if (droppedItem.itemName.Equals("Coin"))
		{
			Inventory.Instance.Add(droppedItem, true);
			return;
		}

		// If this is an empty slot.
		if (currentItem == null)
		{
			// Add the item if it doesn't already exist, otherwise move the item to this slot.
			if (!Inventory.Instance.IsExisting(droppedItem.id))
				Inventory.Instance.Add(droppedItem, true);

			Inventory.Instance.UpdateSlotIndex(droppedItem.id, transform.GetSiblingIndex());
				
			return;
		}

		// If this is the original item that we had dragged.
		else if (currentItem.id.Equals(droppedItem.id))
			return;

		// If this is an item with the same name, check if it can be stacked together.
		else if (currentItem.itemName.Equals(droppedItem.itemName))
		{
			if (currentItem.stackable && currentItem.quantity < currentItem.maxPerStack)
			{
				int totalQuantity = currentItem.quantity + droppedItem.quantity;

				if (totalQuantity > currentItem.maxPerStack)
				{
					int residue = totalQuantity - currentItem.maxPerStack;

					Inventory.Instance.UpdateQuantity(currentItem.id, currentItem.maxPerStack, true);

					cloneData.currentStorage.UpdateQuantity(droppedItem.id, residue, true);

					return;
				}

				else if (totalQuantity == currentItem.maxPerStack)
					Inventory.Instance.UpdateQuantity(currentItem.id, totalQuantity, true);

				// Otherwise, just increase the quantity of the current one.
				else
					Inventory.Instance.UpdateQuantity(currentItem.id, droppedItem.quantity);


				// Set as favorite when needed.
				if (!currentItem.isFavorite && droppedItem.isFavorite)
					Inventory.Instance.SetFavorite(currentItem.id, true);


				// Finally, destroy the dragged one if there's no residue was created.
				cloneData.currentStorage.Remove(droppedItem);
			}

			// If it can not be stacked or its stack is currently full, swap the slot indexes between them.
			else if (!currentItem.stackable || currentItem.quantity == currentItem.maxPerStack)
				SwapSlotIndexes<InventorySlot>(cloneData);

			return;
		}

		// If they're two different items.
		SwapSlotIndexes<InventorySlot>(cloneData);
	}
}
