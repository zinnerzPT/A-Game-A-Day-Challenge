using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class Board : MonoBehaviour
{
	[SerializeField] private Tile _tileUnknown;
	[SerializeField] private Tile _tileMine;
	[SerializeField] private Tile _tileExploded;
	[SerializeField] private Tile _tileFlag;
	[SerializeField] private Tile[] _tileEmpty;

	private Tilemap _tilemap;

	public Tilemap Tilemap
	{
		get { return _tilemap; }
	}

	private void Awake()
	{
		_tilemap = GetComponent<Tilemap>();
	}

	public void Draw(Cell[,] state)
	{
		int width = state.GetLength(0);
		int height = state.GetLength(1);

		for(int x = 0; x < width; ++x)
		{
			for(int y = 0; y < height; ++y)
			{
				Cell cell = state[x, y];
				_tilemap.SetTile(cell.position, GetTile(cell));
			}
		}
	}

	private Tile GetTile(Cell cell)
	{
		if(cell.revealed)
		{
			return GetRevealedTile(cell);
		}
		else if(cell.flagged)
		{
			return _tileFlag;
		}
		else
		{
			return _tileUnknown;
		}
	}

	private Tile GetRevealedTile(Cell cell)
	{
		switch(cell.type)
		{
			case Cell.Type.Mine:
				return cell.exploded ? _tileExploded : _tileMine;
			case Cell.Type.Empty:
				return _tileEmpty[cell.number];
			default:
				return null;
		}
	}
}
