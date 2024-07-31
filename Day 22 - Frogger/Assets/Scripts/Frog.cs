using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
	[SerializeField] LayerMask _wallLayer;

	private Vector2 _moveDirection = new Vector2();

	private bool _canMove;

	private Rigidbody2D _rigidbody;

	private GameManager _gameManager;

	private List<Platform> _platforms = new List<Platform>();

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_gameManager = FindObjectOfType<GameManager>();
	}

	private void Update()
	{
		HandleInput();
	}

	private void LateUpdate()
	{
		CheckForWater();
	}

	private void CheckForWater()
	{
		if(transform.position.y > 2.0f && transform.position.y < 7.0f && _platforms.Count == 0)
		{
			Die();
		}
	}

	private void HandleInput()
	{
		if(Input.GetButtonDown("Horizontal"))
		{
			if(Input.GetAxisRaw("Horizontal") > 0.7f)
			{
				_moveDirection = Vector2.right;
			}
			else if(Input.GetAxisRaw("Horizontal") < -0.7f)
			{
				_moveDirection = Vector2.left;
			}
		}
		else if(Input.GetButtonDown("Vertical"))
		{
			if(Input.GetAxisRaw("Vertical") > 0.7f)
			{
				_moveDirection = Vector2.up;
			}
			else if(Input.GetAxisRaw("Vertical") < -0.7f)
			{
				_moveDirection = Vector2.down;
			}
		}

		if(!Input.GetButtonDown("Horizontal") && !Input.GetButtonDown("Vertical"))
		{
			_moveDirection = Vector2.zero;
			_canMove = true;
		}

		Move();
	}

	private void Move()
	{
		if(!_canMove || _moveDirection == Vector2.zero)
			return;

		if(!Physics2D.Raycast(transform.position, _moveDirection, 1.0f, _wallLayer))
		{
			_canMove = false;

			_rigidbody.MovePosition((Vector2)transform.position + _moveDirection);
		}
	}

	public void Die()
	{
		_gameManager.Lose();
		Destroy(gameObject);
	}

	public void AddPlatform(Platform p)
	{
		_platforms.Add(p);
	}

	public void RemovePlatform(Platform p)
	{
		_platforms.Remove(p);
	}
}
