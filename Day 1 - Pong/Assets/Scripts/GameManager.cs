using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	[SerializeField] Ball _ballPrefab;

	[Header("UI")]
	[SerializeField] TMP_Text _player1ScoreText;
	[SerializeField] TMP_Text _player2ScoreText;

	private int _player1Score;
	private int _player2Score;

	void Start()
	{
		_player1Score = _player2Score = 0;
	}

	public void StartNewRound()
	{
		Ball ball = Instantiate(_ballPrefab, Vector2.zero, Quaternion.identity);

		ball.Init(this);
	}

	public void PlayerScored(PlayerType player)
	{
		switch(player)
		{
			case PlayerType.Player1:
				_player1Score++;
				break;
			case PlayerType.Player2:
				_player2Score++;
				break;
		}

		UpdateUI();

		StartNewRound();
	}

	private void UpdateUI()
	{
		_player1ScoreText.text = _player1Score.ToString();
		_player2ScoreText.text = _player2Score.ToString();
	}
}
