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
	[SerializeField] private TextMeshProUGUI healthText;

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
	}

	public void UpdatePlayerHealth(int maxHP, int currentHP)
	{
		healthText.text = $"{maxHP} / {currentHP}";
	}

	public void RestartGame()
	{
		GameDone = false;

		LevelsManager.Instance.LoadSceneAsync("Scenes/Base Scene");
	}

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
