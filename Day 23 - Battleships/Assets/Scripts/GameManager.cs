using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
	[SerializeField] Vector2Int _boardSize = new Vector2Int(10, 10);

	[SerializeField] GameObject _gameOverPanel;
	[SerializeField] TMP_Text _gameOverText;

	[SerializeField] Ship[] _shipsToHide;

	private Board _board;

	private Cell[,] _playerState;
	private Cell[,] _opponentState;

	private bool _gameOver;
	private bool _playerTurn = true;

	private void Awake()
	{
		_board = FindObjectOfType<Board>();
	}

	void Start()
	{
		NewGame();
	}

	private void Update()
	{
		if(_gameOver)
			return;

		if(Input.GetMouseButtonDown(0) && _playerTurn)
		{
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3Int cellPosition = _board.OpponentTilemap.WorldToCell(worldPosition);
			Cell cell = GetCell(cellPosition.x, cellPosition.y, _opponentState);
			if(cell.type == Cell.Type.Invalid || cell.revealed)
				return;
			Reveal(cell, ref _opponentState);
			_playerTurn = false;

			Invoke(nameof(PlayCPUTurn), 0.5f);

			CheckWin(_opponentState);
		}
	}

	private void PlayCPUTurn()
	{
		int x, y;
		do
		{
			x = Random.Range(0, _boardSize.x);
			y = Random.Range(0, _boardSize.y);
		} while(_playerState[x, y].revealed);

		Reveal(_playerState[x, y], ref _playerState);
		_playerTurn = true;
		CheckWin(_playerState);
	}

	private void DrawTiles()
	{
		_board.Draw(_playerState, _board.PlayerTilemap);
		_board.Draw(_opponentState, _board.OpponentTilemap);
	}

	private void OnValidate()
	{
		int maxValue = Mathf.Max(_boardSize.x, _boardSize.y);

		for(int i = 0; i < _shipsToHide.Length; ++i)
		{
			_shipsToHide[i].size = Mathf.Clamp(_shipsToHide[i].size, 1, maxValue);
		}
	}

	private void NewGame()
	{
		_playerState = new Cell[_boardSize.x, _boardSize.y];
		_opponentState = new Cell[_boardSize.x, _boardSize.y];
		_gameOver = false;

		GenerateCells();

		GenerateShips(ref _playerState, _board.PlayerTilemap, true);
		GenerateShips(ref _opponentState, _board.OpponentTilemap);

		DrawTiles();
	}

	private void GenerateCells()
	{
		for(int x = 0; x < _boardSize.x; ++x)
		{
			for(int y = 0; y < _boardSize.y; ++y)
			{
				Cell cell = new Cell();
				cell.position = new Vector3Int(x, y, 0);
				cell.type = Cell.Type.Water;
				_playerState[x, y] = cell;
				_opponentState[x, y] = cell;
			}
		}
	}

	private void GenerateShips(ref Cell[,] state, Tilemap tilemap, bool isVisible = false)
	{
		for(int i = 0; i < _shipsToHide.Length; ++i)
		{
			int x, y;
			Orientation o;
			do
			{
				x = Random.Range(0, _boardSize.x);
				y = Random.Range(0, _boardSize.y);
				o = Random.Range(0, 2) == 0 ? Orientation.Vertical : Orientation.Horizontal;
			} while(state[x, y].type != Cell.Type.Water || !IsValidShipPosition(x, y, _shipsToHide[i].size, o, state));

			PlaceShip(x, y, _shipsToHide[i], o, ref state);
			GameObject ship = Instantiate(_shipsToHide[i].prefab, tilemap.CellToWorld(new Vector3Int(x, y)), Quaternion.identity);
			ship.SetActive(isVisible);
			if(o == Orientation.Horizontal)
				ship.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -90.0f));
			else
				ship.transform.position = new Vector2(ship.transform.position.x + 1.0f, ship.transform.position.y);
		}
	}

	private void PlaceShip(int x, int y, Ship ship, Orientation o, ref Cell[,] state)
	{
		for(int i = 0; i < ship.size; ++i)
		{
			state[x, y].type = Cell.Type.Ship;
			if(o == Orientation.Horizontal)
				x++;
			else if(o == Orientation.Vertical)
				y++;
		}
	}

	private void Reveal(Cell cell, ref Cell[,] state)
	{
		if(cell.type == Cell.Type.Invalid || cell.revealed)
			return;

		switch(cell.type)
		{
			case Cell.Type.Water:
				cell.revealed = true;
				state[cell.position.x, cell.position.y] = cell;
				break;
			case Cell.Type.Ship:
				cell.revealed = true;
				state[cell.position.x, cell.position.y] = cell;
				Hit(cell, ref state);
				break;
		}
		DrawTiles();
	}

	private void Hit(Cell cell, ref Cell[,] state)
	{
		// TODO Check if ship was sunk
	}

	private void CheckWin(Cell[,] state)
	{
		for(int x = 0; x < _boardSize.x; ++x)
		{
			for(int y = 0; y < _boardSize.y; ++y)
			{
				Cell cell = state[x, y];

				if(cell.type == Cell.Type.Ship && !cell.revealed)
				{
					return;
				}
			}
		}

		CancelInvoke();

		// If it gets here, the game has ended!
		_gameOver = true;

		_gameOverPanel.SetActive(true);
		_gameOverText.SetText("Game Over");
	}

	private Cell GetCell(int x, int y, Cell[,] state)
	{
		if(IsValidCell(x, y))
		{
			return state[x, y];
		}
		else
		{
			return new Cell();
		}
	}

	private bool IsValidCell(int x, int y)
	{
		return x >= 0 && x < _boardSize.x && y >= 0 && y < _boardSize.y;
	}

	private bool IsValidShipPosition(int x, int y, int shipLength, Orientation orientation, Cell[,] state)
	{
		bool isValid = false;

		for(int i = 0; i < shipLength; ++i)
		{
			switch(orientation)
			{
				case Orientation.Vertical:
					isValid = IsValidCell(x, y + i);
					if(!isValid)
						return isValid;
					isValid &= state[x, y + i].type == Cell.Type.Water;
					if(!isValid)
						return isValid;
					break;
				case Orientation.Horizontal:
					isValid = IsValidCell(x + i, y);
					if(!isValid)
						return isValid;
					isValid &= state[x + i, y].type == Cell.Type.Water;
					if(!isValid)
						return isValid;
					break;
				default:
					return false;
			}
		}

		return isValid;
	}

	public void PlayAgain()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}

public enum Orientation
{
	Vertical,
	Horizontal
}

[System.Serializable]
public struct Ship
{
	public int size;
	public GameObject prefab;
}