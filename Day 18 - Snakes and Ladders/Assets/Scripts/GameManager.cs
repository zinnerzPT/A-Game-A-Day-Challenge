using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Player[] _players;

	[SerializeField] private GameObject _playerSelectionPanel;
	[SerializeField] private Image _diceRollingPanel;
	[SerializeField] private GameObject _gameOverPanel;
	[SerializeField] private Image _gameOverWinner;

	private Dice _dice;

	private List<Player> _playingPlayers = new List<Player>();
	private int _currentPlayer = 0;

	bool _isPlayerTurn = false;

	private void Awake()
	{
		_dice = FindObjectOfType<Dice>();
		_dice.gameObject.SetActive(false);
	}

	private void Update()
	{
		if(_isPlayerTurn && _playingPlayers[_currentPlayer].PlayerType == PlayerType.CPU)
		{
			RollDice();
		}
	}

	public void StartGame()
	{
		foreach(Player player in _players)
		{
			if(player.PlayerType != PlayerType.None)
				_playingPlayers.Add(player);
		}

		if(_playingPlayers.Count < 1)
			return;

		foreach(Player player in _players)
		{
			player.ResetPlayerTypeDisplay();
		}

		_playerSelectionPanel.SetActive(false);
		_dice.gameObject.SetActive(true);
		_diceRollingPanel.gameObject.SetActive(true);
		_isPlayerTurn = true;
		UpdateCurrentPlayerUI();
	}

	public void RollDice()
	{
		if(_isPlayerTurn)
		{
			_playingPlayers[_currentPlayer].Move(_dice.RollDice());
			_isPlayerTurn = false;
		}
	}

	public void NextPlayer()
	{
		_currentPlayer++;
		if(_currentPlayer >= _playingPlayers.Count)
			_currentPlayer = 0;
		_isPlayerTurn = true;
		UpdateCurrentPlayerUI();
	}

	public void Win(Player player)
	{
		_isPlayerTurn = false;
		_gameOverPanel.SetActive(true);
		_gameOverWinner.sprite = player.GetComponentInChildren<SpriteRenderer>().sprite;
	}

	private void UpdateCurrentPlayerUI()
	{
		_diceRollingPanel.sprite = _playingPlayers[_currentPlayer].GetComponentInChildren<SpriteRenderer>().sprite;
	}

	public void PlayAgain()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
