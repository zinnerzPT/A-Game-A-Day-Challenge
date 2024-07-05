using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
	[SerializeField] float _timeToMove = 0.5f;

	[SerializeField] Transform _snakeTailPrefab;

	SnakeManager _snakeManager;

	private Vector2 _storedInput = Vector2.right;
	private Vector2 _movingDirection = Vector2.right;

	Vector2Int _gridPosition;

	float _moveTimer;

	int _tailSize = 0;

	List<Vector2Int> _snakeGridPositions = new List<Vector2Int>();
	List<Transform> _tailTransforms = new List<Transform>();

	bool isAlive = true;

	public Vector2Int GridPosition
	{
		get => _gridPosition;
	}

	public List<Vector2Int> SnakeGridPositions
	{
		get => _snakeGridPositions;
	}

	public void Init(SnakeManager snakeManager, Vector2Int gridPosition)
	{
		_snakeManager = snakeManager;

		_gridPosition = gridPosition;

		transform.position = new Vector3(_gridPosition.x, _gridPosition.y, 0.0f);
	}

	private void Awake()
	{


	}

	void Start()
	{

	}

	void Update()
	{
		if(!isAlive)
			return;

		HandleInput();
		HandleMovement();
	}

	private void HandleInput()
	{
		// TODO Improve
		if(Input.GetButtonDown("Up") && _movingDirection != -Vector2.up)
		{
			_storedInput = Vector2.up;
		}
		if(Input.GetButtonDown("Down") && _movingDirection != -Vector2.down)
		{
			_storedInput = Vector2.down;
		}
		if(Input.GetButtonDown("Left") && _movingDirection != -Vector2.left)
		{
			_storedInput = Vector2.left;
		}
		if(Input.GetButtonDown("Right") && _movingDirection != -Vector2.right)
		{
			_storedInput = Vector2.right;
		}
	}

	private void HandleMovement()
	{
		_moveTimer += Time.deltaTime;
		if(_moveTimer >= _timeToMove)
		{
			_gridPosition += Vector2Int.RoundToInt(_storedInput);

			if(_gridPosition.x >= _snakeManager.GridWidth || _gridPosition.y >= _snakeManager.GridHeight || _gridPosition.x < 0 || _gridPosition.y < 0)
			{
				_snakeManager.GameOver();
				isAlive = false;
				return;
			}

			_movingDirection = _storedInput;
			_moveTimer -= _timeToMove;

			_snakeGridPositions.Insert(0, _gridPosition);

			if(_snakeManager.CheckFood(_gridPosition))
			{
				_tailSize++;
				CreateSnakeTail();
			}

			if(_snakeGridPositions.Count > _tailSize + 1)
			{
				_snakeGridPositions.RemoveAt(_snakeGridPositions.Count - 1);
			}

			// Collision with itself
			for(int i = 1; i < _snakeGridPositions.Count; ++i)
			{
				if(_gridPosition == _snakeGridPositions[i])
				{
					_snakeManager.GameOver();
					isAlive = false;
					return;
				}
			}

			// Only move if you didn't lose
			transform.position = new Vector3(_gridPosition.x, _gridPosition.y, 0.0f);

			// Tail follows head
			for(int i = 0; i < _tailTransforms.Count; ++i)
			{
				Vector3 pos = new Vector3(_snakeGridPositions[i + 1].x, _snakeGridPositions[i + 1].y);
				_tailTransforms[i].position = pos;
			}
		}
	}

	private void CreateSnakeTail()
	{
		Transform tail = Instantiate(_snakeTailPrefab, Vector3.zero, Quaternion.identity);
		_tailTransforms.Add(tail);
	}
}
