using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] GameObject _startUI;

	[SerializeField] GameObject _gameHUD;
	[SerializeField] TMP_Text _scoreText;
	[SerializeField] TMP_Text _highscoreText;

	[SerializeField] GameObject _gameOverPanel;
	[SerializeField] TMP_Text _gameOverScoreText;
	[SerializeField] GameObject _gameOverHighscore;

	private int _score;
	private static int _highscore;

	public bool isRunning { get; private set; }
	public bool isGameOver { get; private set; }

	private void Awake()
	{
		isRunning = false;
	}

	private void Start()
	{
		UpdateScoreUI();
	}

	public void StartGame()
	{
		isRunning = true;
		_startUI.SetActive(false);
		_gameHUD.SetActive(true);
	}

	public void AddScore()
	{
		_score++;
		UpdateScoreUI();
	}

	public void GameOver()
	{
		if(_score > _highscore)
		{
			// Highscore popup
			_highscore = _score;
			_gameOverHighscore.SetActive(true);
		}
		isRunning = false;
		isGameOver = true;
		_gameHUD.SetActive(false);
		_gameOverPanel.SetActive(true);
	}

	public void PlayAgain()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void UpdateScoreUI()
	{
		_scoreText.SetText("Score: " + _score.ToString());
		_highscoreText.SetText("Highscore: " + _highscore.ToString());

		_gameOverScoreText.SetText("Score: " + _score.ToString());
	}
}
