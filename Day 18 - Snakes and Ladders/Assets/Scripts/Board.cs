using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	[SerializeField] private int _boardSize = 100;

	[SerializeField] private int _tilesPerRow = 10;

	[SerializeField] Vector2 _boardOffset;

	[SerializeField] private SnakeOrLadder[] _snakesAndLadders;

	private Dictionary<int, int> _warps = new Dictionary<int, int>();

	private void Awake()
	{
		foreach(SnakeOrLadder snakeOrLadder in _snakesAndLadders)
		{
			_warps.Add(snakeOrLadder.from, snakeOrLadder.to);
		}
	}

	public Vector2 GetTilePosition(int tile)
	{
		if(tile == 0)
			return new Vector2(-1, 0) + _boardOffset;

		if(tile > _boardSize)
			return GetTilePosition(_boardSize - (tile - _boardSize));

		int x;
		int y;

		y = (tile - 1) / _tilesPerRow;

		if(IsEven(y))
			x = (tile - 1) % _tilesPerRow;
		else
			x = _tilesPerRow - ((tile - 1) % _tilesPerRow) - 1;

		return new Vector2(x, y) + _boardOffset;
	}

	public int AttemptToLand(int tile)
	{
		if(tile > _boardSize)
			tile = _boardSize - (tile - _boardSize);
		if(_warps.ContainsKey(tile))
			return _warps[tile];
		return tile;
	}

	internal bool CheckWin(int currentTile)
	{
		return currentTile == _boardSize;
	}

	public bool IsEven(int i)
	{
		return i % 2 == 0;
	}
}
