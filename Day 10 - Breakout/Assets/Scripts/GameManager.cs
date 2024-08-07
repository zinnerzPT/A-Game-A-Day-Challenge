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

	[SerializeField] int _extraBalls = 2;

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
			Color color = Color.white;
			switch(y)
			{
				case 0:
					color = new Color(0.0f, 1.0f, 0.0f);
					break;
				case 1:
					color = new Color(0.5f, 1.0f, 0.0f);
					break;
				case 2:
					color = new Color(1.0f, 1.0f, 0.0f);
					break;
				case 3:
					color = new Color(1.0f, 0.5f, 0.0f);
					break;
				case 4:
					color = new Color(1.0f, 0.0f, 0.0f);
					break;
			}
			for(int x = 0; x < 11; ++x)
			{
				GameObject brick = Instantiate(_brickPrefab, new Vector3(x * 1.5f, y * .5f) + _bricksOffset, Quaternion.identity);
				brick.GetComponent<SpriteRenderer>().color = color;
				_bricks.Add(brick);
			}
		}
	}

	private void SpawnBall()
	{
		Ball ball = Instantiate(_ballPrefab, _player.transform.position + Vector3.up * .5f, Quaternion.identity);

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

	public void LoseBall()
	{
		if(_extraBalls <= 0)
		{
			Lose();
			return;
		}

		_extraBalls--;
		SpawnBall();
	}

	public void Win()
	{
		_gameOverText.gameObject.SetActive(true);
		_gameOverText.text = "You won!\nExtra Balls Left: " + _extraBalls.ToString();

		Destroy(FindObjectOfType<Ball>().gameObject);
	}

	public void Lose()
	{
		_gameOverText.gameObject.SetActive(true);
		_gameOverText.text = "Game Over";
	}

	public void BackToMenu()
	{
		SceneManager.LoadScene(1);
	}
}
