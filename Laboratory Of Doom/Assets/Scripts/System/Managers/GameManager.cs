using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
	[Header("References"), Space]
	[SerializeField] private CanvasGroup inventoryCanvas;

	[Space]
	[SerializeField] private Animator gameOverScreen;
	[SerializeField] private Animator victoryScreen;

	[Space]
	[SerializeField] private HealthBar playerHPBar;

	public bool GameDone { get; private set; }

	private void Start()
	{
		ToggleInventory(false);
	}

	private void Update()
	{
		if (!PlayerStats.IsDeath)
		{
			if (InputManager.Instance.GetKeyDown(KeybindingActions.Inventory))
				ToggleInventory(!inventoryCanvas.interactable);
		}
	}

	public void ToggleInventory(bool state)
	{
		inventoryCanvas.alpha = state ? 1f : 0f;
		inventoryCanvas.blocksRaycasts = state;
		inventoryCanvas.interactable = state;

		if (!state)
			Inventory.Instance.OnToggleOff();
	}

	public void UpdateCurrentHealth(int currentHP)
	{
		playerHPBar.SetCurrentHealth(currentHP);
	}

	public void InitializeHealthBar(int initialHP)
	{
		playerHPBar.SetMaxHealth(initialHP);
	}

	/// <summary>
	/// Callback method for the retry button.
	/// </summary>
	public void RestartGame()
	{
		GameDone = false;

		LevelsManager.Instance.LoadSceneAsync("Scenes/Base Scene");
	}

	/// <summary>
	/// Callback method for the return to menu button.
	/// </summary>
	public void ReturnToMenu()
	{
		LevelsManager.Instance.LoadSceneAsync("Scenes/Menu");
	}

	public void ShowGameOverScreen()
	{
		GameDone = true;
		ToggleInventory(false);

		gameOverScreen.gameObject.SetActive(true);
		gameOverScreen.Play("Increase Alpha");
	}

	public void ShowVictoryScreen()
	{
		GameDone = true;
		ToggleInventory(false);

		victoryScreen.gameObject.SetActive(true);
		victoryScreen.Play("Increase Alpha");
	}
}
