using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Inventory/Food")]
public class Food : Item
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
				$"+{healingAmount} HP.\n" +
				$"Right Click to use.";
	}
}
