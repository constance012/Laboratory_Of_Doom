using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "New Base Weapon", menuName = "Inventory/Electronic")]
public class Electronic : Item
{
	[Header("Battery"), Space]
	[Range(0f, 100f)] public float maxBatteryLife;
	[Range(0f, 100f)] public float currentBatteryLife;
	[Tooltip("Amount / sec")] public float batteryDepleteRate;

	public bool IsTurnedOn { get; set; }
	public bool OutOfBattery => currentBatteryLife <= 0f;

	// Private fields.

	public void UpdateBattery(float deltaTime)
	{
		if (currentBatteryLife > 0f)
		{
			currentBatteryLife -= batteryDepleteRate * deltaTime;
			Inventory.Instance.UpdateItemTooltip(slotIndex);
		}
	}

	public override string ToString()
	{
		return base.ToString() + "\n" +
				$"Remaining battery: {currentBatteryLife.ToString("0.0")}%\n" +
				$"Press {InputManager.Instance.GetKeyForAction(KeybindingActions.Flashlight)} to turn ON / OFF.";
	}
}
