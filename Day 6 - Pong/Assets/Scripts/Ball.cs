using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField] float _startingSpeed = 3.0f;

	[SerializeField] float _limitYPos = 4.25f;
	[SerializeField] float _limitXPos = 9.0f;

	Rigidbody2D _rigidbody2D;

	GameManager _gameManager;

	SoundManager _soundManager;

	public void Init(GameManager gameManager)
	{
		_gameManager = gameManager;
	}

	private void Awake()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();

		_soundManager = FindObjectOfType<SoundManager>();
	}

	void Start()
	{
		StartMoving();
	}

	void Update()
	{
		if(transform.position.y > _limitYPos)
		{
			transform.position = new Vector2(transform.position.x, _limitYPos);
			_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -_rigidbody2D.velocity.y);
		}
		else if(transform.position.y < -_limitYPos)
		{
			transform.position = new Vector2(transform.position.x, -_limitYPos);
			_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -_rigidbody2D.velocity.y);
		}

		if(transform.position.x > _limitXPos)
		{
			// Player 1 scores
			_gameManager.PlayerScored(PlayerType.Player1);
			Destroy(gameObject);
		}
		else if(transform.position.x < -_limitXPos)
		{
			// Player 2 scores
			_gameManager.PlayerScored(PlayerType.Player2);
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		_rigidbody2D.velocity = new Vector2(-_rigidbody2D.velocity.x, _rigidbody2D.velocity.y) * 1.1f;

		_soundManager.PlayBounceSound();
	}

	public void StartMoving()
	{
		int randX = Random.Range(0, 2) * 2 - 1;
		int randY = Random.Range(0, 2) * 2 - 1;

		_rigidbody2D.velocity = new Vector2(randX, randY) * _startingSpeed;
	}
}
