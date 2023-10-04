using System;
using System.Collections.Generic;
using UnityEngine;
using CSTGames.DataPersistence;
using UnityEngine.EventSystems;

public class Inventory : ItemStorage, ISaveDataTransceiver, IPointerEnterHandler, IPointerExitHandler
{
	public static Inventory Instance { get; private set; }

	[Header("Item Database")]
	[Space]
	public ItemDatabase database;
	[ReadOnly] public bool insideInventory;

	[Header("Items List")]
	[Space]
	public List<Item> preplacedItems = new List<Item>();
	public List<Item> items = new List<Item>();

	public bool CanvasActive
	{
		get { return transform.parent.gameObject.activeInHierarchy; }
		set { transform.parent.gameObject.SetActive(value); }
	}

	private InventorySlot[] _slots;
	private bool _initializeOnStartup;

	private void OnDisable()
	{
		TooltipHandler.Hide();
		insideInventory = false;
	}

	protected override void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
		{
			Debug.LogWarning("More than one Instance of Inventory found!!");
			Destroy(gameObject);
			return;
		}

		_slots = transform.GetComponentsInChildren<InventorySlot>("Slots");

		base.Awake();
	}

	private void Start()
	{
		if (!_initializeOnStartup)
		{
			Debug.Log("Inventory initializing...");
			foreach (Item itemSO in preplacedItems)
			{
				Item currentItem = Instantiate(itemSO);
				currentItem.name = itemSO.name;
				Add(currentItem);
			}

			preplacedItems.Clear();

			if (HasAny("Pistol"))
				PlayerActions.weapons[0] = GetItemByName("Pistol") as Weapon;

			if (HasAny("Knife"))
				PlayerActions.weapons[1] = GetItemByName("Knife") as Weapon;

			if (HasAny("Flashlight"))
			{
				Electronic flashLight = GetItemByName("Flashlight") as Electronic;
				flashLight.IsTurnedOn = true;
				PlayerActions.flashlight = flashLight;
			}

			_initializeOnStartup = true;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		insideInventory = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		insideInventory = false;
	}

	#region Inherited Methods.
	public override bool Add(Item target, bool forcedSplit = false)
	{
		bool success = base.AddToList(items, target, forcedSplit, out bool outOfSpace);

		if (outOfSpace)
			GameManager.Instance.ToggleInventory(true);

		return success;
	}

	public override void Remove(Item target, bool forced = false)
	{
		if (!target.isFavorite || forced)
		{
			items.Remove(target);
			onItemChanged?.Invoke();
		}
	}

	public override void Remove(string targetID, bool forced = false)
	{
		Item target = GetItem(targetID);

		if (!target.isFavorite || forced)
		{
			items.Remove(target);
			onItemChanged?.Invoke();
		}
	}

	public override void RemoveWithoutNotify(Item target)
	{
		items.Remove(target);
	}

	public override Item GetItem(string targetID)
	{
		return items.Find(item => item.id.Equals(targetID));
	}

	public override Item GetItemByName(string targetName)
	{
		return items.Find(item => item.itemName.Equals(targetName));
	}

	public override bool HasAny(string targetName)
	{
		return items.Find(item => item.itemName.Equals(targetName)) != null;
	}

	public override bool IsExisting(string targetID)
	{
		return items.Exists(item => item.id == targetID);
	}

	public override void UpdateQuantity(string targetID, int amount, bool setExactAmount = false)
	{
		Item target = GetItem(targetID);

		base.SetQuantity(target, amount, setExactAmount);

		if (target.quantity <= 0)
		{
			Remove(target, true);
			return;
		}

		onItemChanged?.Invoke();
	}

	public void UpdateItemTooltip(int slotIndex)
	{
		_slots[slotIndex].UpdateTooltipContent();
	}

	#region Save and Load Data.
	public void LoadData(GameData gameData)
	{

	}

	public void SaveData(GameData gameData)
	{

	}
	#endregion

	protected override void ReloadUI()
	{
		// Split the master list into 2 smaller lists.
		List<Item> unindexedItems = items.FindAll(item => item.slotIndex == -1);
		List<Item> indexedItems = items.FindAll(item => item.slotIndex != -1);

		// Clear all the _slots.
		Array.ForEach(_slots, (slot) => slot.ClearItem());

		// Load the indexed items first.
		if (indexedItems.Count != 0)
		{
			Action<Item> ReloadIndexedItems = (item) => _slots[item.slotIndex].AddItem(item);
			indexedItems.ForEach(ReloadIndexedItems);
		}

		// Secondly, load the unindexed items to the leftover empty slots.
		if (unindexedItems.Count != 0)
		{
			int i = 0;

			foreach (InventorySlot slot in _slots)
			{
				if (i == unindexedItems.Count)
					break;

				if (slot.currentItem == null)
				{
					unindexedItems[i].slotIndex = slot.transform.GetSiblingIndex();

					slot.AddItem(unindexedItems[i]);

					i++;
				}
			}
		}

		// Update the master list.
		items.Clear();
		items.AddRange(unindexedItems);
		items.AddRange(indexedItems);

		// Sort the list by slot indexes in ascending order.
		items.Sort((a, b) => a.slotIndex.CompareTo(b.slotIndex));
	}
	#endregion
}
