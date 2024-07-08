using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] Ball _ballPrefab;

	[Header("UI")]
	[SerializeField] TMP_Text _player1ScoreText;
	[SerializeField] TMP_Text _player2ScoreText;

	[Header("Game Over")]
	[SerializeField] TMP_Text _gameOverText;

	private int _player1Score;
	private int _player2Score;

	Paddle[] _players;

	SoundManager _soundManager;

	private void Awake()
	{
		_players = FindObjectsByType<Paddle>(FindObjectsSortMode.None);

		_soundManager = FindObjectOfType<SoundManager>();
	}

	void Start()
	{
		_player1Score = _player2Score = 0;
	}

	public void PlayUISound()
	{
		_soundManager.PlayUISound();
	}

	public void StartNewRound()
	{
		Ball ball = Instantiate(_ballPrefab, Vector2.zero, Quaternion.identity);

		ball.Init(this);

		for(int i = 0; i < _players.Length; ++i)
		{
			_players[i].SetBall(ball);
		}
	}

	public void PlayerScored(PlayerType player)
	{
		switch(player)
		{
			case PlayerType.Player1:
				_player1Score++;
				break;
			case PlayerType.Player2:
			case PlayerType.CPU:
				_player2Score++;

				bool isCpu = false;
				for(int i = 0; i < _players.Length; ++i)
				{
					isCpu |= _players[i].PlayerType == PlayerType.CPU;
				}

				if(isCpu)
					player = PlayerType.CPU;
				break;
		}

		UpdateUI();

		_soundManager.PlayScoreSound();

		if(_player1Score < 5 && _player2Score < 5)
		{
			StartNewRound();
		}
		else
		{
			EndGame(player);
		}
	}

	private void UpdateUI()
	{
		_player1ScoreText.text = _player1Score.ToString();
		_player2ScoreText.text = _player2Score.ToString();
	}

	private void EndGame(PlayerType winner)
	{
		_gameOverText.SetText(winner.ToString() + " wins!");
		_gameOverText.gameObject.SetActive(true);
	}

	public void BackToMenu()
	{
		SceneManager.LoadScene(1);
	}
}
