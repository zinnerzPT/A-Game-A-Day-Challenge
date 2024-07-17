using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	[SerializeField] GameObject _canvas;

	[SerializeField] TMP_Text _strokeUI;

	[SerializeField] GameObject _levelCompletePanel;

	[SerializeField] TMP_Text _levelCompleteText;

	[Header("Game Settings")]

	private int _strokes;
	private int _totalStrokes;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(_canvas);
	}

	private void Start()
	{
		UpdateStrokes();
		SceneManager.LoadSceneAsync(1);
	}

	private void UpdateStrokes()
	{
		_strokeUI.SetText("Strokes: " + _strokes);
		_levelCompleteText.SetText("Completed in " + _strokes + " strokes");
	}

	public void IncrementStroke()
	{
		_strokes++;
		_totalStrokes++;
		UpdateStrokes();
	}

	public void LevelCompleted()
	{
		if(_strokes == 1)
			_levelCompleteText.SetText("Hole-In-One!");

		_levelCompletePanel.SetActive(true);
	}

	public void NextLevel()
	{
		LoadLevel(1 + (SceneManager.GetActiveScene().buildIndex % (SceneManager.sceneCountInBuildSettings - 1)));
	}

	private void LoadLevel(int levelIndex)
	{
		if(levelIndex == 1)
			_totalStrokes = 0;
		_strokes = 0;

		_levelCompletePanel.SetActive(false);

		SceneManager.LoadSceneAsync(levelIndex);
		UpdateStrokes();
	}
}
