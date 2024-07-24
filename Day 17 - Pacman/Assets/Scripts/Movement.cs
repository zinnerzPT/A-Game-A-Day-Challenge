using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
	[SerializeField] private float _speed = 8.0f;
	[SerializeField] public float speedMultiplier = 1.0f;
	[SerializeField] private Vector3 _initialDirection;
	[SerializeField] private LayerMask _obstacleLayer;

	[HideInInspector] public new Rigidbody rigidbody { get; private set; }

	public Vector3 moveDirection { get; private set; }
	private Vector3 _nextDirection;

	private Vector3 _startingPosition;


	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		_startingPosition = transform.position;
	}

	private void Start()
	{
		ResetState();
	}

	private void Update()
	{
		if(_nextDirection != Vector3.zero)
		{
			SetDirection(_nextDirection);
		}

		HandlePassages();
	}

	private void FixedUpdate()
	{
		Vector3 position = rigidbody.position;
		Vector3 translation = moveDirection * _speed * speedMultiplier * Time.fixedDeltaTime;

		rigidbody.MovePosition(position + translation);
	}

	public void ResetState()
	{
		speedMultiplier = 1.0f;
		moveDirection = _initialDirection;
		_nextDirection = Vector3.zero;
		transform.position = _startingPosition;
		rigidbody.isKinematic = false;
		enabled = true;

		transform.LookAt(transform.position + moveDirection);
	}

	public void SetDirection(Vector3 direction, bool forced = false)
	{
		if(forced || !Occupied(direction))
		{
			moveDirection = direction;
			_nextDirection = Vector3.zero;

			transform.LookAt(transform.position + moveDirection);
		}
		else
		{
			_nextDirection = direction;
		}
	}

	public bool Occupied(Vector3 direction)
	{
		return Physics.BoxCast(transform.position, Vector3.one * .7f, direction, Quaternion.identity, 1.5f, _obstacleLayer);
	}

	private void HandlePassages()
	{
		if(transform.position.x >= 13.5)
			transform.position = new Vector3(transform.position.x - 27.0f, transform.position.y, transform.position.z);
		else if(transform.position.x <= -13.5)
			transform.position = new Vector3(transform.position.x + 27.0f, transform.position.y, transform.position.z);
	}
}
