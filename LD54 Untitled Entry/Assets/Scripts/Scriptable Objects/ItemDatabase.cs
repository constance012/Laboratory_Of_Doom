using System.Collections.Generic;
using UnityEngine;
using CSTGames.DataPersistence;

[CreateAssetMenu(fileName = "Empty Item Database", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
	[Header("Base Items")]
	[Space]
	public List<Item> baseItems;

	public Item GetItem(ItemSaveData saveData)
	{
		List<Item> masterList = new List<Item>();

		masterList.AddRange(baseItems);

		Item itemSO = masterList.Find(item => item.itemName.Equals(saveData.itemName));

		Item itemToGet = Instantiate(itemSO);
		itemToGet.name = itemSO.name;

		itemToGet.InitializeSaveData(saveData);

		return itemToGet;
	}
}
