using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	[SerializeField] float _jumpVelocity = 3.0f;

	[Header("Better Jump")]
	[SerializeField] float _fallMultiplier = 2.5f;
	[SerializeField] float _lowJumpMultiplier = 2.0f;

	[Header("Ground Check")]
	[SerializeField] private LayerMask _groundLayer;
	[SerializeField] private Vector2 _groundCheckBoxSize;
	[SerializeField] private float _groundCheckCastDistance;

	private Rigidbody2D _rigidbody;
	private Animator _animator;

	private GameManager _gameManager;
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponentInChildren<Animator>();
		_gameManager = FindObjectOfType<GameManager>();
	}

	void Update()
	{
		if(_gameManager.isGameOver)
			return;

		HandleInput();
	}

	private void HandleInput()
	{
		_animator.SetBool("Grounded", IsGrounded());

		if(Input.GetButtonDown("Jump") && IsGrounded())
		{
			if(!_gameManager.isRunning)
				_gameManager.StartGame();
			_rigidbody.velocity = new Vector2(0.0f, _jumpVelocity);
		}

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
		_rigidbody.constraints = RigidbodyConstraints2D.None;
		GetComponent<CapsuleCollider2D>().enabled = false;
		_rigidbody.velocity = new Vector2(Random.Range(-3.0f, 3.0f), 3.0f);
		_rigidbody.AddTorque(-360.0f);
	}
}
