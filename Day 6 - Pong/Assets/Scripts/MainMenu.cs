using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	SoundManager _soundManager;

	private void Awake()
	{
		_soundManager = FindObjectOfType<SoundManager>();
	}

	public void StartGame(int nrPlayers)
	{
		_soundManager.PlayUISound();
		SceneManager.LoadScene(nrPlayers + 1);
	}
}
