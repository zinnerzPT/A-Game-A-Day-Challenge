using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField] float _startingSpeed = 3.0f;

	[SerializeField] float _limitYPos = 7f;

	Rigidbody2D _rigidbody2D;

	GameManager _gameManager;
	public void Init(GameManager gameManager)
	{
		_gameManager = gameManager;
	}

	private void Awake()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		StartMoving();
		_rigidbody2D.velocity = new Vector2(1.0f, 1.0f) * _startingSpeed;
	}

	// Update is called once per frame
	void Update()
	{
		if(transform.position.y < -_limitYPos)
		{
			_gameManager.Lose();
			Destroy(gameObject);

		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Bounce
		_rigidbody2D.velocity = Vector3.Reflect(_rigidbody2D.velocity, collision.GetContact(0).normal);

		if(collision.gameObject.tag == "Brick")
			_gameManager.HitBrick(collision.gameObject);
	}

	public void StartMoving()
	{
		// 		int randX = Random.Range(0, 2) * 2 - 1;
		// 		int randY = Random.Range(0, 2) * 2 - 1;
		// 
		// 		_rigidbody2D.velocity = new Vector2(randX, randY) * _startingSpeed;
	}
}
