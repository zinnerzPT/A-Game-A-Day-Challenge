using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
	[SerializeField] float _minSpawningDelay = 3.0f;
	[SerializeField] float _maxSpawningDelay = 5.0f;

	[SerializeField] Obstacle[] _obstaclePrefabs;

	[SerializeField] Vector2 _obstacleMoveDirection;
	[SerializeField] float _obstacleMoveSpeed;

	private float _timeSinceSpawn;
	private float _spawningDelay;

	void Start()
	{
		// Start the game with one already spawned
		_spawningDelay = Random.Range(_minSpawningDelay, _maxSpawningDelay);
		int rand = Random.Range(0, _obstaclePrefabs.Length);
		Obstacle obstacle = Instantiate(_obstaclePrefabs[rand], transform.position + (Vector3)_obstacleMoveDirection * _spawningDelay, Quaternion.identity);
		obstacle.SetMoveDirection(_obstacleMoveDirection);
		obstacle.SetMoveSpeed(_obstacleMoveSpeed);
		obstacle.StartMove();

		// First spawn should be faster
		_spawningDelay = Random.Range(0.0f, _minSpawningDelay);
	}

	void Update()
	{
		_timeSinceSpawn += Time.deltaTime;

		if(_timeSinceSpawn >= _spawningDelay)
		{
			SpawnObstacle();
			_timeSinceSpawn -= _spawningDelay;
			_spawningDelay = Random.Range(_minSpawningDelay, _maxSpawningDelay);
		}
	}

	private void SpawnObstacle()
	{
		int rand = Random.Range(0, _obstaclePrefabs.Length);
		Obstacle obstacle = Instantiate(_obstaclePrefabs[rand], transform.position, Quaternion.identity);
		obstacle.SetMoveDirection(_obstacleMoveDirection);
		obstacle.SetMoveSpeed(_obstacleMoveSpeed);
		obstacle.StartMove();
	}
}
