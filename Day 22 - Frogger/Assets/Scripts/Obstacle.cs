using UnityEngine;

public class Obstacle : MonoBehaviour
{
	private Vector2 _moveDirection;
	private float _moveSpeed;

	private Rigidbody2D _rigidbody;

	private void Update()
	{
		if(Mathf.Abs(transform.position.x) > 8.5f)
			Destroy(gameObject);
	}

	public void SetMoveDirection(Vector2 moveDirection)
	{
		if(_rigidbody == null)
			_rigidbody = GetComponent<Rigidbody2D>();

		_moveDirection = moveDirection;
	}

	public void SetMoveSpeed(float moveSpeed)
	{
		if(_rigidbody == null)
			_rigidbody = GetComponent<Rigidbody2D>();

		_moveSpeed = moveSpeed;
	}

	public void StartMove()
	{
		_rigidbody.velocity = _moveDirection * _moveSpeed;
	}
}