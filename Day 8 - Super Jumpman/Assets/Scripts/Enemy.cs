using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	//gameObject components
	private SpriteRenderer _spriteRenderer;
	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private Collider2D _collider;

	//movement
	[Header("Movement")]
	[Space]
	[SerializeField] private float _moveSpeed = 3f;

	//ground check
	[Header("Ground / Wall Check")]
	[Space]
	[SerializeField] private Transform _groundCheck;
	[SerializeField] private Transform _groundCheckAhead;
	[SerializeField] private Transform _wallCheck;
	[SerializeField] private LayerMask _groundLayerMask;


	[SerializeField] private Transform _eyeLevel;
	[SerializeField] private Transform _footLevel;


	//bool states
	private bool _isGrounded;
	private bool _facingRight;
	private bool _isAlive = true;

	[SerializeField] private bool _goesLeft;

	private float horizontalMove;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_animator = GetComponent<Animator>();
		_collider = GetComponent<Collider2D>();

		if(_goesLeft)
		{
			Flip();
		}
	}

	private void Update()
	{
		//_animator.SetFloat("Horizontal Speed", Mathf.Abs(_rigidbody.velocity.x));
	}

	private void FixedUpdate()
	{
		if(_isAlive)
		{
			if(CheckWalls())
			{
				Flip();
			}
			horizontalMove = _facingRight ? 1f : -1f;
		}
		else
		{
			horizontalMove = 0f;
		}
		_isGrounded = Physics2D.Linecast(transform.position, _groundCheck.position, _groundLayerMask);

		if(_isAlive)
		{

			//Move
			Move();

			//Flip sprite
			if(horizontalMove < 0 && _facingRight)
			{
				Flip();
			}
			else if(horizontalMove > 0 && !_facingRight)
			{
				Flip();
			}
		}
	}

	//Flips the object
	private void Flip()
	{
		_facingRight = !_facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	//Moves
	private void Move()
	{
		_rigidbody.velocity = new Vector2(horizontalMove * _moveSpeed, _rigidbody.velocity.y);
	}

	private bool CheckWalls()
	{
		return !Physics2D.Linecast(_eyeLevel.position, _groundCheckAhead.position, _groundLayerMask) ||
				Physics2D.Linecast(_eyeLevel.position, _wallCheck.position, _groundLayerMask) ||
				Physics2D.Linecast(_footLevel.position, _wallCheck.position, _groundLayerMask);
	}

	public void Die()
	{
		_isAlive = false;
		_animator.SetTrigger("Die");
		_collider.enabled = false;
		_rigidbody.gravityScale = 0f;
		_rigidbody.velocity = Vector2.zero;
	}

	public void SetIsAlive(bool a)
	{
		_isAlive = a;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		//kill player
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Enemy"))
		{
			Flip();
		}
	}
}