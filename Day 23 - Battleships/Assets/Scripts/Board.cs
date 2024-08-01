using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
	[SerializeField] private Tile _tileUnknown;
	[SerializeField] private Tile _tileMiss;
	[SerializeField] private Tile _tileHit;

	[SerializeField] private Tilemap _tilemapPlayer;
	[SerializeField] private Tilemap _tilemapOpponent;

	public Tilemap PlayerTilemap
	{
		get { return _tilemapPlayer; }
	}

	public Tilemap OpponentTilemap
	{
		get { return _tilemapOpponent; }
	}

	public void Draw(Cell[,] state, Tilemap tilemap)
	{
		int width = state.GetLength(0);
		int height = state.GetLength(1);

		for(int x = 0; x < width; ++x)
		{
			for(int y = 0; y < height; ++y)
			{
				Cell cell = state[x, y];
				tilemap.SetTile(cell.position, GetTile(cell));
			}
		}
	}

	private Tile GetTile(Cell cell)
	{
		if(cell.revealed)
		{
			return GetRevealedTile(cell);
		}
		//else if (cell.type == Cell.Type.Ship) { return _tileHit; }
		// 		else if(cell.flagged)
		// 		{
		// 			return _tileFlag;
		// 		}
		else
		{
			return _tileUnknown;
		}
	}

	private Tile GetRevealedTile(Cell cell)
	{
		switch(cell.type)
		{
			case Cell.Type.Ship:
				return _tileHit;
			case Cell.Type.Water:
				return _tileMiss;
			default:
				return null;
		}
	}
}
