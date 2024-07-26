using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
	[SerializeField] float _maxAllowedY = 2.15f;
	[SerializeField] float _flapStrength = 2.0f;

	private Rigidbody2D _rigidbody;

	private GameManager _gameManager;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_gameManager = FindObjectOfType<GameManager>();
	}

	void Update()
	{
		if(_gameManager.isGameOver)
			return;

		if(!_gameManager.isRunning && transform.position.y < -0.5f)
		{
			Flap();
		}

		HandleInput();
	}

	private void HandleInput()
	{
		if(Input.GetButtonDown("Jump") && transform.position.y < _maxAllowedY)
		{
			if(!_gameManager.isRunning)
				_gameManager.StartGame();
			Flap();
		}
	}

	private void Flap()
	{
		_rigidbody.velocity = new Vector2(0.0f, _flapStrength);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Obstacle"))
		{
			// Die
			_gameManager.GameOver();

			Die();
		}
	}

	private void Die()
	{
		GetComponent<CircleCollider2D>().enabled = false;
		_rigidbody.velocity = new Vector2(Random.Range(-3.0f, 3.0f), 3.0f);
		_rigidbody.AddTorque(-360.0f);
	}
}
