using UnityEngine;
using CSTGames.CommonEnums;

public class ItemPickup : Interactable
{
	[Header("Current Item Info")]
	[Space]

	[Tooltip("The scriptable object represents this item.")] public Item itemSO;
	public GameObject pickedItemUIPrefab;

	private Item _currentItem;

	private void Start()
	{
		_currentItem = Instantiate(itemSO);
		_currentItem.name = itemSO.name;

		spriteRenderer.sprite = _currentItem.icon;		
	}

	protected override void CheckForInteraction(float mouseDistance, float playerDistance)
	{
		if (mouseDistance <= radius || playerDistance <= interactDistance)
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

		if (InputManager.Instance.GetKeyDown(KeybindingActions.Interact) && playerDistance <= interactDistance)
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

	private void Pickup()
	{
		Debug.Log("You're picking up a(n) " + _currentItem.itemName);

		if (Inventory.instance.Add(_currentItem))
		{
			//NewItemNotifier.Generate(pickedItemUIPrefab, itemSO);
			Destroy(gameObject);
		}
	}
}
