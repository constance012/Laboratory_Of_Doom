using System.Collections;
using UnityEngine;
using TMPro;
using CSTGames.CommonEnums;
using System.Linq;

public class LevelsNavigationDoor : Interactable
{
	public enum DoorDirection { ToNextLevel, ToPreviousLevel, ToMainMenu, RemainCurrentLevel }
	
	[Header("UI"), Space]
	[SerializeField] private TextMeshProUGUI _levelNameText;

	[Header("Status"), Space]
	[ReadOnly] public bool isOpened;

	[Header("Door Settings"), Space]
	public Item[] keycards;
	public DoorDirection direction;
	public string doorDescription;

	[Header("Sprites"), Space]
	[SerializeField] private Sprite openSprite;
	[SerializeField] private Sprite closeSprite;

	// Private fields.
	private Transform _enemiesContainer;
	private BoxCollider2D _collider;
	private IEnumerator _displayTextRoutine;

	private bool _levelCleared;

	protected override void Awake()
	{
		base.Awake();
		
		_enemiesContainer = GameObject.FindWithTag("EnemyContainer").transform;
		_collider = GetComponent<BoxCollider2D>();
	}

	private IEnumerator Start()
	{
		if (direction == DoorDirection.ToNextLevel)
		{
			yield return new WaitForSeconds(1f);

			_levelNameText = GameObject.FindWithTag("LevelNameText").GetComponent<TextMeshProUGUI>();
			_levelNameText.text = LevelsManager.Instance.CurrentScene.name.ToUpper();

			_displayTextRoutine = DisplayLevelText();
			StartCoroutine(_displayTextRoutine);
		}

		spriteRenderer.sprite = closeSprite;
		isOpened = false;
	}

	private void LateUpdate()
	{
		if (_levelCleared || direction != DoorDirection.ToNextLevel)
			return;

		bool isCleared = _enemiesContainer.childCount == 0;

		if (_levelCleared != isCleared)
		{
			_levelCleared = isCleared;
			_levelNameText.text = "<color=#C88529> THANK YOU! </color> FOR RELEASING OUR SOULS";

			StopCoroutine(_displayTextRoutine);
			StartCoroutine(_displayTextRoutine);
		}
	}

	protected override void TriggerInteraction(float playerDistance)
	{
		base.TriggerInteraction(playerDistance);

		if (InputManager.Instance.GetKeyDown(KeybindingActions.Interact) && !hasInteracted)
			Interact();
	}

	public override void Interact()
	{
		base.Interact();

		if (CheckForKeycards())
		{
			isOpened = true;
			hasInteracted = true;

			spriteRenderer.sprite = openSprite;
			
			Enter();
		}
	}

	protected override void CreatePopupLabel()
	{
		if (!isOpened)
		{
			base.CreatePopupLabel();

			string text = "REQUIRES TO UNLOCK:\n";

			if (keycards.Length == 0)
				text = "REQUIRES TO UNLOCK: NONE.";
			else
				foreach (Item keycard in keycards)
					text += $"<color=#{ColorUtility.ToHtmlStringRGB(keycard.rarity.color)}> {keycard.itemName} </color>,\n";

			clone.SetObjectName(text);
		}
	}

	private bool CheckForKeycards()
	{
		bool allCardMatched = true;

		foreach(Item keycard in keycards)
		{
			if (!Inventory.Instance.HasAny(keycard.itemName))
			{
				allCardMatched = false;
				break;
			}
		}

		return allCardMatched;
	}

	private void Enter()
	{
		if (isOpened)
		{
			switch (direction)
			{
				case DoorDirection.ToNextLevel:
					GameManager.Instance.ShowVictoryScreen();
					break;

				case DoorDirection.ToPreviousLevel:
					LevelsManager.Instance.LoadPreviousLevel();
					break;

				case DoorDirection.ToMainMenu:
					GameManager.Instance.ReturnToMenu();
					break;

				case DoorDirection.RemainCurrentLevel:
					_collider.enabled = false;
					break;
			}

			hasInteracted = true;
		}
	}

	private IEnumerator DisplayLevelText()
	{
		_levelNameText.gameObject.SetActive(true);

		Animator textAnim = _levelNameText.GetComponent<Animator>();

		textAnim.Play("Increase Alpha");

		yield return new WaitForSecondsRealtime(2f);

		textAnim.Play("Decrease Alpha");

		yield return new WaitForSecondsRealtime(.75f);

		_levelNameText.text = LevelsManager.Instance.CurrentScene.name.ToUpper();
	}
}
