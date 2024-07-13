using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField] float _startingSpeed = 3.0f;

	[SerializeField] float _limitYPos = 7f;

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
		_rigidbody2D.velocity = new Vector2(Random.Range(-1.0f, 1.0f), 1.0f).normalized * _startingSpeed;
	}

	// Update is called once per frame
	void Update()
	{
		if(transform.position.y < -_limitYPos)
		{
			_gameManager.LoseBall();
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Bounce
		_rigidbody2D.velocity = Vector3.Reflect(_rigidbody2D.velocity, collision.GetContact(0).normal);
		_soundManager.PlayBounceSound();

		if(collision.gameObject.tag == "Brick")
		{
			_gameManager.HitBrick(collision.gameObject);
			_rigidbody2D.velocity *= 1.05f;
		}
	}
}
