using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] float _movementSpeed = 3.0f;
	[SerializeField] float _jumpVelocity = 3.0f;

	[Header("Better Jump")]
	[SerializeField] float _fallMultiplier = 2.5f;
	[SerializeField] float _lowJumpMultiplier = 2.0f;

	[Header("Ground Check")]
	[SerializeField] private LayerMask _groundLayer;
	[SerializeField] private Vector2 _groundCheckBoxSize;
	[SerializeField] private float _groundCheckCastDistance;

	private Rigidbody2D _rigidbody2d;
	private Animator _animator;


	private bool facingLeft = true;


	private void Awake()
	{
		_rigidbody2d = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}

	void Start()
	{
		Flip();
	}


	void Update()
	{
		HandleInput();
	}


	private void HandleInput()
	{
		float moveInput = Input.GetAxisRaw("Horizontal");

		Move(moveInput);
	}

	private void Move(float moveInput)
	{
		Vector2 inputVelocity = new Vector2(moveInput * _movementSpeed, _rigidbody2d.velocity.y);

		//Flip sprite
		if(moveInput > 0 && facingLeft)
		{
			Flip();
		}
		else if(moveInput < 0 && !facingLeft)
		{
			Flip();
		}

		_animator.SetBool("Is Running", Mathf.Abs(moveInput) > 0);

		_animator.SetBool("Is Grounded", IsGrounded());

		if(Input.GetButtonDown("Jump") && IsGrounded())
		{
			inputVelocity.y = _jumpVelocity;
		}

		_rigidbody2d.velocity = inputVelocity;

		// Better Jump
		if(_rigidbody2d.velocity.y < 0)
		{
			_rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
		}
		else if(_rigidbody2d.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			_rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	public bool IsGrounded()
	{
		if(Physics2D.BoxCast(transform.position, _groundCheckBoxSize, 0.0f, -transform.up, _groundCheckCastDistance, _groundLayer))
		{
			return true;
		}
		return false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position - transform.up * _groundCheckCastDistance, _groundCheckBoxSize);
	}

	private void Flip()
	{
		facingLeft = !facingLeft;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

}
