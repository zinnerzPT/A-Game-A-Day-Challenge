using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerMushroom : PowerUp
{
	[SerializeField] float _moveSpeed = 3.0f;

	Vector3 _movingDirection = Vector3.right;

	Rigidbody2D _rigidbody;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		_rigidbody.velocity = _movingDirection * _moveSpeed;
	}

	private void Update()
	{
		if(_rigidbody.velocity.magnitude <= 0.1f)
		{
			_movingDirection = -_movingDirection;
			_rigidbody.velocity = _movingDirection * _moveSpeed;
		}
	}

	protected override void Activate(Player player)
	{
		player.ActivatePowerMushroom();
		Destroy(gameObject);
	}
}
