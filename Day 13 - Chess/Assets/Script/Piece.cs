using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
	[SerializeField] private MovePlate _movePlatePrefab;

	private GameManager _gameManager;
	private static List<MovePlate> _movePlates = new List<MovePlate>();

	private Vector2Int _position;

	private SpriteRenderer _spriteRenderer;

	Chesspiece _chesspiece;
	TeamColor _color;


	public Vector2Int Position
	{
		get => _position;
	}

	public Chesspiece PieceType
	{
		get => _chesspiece;
	}

	public TeamColor Color
	{
		get => _color;
	}


	private void Awake()
	{
		_gameManager = FindObjectOfType<GameManager>();
	}


	public void Init(Chesspiece chesspiece, TeamColor color)
	{
		ChessAssets assets = FindObjectOfType<ChessAssets>();
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		_chesspiece = chesspiece;
		_color = color;

		_spriteRenderer.sprite = assets.GetSprite(chesspiece, color);
	}

	public void SetPosition(Vector2Int newPosition)
	{
		_position = newPosition;

		transform.position = new Vector3(_position.x, _position.y);
	}

	private void OnMouseUp()
	{
		DestroyMovePlates();

		if(_gameManager.IsGameOver() || _gameManager.CurrentPlayer != _color)
			return;

		InitMovePlates();
	}

	public void DestroyMovePlates()
	{
		for(int i = 0; i < _movePlates.Count; ++i)
		{
			Destroy(_movePlates[i].gameObject);
		}

		_movePlates.Clear();
	}

	private void InitMovePlates()
	{
		// Instantiate them and 

		switch(_chesspiece)
		{
			case Chesspiece.Pawn:
				if(_color == TeamColor.White)
				{
					PawnMovePlate(_position + Vector2Int.up);

					if(_position.y == 1 && _gameManager.GetPiece(_position + Vector2Int.up) == null)
						PawnMovePlate(_position + Vector2Int.up + Vector2Int.up, false);

				}
				else if(_color == TeamColor.Black)
				{
					PawnMovePlate(_position + Vector2Int.down);

					if(_position.y == 6 && _gameManager.GetPiece(_position + Vector2Int.down) == null)
						PawnMovePlate(_position + Vector2Int.down + Vector2Int.down, false);
				}
				break;
			case Chesspiece.Knight:
				LMovePlate();
				break;
			case Chesspiece.Bishop:
				LineMovePlate(1, 1);
				LineMovePlate(-1, 1);
				LineMovePlate(-1, -1);
				LineMovePlate(1, -1);
				break;
			case Chesspiece.Rook:
				LineMovePlate(1, 0);
				LineMovePlate(0, 1);
				LineMovePlate(-1, 0);
				LineMovePlate(0, -1);
				break;
			case Chesspiece.Queen:
				LineMovePlate(1, 0);
				LineMovePlate(1, 1);
				LineMovePlate(0, 1);
				LineMovePlate(-1, 1);
				LineMovePlate(-1, 0);
				LineMovePlate(-1, -1);
				LineMovePlate(0, -1);
				LineMovePlate(1, -1);
				break;
			case Chesspiece.King:
				AdjacentMovePlate();
				break;
		}
	}

	private void LineMovePlate(int xIncrement, int yIncrement)
	{
		//_gameManager

		int x = _position.x + xIncrement;
		int y = _position.y + yIncrement;

		while(_gameManager.IsWithinBoard(new Vector2Int(x, y)))
		{
			if(_gameManager.GetPiece(new Vector2Int(x, y)) != null)
			{
				if(_gameManager.GetPiece(new Vector2Int(x, y)).Color != _color)
					SpawnMovePlate(new Vector2Int(x, y));
				break;
			}


			SpawnMovePlate(new Vector2Int(x, y));
			x += xIncrement;
			y += yIncrement;
		}
	}

	private void LMovePlate()
	{
		PointMovePlate(_position + new Vector2Int(1, 2));
		PointMovePlate(_position + new Vector2Int(1, -2));

		PointMovePlate(_position + new Vector2Int(-1, 2));
		PointMovePlate(_position + new Vector2Int(-1, -2));

		PointMovePlate(_position + new Vector2Int(2, 1));
		PointMovePlate(_position + new Vector2Int(-2, 1));

		PointMovePlate(_position + new Vector2Int(2, -1));
		PointMovePlate(_position + new Vector2Int(-2, -1));
	}

	private void AdjacentMovePlate()
	{
		PointMovePlate(_position + new Vector2Int(1, 0));
		PointMovePlate(_position + new Vector2Int(1, 1));
		PointMovePlate(_position + new Vector2Int(0, 1));
		PointMovePlate(_position + new Vector2Int(-1, 1));
		PointMovePlate(_position + new Vector2Int(-1, 0));
		PointMovePlate(_position + new Vector2Int(-1, -1));
		PointMovePlate(_position + new Vector2Int(0, -1));
		PointMovePlate(_position + new Vector2Int(1, -1));
	}

	private void PawnMovePlate(Vector2Int pos, bool canCapture = true)
	{
		if(_gameManager.IsWithinBoard(pos))
		{
			if(_gameManager.GetPiece(pos) == null)
			{
				SpawnMovePlate(pos);
			}

			if(!canCapture)
				return;

			if(_gameManager.IsWithinBoard(pos + Vector2Int.right) &&
				_gameManager.GetPiece(pos + Vector2Int.right) != null &&
				_gameManager.GetPiece(pos + Vector2Int.right).Color != _color)
			{
				SpawnMovePlate(pos + Vector2Int.right);
			}

			if(_gameManager.IsWithinBoard(pos + Vector2Int.left) &&
				_gameManager.GetPiece(pos + Vector2Int.left) != null &&
				_gameManager.GetPiece(pos + Vector2Int.left).Color != _color)
			{
				SpawnMovePlate(pos + Vector2Int.left);
			}
		}
	}

	private void PointMovePlate(Vector2Int pos)
	{
		if(_gameManager.IsWithinBoard(pos) && (_gameManager.GetPiece(pos) == null || _gameManager.GetPiece(pos).Color != _color))
		{
			SpawnMovePlate(pos);
		}
	}

	private void SpawnMovePlate(Vector2Int pos)
	{
		MovePlate movePlate = Instantiate(_movePlatePrefab, Vector3.zero, Quaternion.identity);
		movePlate.Init(this, pos);
		_movePlates.Add(movePlate);
	}
}

public enum Chesspiece
{
	Pawn = 0,
	Knight = 1,
	Bishop = 2,
	Rook = 3,
	Queen = 4,
	King = 5
}

public enum TeamColor
{
	White = 0,
	Black = 1,
	Blue = 2,
	Green = 3,
	Red = 4
}
