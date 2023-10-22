using UnityEngine;

public class ItemPickup : Interactable
{
	[Header("Current Item Info")]
	[Space]

	[Tooltip("The scriptable object represents this item.")]
	public Item itemSO;

	private Item _currentItem;

	private void Start()
	{
		_currentItem = Instantiate(itemSO);
		_currentItem.name = itemSO.name;

		spriteRenderer.sprite = _currentItem.icon;		
	}

	protected override void CheckForInteraction(float mouseDistance, float playerDistance)
	{
		if (playerDistance <= interactDistance)
		{
			TriggerInteraction(playerDistance);
		}
		else
		{
			CancelInteraction(playerDistance);
		}

	}

	protected override void TriggerInteraction(float playerDistance)
	{
		base.TriggerInteraction(playerDistance);

		if (InputManager.Instance.GetKeyDown(KeybindingActions.Interact))
			Interact();
	}

	public override void Interact()
	{
		base.Interact();

		Pickup();
	}

	public override void ExecuteRemoteLogic(bool state)
	{
		
	}

	protected override void CreatePopupLabel()
	{
		Transform foundLabel = worldCanvas.transform.Find("Popup Label");

		string itemName = _currentItem.itemName;
		int quantity = _currentItem.quantity;
		Color textColor = _currentItem.rarity.color;

		// Create a clone if not already exists.
		if (foundLabel == null)
		{
			base.CreatePopupLabel();
			clone.SetObjectName(itemName, quantity, textColor);
		}

		// Otherwise, append to the existing one.
		else
		{
			clone = foundLabel.GetComponent<InteractionPopupLabel>();

			clone.SetObjectName(itemName, quantity, textColor, true);
			clone.RestartAnimation();
		}
	}

	private void Pickup()
	{
		Debug.Log("You're picking up a(n) " + _currentItem.itemName);

		Destroy(clone.gameObject);

		if (Inventory.Instance.Add(_currentItem))
		{
			if (_currentItem.category == ItemCategory.Weapon)
			{
				Weapon weapon = _currentItem as Weapon;

				int slotIndex = (int)weapon.weaponSlot;
				PlayerActions.weapons[slotIndex] = weapon;
			}

			if (_currentItem.itemName.Equals("Flashlight") && _currentItem.category == ItemCategory.Electronic)
				PlayerActions.flashlight = _currentItem as Electronic;

			Destroy(gameObject);
		}

		if (_currentItem.quantity <= 0)
			Destroy(gameObject);
	}
}
