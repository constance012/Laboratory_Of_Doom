using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class Consumable : Item
{
	[Header("Healing amount"), Space]
	public int healingAmount;

	public override void Use()
	{
		base.Use();

		GameObject.FindWithTag("Player").GetComponent<PlayerStats>().Heal(healingAmount);

		Inventory.Instance.UpdateQuantity(id, -1);
	}

	public override string ToString()
	{
		return base.ToString() + "\n" +
				$"<b> +{healingAmount} HP. </b>\n" +
				$"<b> Right Click to use. </b>";
	}
}
