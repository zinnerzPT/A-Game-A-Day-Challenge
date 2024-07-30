using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("Game Settings")]
	[SerializeField] float _startingSpeed = 2.0f;
	[SerializeField] float _maxSpeed = 10.0f;
	[SerializeField] float _distanceBetweenObstacles = 10.0f;

	[Header("Game Components")]
	[SerializeField] Scrolling[] _floor;
	[SerializeField] Scrolling[] _background;

	[SerializeField] Obstacle[] _obstaclePrefabs;

	[Header("UI Elements")]
	[SerializeField] GameObject _startUI;

	[SerializeField] GameObject _gameHUD;
	[SerializeField] TMP_Text _scoreText;
	[SerializeField] TMP_Text _highscoreText;

	[SerializeField] GameObject _gameOverPanel;
	[SerializeField] TMP_Text _gameOverScoreText;
	[SerializeField] GameObject _gameOverHighscore;



	private float _score;
	private static float _highscore;

	private float _currentSpeed;
	private float _currentPosition = 0.0f;

	private List<Obstacle> _obstacles = new List<Obstacle>();

	public bool isRunning { get; private set; }
	public bool isGameOver { get; private set; }

	private void Awake()
	{
		isRunning = false;
	}

	private void Start()
	{
		_currentSpeed = _startingSpeed;
		UpdateSpeed();
		UpdateScoreUI();
	}

	private void Update()
	{
		if(!isRunning)
			return;

		_currentPosition += _currentSpeed * Time.deltaTime;

		_score += Time.deltaTime * _currentSpeed * .5f;
		UpdateScoreUI();

		if(_currentPosition >= _distanceBetweenObstacles)
		{
			// Spawn obstacle
			SpawnObstacle();

			_currentPosition -= _distanceBetweenObstacles;
		}


		if(_currentSpeed < _maxSpeed)
		{
			_currentSpeed += Time.deltaTime * .1f;
			if(_currentSpeed > _maxSpeed)
				_currentSpeed = _maxSpeed;
			UpdateSpeed();
		}
	}



	private void UpdateSpeed()
	{
		foreach(Scrolling floor in _floor)
			floor.SetSpeed(_currentSpeed);

		foreach(Scrolling background in _background)
			background.SetSpeed(_currentSpeed / 2.0f);

		foreach(Obstacle obstacle in _obstacles)
			obstacle.SetSpeed(_currentSpeed);
	}

	public void StartGame()
	{
		isRunning = true;
		_startUI.SetActive(false);
		_gameHUD.SetActive(true);

		_currentPosition = _distanceBetweenObstacles;
	}

	public void GameOver()
	{
		if(_score > _highscore)
		{
			// Highscore popup
			_highscore = _score;
			_gameOverHighscore.SetActive(true);
		}
		isRunning = false;
		isGameOver = true;
		_gameHUD.SetActive(false);
		_gameOverPanel.SetActive(true);
	}

	public void PlayAgain()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void UpdateScoreUI()
	{
		_scoreText.SetText("Score: " + _score.ToString("0"));
		_highscoreText.SetText("Highscore: " + _highscore.ToString("0"));

		_gameOverScoreText.SetText("Score: " + _score.ToString("0"));
	}

	private void SpawnObstacle()
	{
		int rand = Random.Range(0, _obstaclePrefabs.Length);

		Obstacle obstacle = Instantiate(_obstaclePrefabs[rand], new Vector2(10.0f, 0.0f), Quaternion.identity);

		obstacle.SetSpeed(_currentSpeed);
		_obstacles.Add(obstacle);
	}

	public void RemoveObstacle(Obstacle obstacle)
	{
		_obstacles.Remove(obstacle);
	}
}
