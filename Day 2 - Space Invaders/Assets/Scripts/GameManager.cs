using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] TMP_Text _gameOverText;

	[Space]
	[SerializeField] Enemy _enemy1Prefab;
	[SerializeField] Enemy _enemy2Prefab;
	[SerializeField] Enemy _enemy3Prefab;

	[SerializeField] Vector2 _enemyOffset = new Vector2(-4.0f, 0.0f);

	[SerializeField] float _enemyMovementSpeed = 1.0f;

	List<Enemy> _enemies = new List<Enemy>();

	Vector3 _enemyMovementDirection = Vector2.left;
	Vector3 _nextMovementDirection;

	bool _hasEnemyReachedBorder;

	bool _enemiesMovingDown;
	float _distanceMovedDown;

	void Start()
	{
		SpawnEnemies();
		StartCoroutine(RandomEnemyFire());
	}

	void Update()
	{
		HandleEnemyMovement();
	}

	private void SpawnEnemies()
	{
		for(int i = 0; i < 5; ++i)
		{
			for(int j = 0; j < 9; ++j)
			{
				Enemy enemy;
				if(i < 2)
					enemy = Instantiate(_enemy1Prefab, new Vector2(j, i) + _enemyOffset, Quaternion.identity);
				else if(i < 4)
					enemy = Instantiate(_enemy2Prefab, new Vector2(j, i) + _enemyOffset, Quaternion.identity);
				else
					enemy = Instantiate(_enemy3Prefab, new Vector2(j, i) + _enemyOffset, Quaternion.identity);
				enemy.Init(this);
				_enemies.Add(enemy);
			}
		}
	}

	private void HandleEnemyMovement()
	{
		if(_enemies.Count < 1)
			return;

		Vector3 pos1 = _enemies[0].transform.position;
		// Move Enemies
		foreach(Enemy e in _enemies)
		{
			e.Move(_enemyMovementDirection * _enemyMovementSpeed);
		}

		Vector3 movementVector = pos1 - _enemies[0].transform.position;

		if(_enemiesMovingDown)
		{
			_distanceMovedDown += movementVector.magnitude;

			if(_distanceMovedDown >= 0.5f)
			{
				_enemiesMovingDown = false;
				_enemyMovementSpeed *= 1.1f;
				_enemyMovementDirection = _nextMovementDirection;
			}
		}

		if(_hasEnemyReachedBorder)
		{
			_hasEnemyReachedBorder = false;
			_enemyMovementDirection = Vector3.down;
			_enemiesMovingDown = true;
			_distanceMovedDown = 0.0f;
		}
	}

	public void RemoveEnemy(Enemy enemy)
	{
		_enemies.Remove(enemy);
		if(_enemies.Count < 1)
			Win();
	}

	public void EnemyReachedBorder(bool hasEnemyReachedBorder, float sign)
	{
		_hasEnemyReachedBorder = hasEnemyReachedBorder;
		_nextMovementDirection = sign * Vector3.left;
	}

	private IEnumerator RandomEnemyFire()
	{
		while(_enemies.Count > 0)
		{
			_enemies[UnityEngine.Random.Range(0, _enemies.Count)].Fire();
			yield return new WaitForSeconds(1.0f);
		}
	}

	private void Win()
	{
		if(_gameOverText == null) // Hack to prevent error on level restart
			return;

		_gameOverText.gameObject.SetActive(true);
		_gameOverText.text = "You Won!";
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
