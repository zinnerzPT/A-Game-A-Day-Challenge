using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField] float _maxPower = 10.0f;
	[SerializeField] float _power = 2.0f;
	[SerializeField] float _minSpeed = 0.1f;

	private Rigidbody2D _rigidbody;
	private LineRenderer _lineRenderer;

	bool _isDragging;
	bool _inHole;

	LevelManager _levelManager;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_lineRenderer = GetComponent<LineRenderer>();

		_levelManager = FindObjectOfType<LevelManager>();
	}

	void Update()
	{
		HandleInput();
	}

	private void HandleInput()
	{
		if(!IsReady())
			return;

		Vector2 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		float distance = Vector2.Distance(transform.position, inputPos);

		if(Input.GetMouseButtonDown(0) && distance <= 0.5f)
		{
			DragStart();
		}

		if(Input.GetMouseButton(0) && _isDragging)
		{
			DragChange(inputPos);
		}

		if(Input.GetMouseButtonUp(0) && _isDragging)
		{
			DragRelease(inputPos);
		}
	}

	private void DragStart()
	{
		_isDragging = true;

		_lineRenderer.positionCount = 2;
	}

	private void DragChange(Vector2 pos)
	{
		Vector2 dir = (Vector2)transform.position - pos;

		_lineRenderer.SetPosition(0, transform.position);
		_lineRenderer.SetPosition(1, (Vector2)transform.position - Vector2.ClampMagnitude((dir * _power) / 4.0f, _maxPower / 4.0f));
	}

	private void DragRelease(Vector2 pos)
	{
		float distance = Vector2.Distance((Vector2)transform.position, pos);
		_isDragging = false;
		_lineRenderer.positionCount = 0;

		if(distance < 0.2f)
			return;

		_levelManager.IncrementStroke();

		Vector2 dir = (Vector2)transform.position - pos;

		_rigidbody.velocity = Vector2.ClampMagnitude(dir * _power, _maxPower);
	}

	private bool IsReady()
	{
		return _rigidbody.velocity.magnitude <= _minSpeed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Hole"))
			CheckWinState();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if(collision.CompareTag("Hole"))
			CheckWinState();
	}

	private void CheckWinState()
	{
		if(_inHole)
			return;

		if(_rigidbody.velocity.magnitude <= 1.0f)
		{
			_inHole = true;

			gameObject.SetActive(false);

			_levelManager.LevelCompleted();
		}
	}
}
