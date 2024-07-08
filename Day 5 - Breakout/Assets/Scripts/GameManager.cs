using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] Paddle _playerPrefab;
	[SerializeField] Ball _ballPrefab;
	[SerializeField] GameObject _brickPrefab;

	[SerializeField] Vector3 _bricksOffset = new Vector3(-7.5f, 3.0f, 0.0f);

	[SerializeField] TMP_Text _gameOverText;

	List<GameObject> _bricks = new List<GameObject>();
	Paddle _player;

	void Start()
	{
		SpawnPlayer();
		SpawnBricks();
		SpawnBall();
	}

	private void SpawnPlayer()
	{
		_player = Instantiate(_playerPrefab, new Vector3(0.0f, -6.0f, 0.0f), Quaternion.identity);
	}

	public void SpawnBricks()
	{
		for(int y = 0; y < 5; ++y)
		{
			for(int x = 0; x < 11; ++x)
			{
				_bricks.Add(Instantiate(_brickPrefab, new Vector3(x * 1.5f, y * .5f) + _bricksOffset, Quaternion.identity));
			}
		}
	}

	private void SpawnBall()
	{
		Ball ball = Instantiate(_ballPrefab, _player.transform.position + Vector3.up, Quaternion.identity);

		ball.Init(this);
	}

	public void HitBrick(GameObject brick)
	{
		_bricks.Remove(brick);
		Destroy(brick);

		if(_bricks.Count == 0)
		{
			Win();
		}
	}

	public void Win()
	{
		_gameOverText.gameObject.SetActive(true);
		_gameOverText.text = "You won!";
	}

	public void Lose()
	{
		_gameOverText.gameObject.SetActive(true);
		_gameOverText.text = "Game Over";
	}

	public void PlayAgain()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
