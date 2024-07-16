using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] float _moveSpeed = 3.0f;
	[SerializeField] float _jumpVelocity = 3.0f;

	[Header("Better Jump")]
	[SerializeField] float _fallMultiplier = 2.5f;
	[SerializeField] float _lowJumpMultiplier = 2.0f;

	[Header("Ground Check")]
	[SerializeField] private LayerMask _groundLayer;
	[SerializeField] private Vector2 _groundCheckBoxSize;
	[SerializeField] private float _groundCheckCastDistance;

	[Header("Wall Check")]
	[SerializeField] private Vector2 _wallCheckBoxSize;
	[SerializeField] private float _wallCheckCastDistance;

	//gameObject components
	private Rigidbody2D _rigidbody;

	//bool states
	private bool _facingRight;
	private bool _isAlive = true;

	private float horizontalMove;

	private float _jumpBufferTime = 0.2f;
	private float _jumpBufferCounter;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		//Flip();
	}

	private void Update()
	{
		//_animator.SetFloat("Horizontal Speed", Mathf.Abs(_rigidbody.velocity.x));
		HandleInput();
		Move();
	}

	private void HandleInput()
	{
		//float moveInput = Input.GetAxisRaw("Horizontal");

		// Jump Buffering
		if(Input.GetButtonDown("Jump"))
		{
			_jumpBufferCounter = _jumpBufferTime;
		}
		else
		{
			_jumpBufferCounter -= Time.deltaTime;
		}
	}

	private void Move()
	{
		if(!_isAlive)
			return;

		if(CheckWalls())
		{
			Flip();
		}
		horizontalMove = _facingRight ? 1f : -1f;

		//Flip sprite
		if(horizontalMove < 0 && _facingRight)
		{
			Flip();
		}
		else if(horizontalMove > 0 && !_facingRight)
		{
			Flip();
		}

		//_animator.SetBool("Is Running", Mathf.Abs(horizontalMove) > 0);

		//_animator.SetBool("Is Grounded", IsGrounded());

		Vector2 inputVelocity = new Vector2(horizontalMove * _moveSpeed, _rigidbody.velocity.y);

		// Jump
		if(_jumpBufferCounter > 0.0f && IsGrounded())
		{
			inputVelocity.y = _jumpVelocity;
			_jumpBufferCounter = 0.0f;
		}

		_rigidbody.velocity = inputVelocity;

		// Better Jump
		if(_rigidbody.velocity.y < 0)
		{
			_rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
		}
		else if(_rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			_rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	public bool IsGrounded()
	{
		return Physics2D.BoxCast(transform.position, _groundCheckBoxSize, 0.0f, -transform.up, _groundCheckCastDistance, _groundLayer);
	}
	private bool CheckWalls()
	{
		return Physics2D.BoxCast(transform.position, _wallCheckBoxSize, 0.0f, _facingRight ? transform.right : -transform.right, _wallCheckCastDistance, _groundLayer);
	}

	private void Flip()
	{
		_facingRight = !_facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	public void Die()
	{
		_isAlive = false;
		//myAnimator.SetTrigger("Die");
	}

	// 	public void SetIsAlive(bool a)
	// 	{
	// 		_isAlive = a;
	// 	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position - transform.up * _groundCheckCastDistance, _groundCheckBoxSize);
		Gizmos.DrawWireCube(transform.position + (_facingRight ? transform.right : -transform.right) * _wallCheckCastDistance, _wallCheckBoxSize);
	}
#endif
}