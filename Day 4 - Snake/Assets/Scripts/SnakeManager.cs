using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SnakeManager : MonoBehaviour
{
	[SerializeField] private int _gridWidth = 14;
	[SerializeField] private int _gridHeight = 10;


	[SerializeField] Snake _snakePrefab;
	[SerializeField] GameObject _foodPrefab;

	[SerializeField] TMP_Text _gameOverText;

	private Vector2Int _foodGridPosition;

	Snake _snake;
	GameObject _spawnedFood;

	public int GridWidth
	{
		get => _gridWidth;
	}
	public int GridHeight
	{
		get => _gridHeight;
	}

	private void Start()
	{
		_snake = Instantiate(_snakePrefab, new Vector3(_gridWidth / 3, _gridHeight / 3), Quaternion.identity);
		_snake.Init(this, new Vector2Int(_gridWidth / 3, _gridHeight / 2));

		SpawnFood();
	}

	private void Update()
	{

	}
	private void SpawnFood()
	{
		do
		{
			_foodGridPosition = new Vector2Int(Random.Range(0, _gridWidth), Random.Range(0, _gridHeight));
		} while(_snake.SnakeGridPositions.Contains(_foodGridPosition));

		_spawnedFood = Instantiate(_foodPrefab, new Vector3(_foodGridPosition.x, _foodGridPosition.y), Quaternion.identity);
	}

	public bool CheckFood(Vector2Int snakeGridPosition)
	{
		if(snakeGridPosition == _foodGridPosition)
		{
			Destroy(_spawnedFood);
			SpawnFood();
			return true;
		}
		return false;
	}

	public void GameOver()
	{
		_gameOverText.gameObject.SetActive(true);
	}


	public void PlayAgain()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
