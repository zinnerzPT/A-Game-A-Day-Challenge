using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Tilemap _currentState;
	[SerializeField] private Tilemap _nextState;

	[SerializeField] private Tile _aliveTile;
	[SerializeField] private Tile _deadTile;

	[SerializeField] private Pattern _pattern;

	[SerializeField] private float _updateInterval = 0.05f;

	private HashSet<Vector3Int> _aliveCells;
	private HashSet<Vector3Int> _cellsToCheck;

	[Header("Game Of Life Rules")]
	[SerializeField] private int _neighborsToUnderpopulate = 1;
	[SerializeField] private int _neighborsToOverpopulate = 4;
	[SerializeField] private int _neighborsToReproduce = 3;


	public int population { get; private set; }
	public int iterations { get; private set; }
	public float time { get; private set; }



	private void Awake()
	{
		_aliveCells = new HashSet<Vector3Int>();
		_cellsToCheck = new HashSet<Vector3Int>();
	}

	private void Start()
	{
		SetPattern(_pattern);
	}

	private void OnEnable()
	{
		StartCoroutine(Simulate());
	}

	public void CreateRandomPattern()
	{
		HashSet<Vector2Int> newPattern = new HashSet<Vector2Int>();
		for(int i = 0; i < 1000; ++i)
		{
			newPattern.Add(new Vector2Int(Random.Range(-30, 30), Random.Range(-30, 30)));
		}

		Pattern pattern = Pattern.CreateInstance<Pattern>();
		pattern.cells = new Vector2Int[newPattern.Count];

		newPattern.CopyTo(pattern.cells);

		SetPattern(pattern);
	}

	private void SetPattern(Pattern pattern)
	{
		Clear();

		Vector2Int center = pattern.GetCenter();

		for(int i = 0; i < pattern.cells.Length; ++i)
		{
			Vector3Int cell = (Vector3Int)(pattern.cells[i] - center);
			_currentState.SetTile(cell, _aliveTile);
			_aliveCells.Add(cell);
		}

		population = _aliveCells.Count;
	}

	private void Clear()
	{
		_currentState.ClearAllTiles();
		_nextState.ClearAllTiles();
		_aliveCells.Clear();
		_cellsToCheck.Clear();

		population = 0;
		iterations = 0;
		time = 0.0f;
	}

	private IEnumerator Simulate()
	{
		var interval = new WaitForSeconds(_updateInterval);
		yield return interval;

		while(enabled)
		{
			UpdateState();

			population = _aliveCells.Count;
			iterations++;
			time += _updateInterval;

			yield return interval;
		}
	}

	private void UpdateState()
	{
		_cellsToCheck.Clear();

		// gather cells to check
		foreach(Vector3Int cell in _aliveCells)
		{
			for(int x = -1; x <= 1; ++x)
			{
				for(int y = -1; y <= 1; ++y)
				{
					_cellsToCheck.Add(cell + new Vector3Int(x, y));
				}
			}
		}

		// transitioning cells to the next state
		foreach(Vector3Int cell in _cellsToCheck)
		{
			int neighbors = CountNeighbors(cell);
			bool alive = IsAlive(cell);

			if(!alive && neighbors == _neighborsToReproduce)
			{
				// becomes alive
				_nextState.SetTile(cell, _aliveTile);
				_aliveCells.Add(cell);
			}
			else if(alive && (neighbors <= _neighborsToUnderpopulate || neighbors >= _neighborsToOverpopulate))
			{
				// becomes dead
				_nextState.SetTile(cell, null);
				_aliveCells.Remove(cell);
			}
			else
			{
				// stays the same
				_nextState.SetTile(cell, _currentState.GetTile(cell));
			}
		}

		Tilemap temp = _currentState;
		_currentState = _nextState;
		_nextState = temp;
		_nextState.ClearAllTiles();
	}

	private int CountNeighbors(Vector3Int cell)
	{
		int count = 0;

		for(int x = -1; x <= 1; ++x)
		{
			for(int y = -1; y <= 1; ++y)
			{
				if(x == 0 && y == 0)
					continue;

				Vector3Int neighbor = cell + new Vector3Int(x, y);

				if(IsAlive(neighbor))
				{
					count++;
				}
			}
		}

		return count;
	}

	private bool IsAlive(Vector3Int cell)
	{
		return _currentState.HasTile(cell);
	}
}
