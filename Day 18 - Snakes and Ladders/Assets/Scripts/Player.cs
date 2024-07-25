using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] Sprite _humanPlayerSprite;
	[SerializeField] Sprite _cpuPlayerSprite;
	[SerializeField] SpriteRenderer _playerTypeRenderer;

	private GameManager _gameManager;
	private Board _board;

	private int _currentTile;

	private PlayerType _playerType;
	private int _playerNr;

	bool _choosingPlayer = true;

	public PlayerType PlayerType
	{
		get { return _playerType; }
	}

	private void Awake()
	{
		_gameManager = FindObjectOfType<GameManager>();
		_board = FindObjectOfType<Board>();
	}

	public void Move(int tiles)
	{
		StartCoroutine(MoveCoroutine(tiles));
	}

	private IEnumerator MoveCoroutine(int tiles)
	{
		var delay = new WaitForSeconds(0.5f);
		yield return delay;
		for(int i = 1; i <= tiles; ++i)
		{
			transform.position = _board.GetTilePosition(_currentTile + i);
			yield return delay;
		}

		_currentTile = _board.AttemptToLand(_currentTile + tiles);
		transform.position = _board.GetTilePosition(_currentTile);

		if(_board.CheckWin(_currentTile))
		{
			_gameManager.Win(this);
		}
		else
		{
			_gameManager.NextPlayer();
		}
	}

	public void OnMouseDown()
	{
		if(!_choosingPlayer)
			return;

		_playerType = NextPlayerType(_playerType);
		UpdatePlayerTypeDisplay();
	}

	private void UpdatePlayerTypeDisplay()
	{
		_playerTypeRenderer.gameObject.SetActive(true);
		switch(_playerType)
		{
			case PlayerType.None:
				_playerTypeRenderer.gameObject.SetActive(false);
				break;

			case PlayerType.Human:
				_playerTypeRenderer.sprite = _humanPlayerSprite;
				break;

			case PlayerType.CPU:
				_playerTypeRenderer.sprite = _cpuPlayerSprite;
				break;
		}
	}

	public void ResetPlayerTypeDisplay()
	{
		_choosingPlayer = false;
		_playerTypeRenderer.gameObject.SetActive(false);
	}




	public static PlayerType NextPlayerType(PlayerType type)
	{
		switch(type)
		{
			case PlayerType.None:
				return PlayerType.Human;
			case PlayerType.Human:
				return PlayerType.CPU;
			case PlayerType.CPU:
				return PlayerType.None;

			default:
				return PlayerType.None;
		}
	}
}

public enum PlayerType
{
	None,
	Human,
	CPU
}
