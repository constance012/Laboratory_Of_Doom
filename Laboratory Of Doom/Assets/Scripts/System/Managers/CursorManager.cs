using System;
using UnityEngine;

public enum CursorTextureType { Default, Crosshair }

public class CursorManager : Singleton<CursorManager>
{

	[Serializable]
	public struct CustomCursor
	{
		public CursorTextureType type;

		public Texture2D texture;
		public Vector2 hotSpot;
	}

	[Header("Custom Cursors"), Space]
	[SerializeField] private CustomCursor defaultCursor;
	[SerializeField] private CustomCursor crosshairCursor;

	private void Start()
	{
		SwitchCursorTexture(CursorTextureType.Default);
	}

	public void SwitchCursorTexture(CursorTextureType type)
	{
		switch (type)
		{
			case CursorTextureType.Default:
				Cursor.SetCursor(defaultCursor.texture, defaultCursor.hotSpot, CursorMode.Auto);
				break;

			case CursorTextureType.Crosshair:
				Cursor.SetCursor(crosshairCursor.texture, crosshairCursor.hotSpot, CursorMode.Auto);
				break;
		}
	}

	public void SetVisible(bool visible)
	{
		Cursor.visible = visible;
	}
}
