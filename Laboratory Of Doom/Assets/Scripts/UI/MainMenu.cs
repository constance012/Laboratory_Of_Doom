using CSTGames.DataPersistence;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private AudioMixer mixer;

	private static bool _hasStartup;

	private void Start()
	{
		#if UNITY_EDITOR
		_hasStartup = false;
		#endif

		if (!_hasStartup)
		{
			InternalInitialization();
			_hasStartup = true;
		}
	}

	public void StartGame()
	{
		SceneManager.LoadSceneAsync("Scenes/Base Scene");
		//GameDataManager.Instance.LoadGame(false);
	}

	public void QuitGame()
	{
		Debug.Log("Quiting player...");
		Application.Quit();
	}

	private void InternalInitialization()
	{
		Debug.Log("Initializing settings internally...");

		mixer.SetFloat("musicVol", UserSettings.MusicVolume);
		mixer.SetFloat("soundsVol", UserSettings.SoundsVolume);
		QualitySettings.SetQualityLevel(UserSettings.QualityLevel);
	}
}
