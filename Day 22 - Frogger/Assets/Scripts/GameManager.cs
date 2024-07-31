using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] GameObject _startUI;

	[SerializeField] GameObject _gameHUD;
	[SerializeField] TMP_Text _livesUI;

	[SerializeField] GameObject _gameOverPanel;
	[SerializeField] TMP_Text _gameOverText;

	[SerializeField] Image _livesImage;

	[SerializeField] Sprite[] _digitSprites;

	[Space]
	[SerializeField] Frog _playerPrefab;

	[SerializeField] Objective[] _objectives;

	private int _score = 0;

	private int _playerLives = 2;

	public void StartGame()
	{
		_startUI.SetActive(false);
		_gameHUD.SetActive(true);
		_gameOverPanel.SetActive(false);

		_score = 0;
		_playerLives = 2;
		UpdatePlayerLivesUI();

		StartNewRound();

		foreach(Objective o in _objectives)
		{
			o.Initialize();
		}
	}

	public void GameOver()
	{
		_gameHUD.SetActive(false);
		_gameOverPanel.SetActive(true);
	}

	private void SpawnPlayer()
	{
		Instantiate(_playerPrefab, new Vector3(0.0f, -4.5f), Quaternion.identity);
	}

	private void StartNewRound()
	{
		SpawnPlayer();
	}

	public void Score()
	{
		_score++;
		if(_score == _objectives.Length)
			Win();
		else
			StartNewRound();
	}

	private void Win()
	{
		GameOver();

		_gameOverText.text = "You won!";
	}

	public void Lose()
	{
		if(_playerLives <= 0)
		{
			GameOver();
			_gameOverText.text = "You lost";
		}
		else
		{
			_playerLives--;
			UpdatePlayerLivesUI();
			StartNewRound();
		}
	}

	private void UpdatePlayerLivesUI()
	{
		_livesUI.SetText("Lives: " + _playerLives);
		//_livesImage.sprite = _digitSprites[_playerLives];
	}
}
