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


	private bool _facingLeft = true;




	//movement
	[Header("Movement")]
	[Space]

	[SerializeField]
	private float _bounceSpeed = 4f;
	[SerializeField]
	[Range(0.0f, 1.0f)]
	private float _bounceNormal = 0.9f;

	//ground check
	[Header("Ground Check")]
	[Space]
	[SerializeField]
	private Transform groundCheck;
	private int groundLayerMask;

	//bool states
	[SerializeField]
	private bool isDark;
	private bool isGrounded;
	private bool jump;
	private bool isAlive;

	private float horizontalMove;


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
		if(moveInput > 0 && _facingLeft)
		{
			Flip();
		}
		else if(moveInput < 0 && !_facingLeft)
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
		_facingLeft = !_facingLeft;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	public void Die()
	{
		isAlive = false;
		//myAnimator.SetTrigger("Die");
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Enemy enemy = collision.collider.GetComponent<Enemy>();
		if(enemy != null)
		{
			foreach(ContactPoint2D point in collision.contacts)
			{
				if(point.normal.y >= _bounceNormal)
				{
					//Bounce on enemy
					_rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, _bounceSpeed);
					enemy.Die();
				}
				else
				{
					Die();
				}
			}
		}
	}

	public void SetIsAlive(bool a)
	{
		isAlive = a;
	}


}