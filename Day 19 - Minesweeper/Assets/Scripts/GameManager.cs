using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] Vector2Int _boardSize = new Vector2Int(16, 16);
	[SerializeField] int _numberOfMines = 40;

	[SerializeField] TMP_Text _gameOverText;

	private Board _board;

	private Cell[,] _state;

	private bool _firstPlay;
	private bool _gameOver;

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
		if(Input.GetMouseButtonDown(1))
		{
			Flag();
		}
		if(Input.GetMouseButtonDown(0))
		{
			Reveal();
		}
	}

	private void OnValidate()
	{
		_numberOfMines = Mathf.Clamp(_numberOfMines, 0, _boardSize.x * _boardSize.y - 1);
	}

	private void NewGame()
	{
		_state = new Cell[_boardSize.x, _boardSize.y];
		_gameOver = false;
		_firstPlay = true;

		GenerateCells();

		Camera.main.transform.position = new Vector3(_boardSize.x / 2.0f, _boardSize.y / 2.0f, -10.0f);
		Camera.main.orthographicSize = _boardSize.y / 2.0f + 1.0f;

		_board.Draw(_state);
	}

	private void GenerateCells()
	{
		for(int x = 0; x < _boardSize.x; ++x)
		{
			for(int y = 0; y < _boardSize.y; ++y)
			{
				Cell cell = new Cell();
				cell.position = new Vector3Int(x, y, 0);
				cell.type = Cell.Type.Empty;
				_state[x, y] = cell;
			}
		}
	}

	private void GenerateMines(Cell cell)
	{
		for(int i = 0; i < _numberOfMines; ++i)
		{
			int x, y;
			do
			{
				x = Random.Range(0, _boardSize.x);
				y = Random.Range(0, _boardSize.y);
			} while(_state[x, y].type != Cell.Type.Empty || (x == cell.position.x && y == cell.position.y));

			_state[x, y].type = Cell.Type.Mine;
		}
	}

	private void GenerateNumbers()
	{
		for(int x = 0; x < _boardSize.x; ++x)
		{
			for(int y = 0; y < _boardSize.y; ++y)
			{
				Cell cell = _state[x, y];

				if(cell.type == Cell.Type.Mine)
					continue;

				cell.number = CountSurroundingMines(cell.position);
				cell.type = Cell.Type.Empty;

				_state[x, y] = cell;
			}
		}
	}

	private int CountSurroundingMines(Vector3Int cellPosition)
	{
		int mines = 0;
		for(int x = -1; x <= 1; ++x)
		{
			for(int y = -1; y <= 1; ++y)
			{
				if(x == 0 && y == 0)
					continue;

				if(GetCell(cellPosition.x + x, cellPosition.y + y).type == Cell.Type.Mine)
					mines++;
			}
		}

		return mines;
	}

	private void Reveal()
	{
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3Int cellPosition = _board.Tilemap.WorldToCell(worldPosition);
		Cell cell = GetCell(cellPosition.x, cellPosition.y);

		if(cell.type == Cell.Type.Invalid || cell.revealed || cell.flagged)
			return;

		if(_firstPlay)
		{
			GenerateMines(cell);
			GenerateNumbers();
			_firstPlay = false;
			cell = GetCell(cellPosition.x, cellPosition.y);
		}

		switch(cell.type)
		{
			case Cell.Type.Empty:
				if(cell.number == 0)
					FloodReveal(cell);
				cell.revealed = true;
				_state[cellPosition.x, cellPosition.y] = cell;
				_board.Draw(_state);
				CheckWin();
				break;
			case Cell.Type.Mine:
				Explode(cell);
				break;
		}
	}

	private void Flag()
	{
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3Int cellPosition = _board.Tilemap.WorldToCell(worldPosition);
		Cell cell = GetCell(cellPosition.x, cellPosition.y);

		if(cell.type == Cell.Type.Invalid || cell.revealed)
			return;

		cell.flagged = !cell.flagged;

		_state[cellPosition.x, cellPosition.y] = cell;
		_board.Draw(_state);
	}

	private void Explode(Cell cell)
	{
		_gameOver = true;

		cell.revealed = true;
		cell.exploded = true;

		_state[cell.position.x, cell.position.y] = cell;

		for(int x = 0; x < _boardSize.x; ++x)
		{
			for(int y = 0; y < _boardSize.y; ++y)
			{
				cell = _state[x, y];

				if(cell.type == Cell.Type.Mine)
				{
					cell.revealed = true;
					_state[x, y] = cell;
				}
			}
		}

		_board.Draw(_state);

		_gameOverText.gameObject.SetActive(true);
		_gameOverText.SetText("You exploded!");
	}

	private void CheckWin()
	{
		for(int x = 0; x < _boardSize.x; ++x)
		{
			for(int y = 0; y < _boardSize.y; ++y)
			{
				Cell cell = _state[x, y];

				if(cell.type == Cell.Type.Empty && !cell.revealed)
				{
					return;
				}
			}
		}

		// If it gets here, you won!
		_gameOver = true;

		_gameOverText.gameObject.SetActive(true);
		_gameOverText.SetText("You won!");

		for(int x = 0; x < _boardSize.x; ++x)
		{
			for(int y = 0; y < _boardSize.y; ++y)
			{
				Cell cell = _state[x, y];

				if(cell.type == Cell.Type.Mine)
				{
					cell.flagged = true;
					_state[x, y] = cell;
				}
			}
		}
		_board.Draw(_state);
	}

	private void FloodReveal(Cell cell)
	{
		for(int x = -1; x <= 1; ++x)
		{
			for(int y = -1; y <= 1; ++y)
			{
				Cell adjacent = GetCell(cell.position.x + x, cell.position.y + y);

				if((x == 0 && y == 0) || adjacent.revealed || adjacent.type == Cell.Type.Invalid)
					continue;

				adjacent.revealed = true;

				_state[adjacent.position.x, adjacent.position.y] = adjacent;

				if(adjacent.type == Cell.Type.Empty && adjacent.number == 0)
					FloodReveal(adjacent);

			}
		}
	}

	private Cell GetCell(int x, int y)
	{
		if(IsValidCell(x, y))
		{
			return _state[x, y];
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

	public void PlayAgain()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
