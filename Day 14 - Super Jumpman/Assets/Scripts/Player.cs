using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	[SerializeField] float _movementSpeed = 3.0f;
	[SerializeField] float _jumpVelocity = 3.0f;

	[SerializeField] private float _bounceSpeed = 4f;
	[SerializeField] [Range(0.0f, 1.0f)] private float _bounceNormal = 0.8f;

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

	private Strength _strength = Strength.Weak;

	bool _isLarge = false;

	private SpriteRenderer _spriteRenderer;

	float _invincibility = 0.0f;


	SoundManager _soundManager;
	private void Awake()
	{
		_rigidbody2d = GetComponent<Rigidbody2D>();
		_animator = GetComponentInChildren<Animator>();
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		_soundManager = FindObjectOfType<SoundManager>();
	}

	void Start()
	{
		Flip();
	}

	void Update()
	{
		if(_invincibility > 0.0f)
			_invincibility -= Time.deltaTime;
		else
			_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1.0f);
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
			_soundManager.PlayPlayerJumpSound();
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
		if(_isLarge)
			LosePowerMushroom();
		else
		{
			_soundManager.PlayPlayerDeathSound();
			//isAlive = false;
			//myAnimator.SetTrigger("Die");

			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Enemy"))
		{
			Enemy enemy = collision.collider.GetComponent<Enemy>();
			if(enemy != null)
			{
				if(collision.contacts[0].normal.y >= _bounceNormal)
				{
					//Bounce on enemy
					_rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, _bounceSpeed);
					enemy.Die();
				}
				else if(_invincibility <= 0.0f)
				{
					enemy.Die();
					Die();
				}
			}
		}
		else if(collision.gameObject.CompareTag("Block"))
		{
			Block block = collision.collider.GetComponent<Block>();
			if(block != null)
			{
				foreach(ContactPoint2D point in collision.contacts)
				{
					if(point.normal.y <= -_bounceNormal)
					{
						block.Activate(_strength);
					}
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Flag"))
		{
			_soundManager.PlayPowerUpSound();
			SceneManager.LoadSceneAsync(1);
		}
	}

	// 	public void SetIsAlive(bool a)
	// 	{
	// 		isAlive = a;
	// 	}


	public void ActivatePowerMushroom()
	{
		_spriteRenderer.transform.localScale = new Vector3(1.0f, 2.0f, 1.0f);
		_isLarge = true;
		_strength = Strength.Strong;

		_soundManager.PlayPowerUpSound();
	}

	public void LosePowerMushroom()
	{
		_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.5f);
		_invincibility = 0.5f;
		_spriteRenderer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		_isLarge = false;
		_strength = Strength.Weak;

		_soundManager.PlayPowerDownSound();
	}

	// 	public void ActivateFireFlower()
	// 	{
	// 		_spriteRenderer.color = Color.red;
	// 	}
	// 
	// 	private void LoseFireFLower()
	// 	{
	// 
	// 	}

}