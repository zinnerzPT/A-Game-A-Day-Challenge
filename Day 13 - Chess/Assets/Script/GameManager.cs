using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] Piece _piecePrefab;

	private Piece[,] _board = new Piece[8, 8];
	private Dictionary<TeamColor, Piece[]> _players = new Dictionary<TeamColor, Piece[]>();

	private TeamColor _currentPlayer = TeamColor.White;

	bool _gameOver = false;

	private MovePlate[] _movePlates;

	public TeamColor CurrentPlayer
	{
		get => _currentPlayer;
	}

	private void Awake()
	{
		// White Team
		_players.Add(TeamColor.White, new Piece[]
		{
		SpawnPieceOfType(Chesspiece.Rook, TeamColor.White, new Vector2Int(0, 0)),
		SpawnPieceOfType(Chesspiece.Knight, TeamColor.White, new Vector2Int(1, 0)),
		SpawnPieceOfType(Chesspiece.Bishop, TeamColor.White, new Vector2Int(2, 0)),
		SpawnPieceOfType(Chesspiece.Queen, TeamColor.White, new Vector2Int(3, 0)),
		SpawnPieceOfType(Chesspiece.King, TeamColor.White, new Vector2Int(4, 0)),
		SpawnPieceOfType(Chesspiece.Bishop, TeamColor.White, new Vector2Int(5, 0)),
		SpawnPieceOfType(Chesspiece.Knight, TeamColor.White, new Vector2Int(6, 0)),
		SpawnPieceOfType(Chesspiece.Rook, TeamColor.White, new Vector2Int(7, 0)),

		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.White, new Vector2Int(0, 1)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.White, new Vector2Int(1, 1)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.White, new Vector2Int(2, 1)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.White, new Vector2Int(3, 1)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.White, new Vector2Int(4, 1)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.White, new Vector2Int(5, 1)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.White, new Vector2Int(6, 1)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.White, new Vector2Int(7, 1))
		});

		// Black Team
		_players.Add(TeamColor.Black, new Piece[]
		{
		SpawnPieceOfType(Chesspiece.Rook, TeamColor.Black, new Vector2Int(0, 7)),
		SpawnPieceOfType(Chesspiece.Knight, TeamColor.Black, new Vector2Int(1, 7)),
		SpawnPieceOfType(Chesspiece.Bishop, TeamColor.Black, new Vector2Int(2, 7)),
		SpawnPieceOfType(Chesspiece.Queen, TeamColor.Black, new Vector2Int(3, 7)),
		SpawnPieceOfType(Chesspiece.King, TeamColor.Black, new Vector2Int(4, 7)),
		SpawnPieceOfType(Chesspiece.Bishop, TeamColor.Black, new Vector2Int(5, 7)),
		SpawnPieceOfType(Chesspiece.Knight, TeamColor.Black, new Vector2Int(6, 7)),
		SpawnPieceOfType(Chesspiece.Rook, TeamColor.Black, new Vector2Int(7, 7)),

		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.Black, new Vector2Int(0, 6)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.Black, new Vector2Int(1, 6)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.Black, new Vector2Int(2, 6)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.Black, new Vector2Int(3, 6)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.Black, new Vector2Int(4, 6)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.Black, new Vector2Int(5, 6)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.Black, new Vector2Int(6, 6)),
		SpawnPieceOfType(Chesspiece.Pawn, TeamColor.Black, new Vector2Int(7, 6))
		});
	}

	private void Update()
	{
		if(_gameOver == true && Input.GetMouseButtonDown(0))
		{
			_gameOver = false;

			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}
	}


	public Piece GetPiece(Vector2Int pos)
	{
		return _board[pos.x, pos.y];
	}

	public void MovePiece(Piece piece, Vector2Int position)
	{
		if(_board[position.x, position.y] != null)
			Capture(position);

		SetEmpty(piece.Position);
		SetPiece(piece, position);
		piece.DestroyMovePlates();

		NextTurn();
	}

	private void Capture(Vector2Int position)
	{
		Piece capturedPiece = _board[position.x, position.y];
		_board[position.x, position.y] = null;

		if(capturedPiece.PieceType == Chesspiece.King)
			_gameOver = true;

		Destroy(capturedPiece.gameObject);
	}

	private void SetEmpty(Vector2Int position)
	{
		_board[position.x, position.y] = null;
	}
	private void SetPiece(Piece piece, Vector2Int position)
	{
		_board[position.x, position.y] = piece;
		piece.SetPosition(position);
	}

	private Piece SpawnPieceOfType(Chesspiece chesspiece, TeamColor color, Vector2Int position)
	{
		Piece piece = Instantiate(_piecePrefab, Vector3.zero, Quaternion.identity);
		piece.Init(chesspiece, color);
		SetPiece(piece, position);
		return piece;
	}

	public bool IsWithinBoard(Vector2Int pos)
	{
		return !(pos.x < 0 || pos.y < 0 || pos.x >= _board.GetLength(0) || pos.y >= _board.GetLength(1));
	}

	public bool IsGameOver()
	{
		return _gameOver;
	}

	private void NextTurn()
	{
		if(_currentPlayer == TeamColor.White)
			_currentPlayer = TeamColor.Black;
		else
			_currentPlayer = TeamColor.White;
	}
}
