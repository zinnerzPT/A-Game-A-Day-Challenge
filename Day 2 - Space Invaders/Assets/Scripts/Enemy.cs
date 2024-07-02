using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] Transform _firePos;

	[SerializeField] Laser _laserPrefab;

	[SerializeField] float _xLimit = 6.0f;

	private GameManager _gameManager;

	void Start()
	{

	}

	void Update()
	{

	}

	public void Move(Vector3 movement)
	{
		transform.position += movement * Time.deltaTime;
		// TODO Need to limit movement

		if(Mathf.Abs(transform.position.x) > _xLimit)
		{
			transform.position = new Vector2(_xLimit * Mathf.Sign(transform.position.x), transform.position.y);
			_gameManager.EnemyReachedBorder(true, Mathf.Sign(transform.position.x));
		}
	}

	public void Fire()
	{
		// Instantiate bullet

		Instantiate(_laserPrefab, _firePos.position, Quaternion.Euler(0.0f, 0.0f, 180.0f));
	}

	public void Init(GameManager gameManager)
	{
		_gameManager = gameManager;
	}

	private void OnDestroy()
	{
		_gameManager.RemoveEnemy(this);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.layer == 6)
			Destroy(collision.gameObject);
	}
}
