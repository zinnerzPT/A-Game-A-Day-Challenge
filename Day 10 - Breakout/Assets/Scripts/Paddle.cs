using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
	[SerializeField] float _limitXPos = 3.0f;
	[SerializeField] float _speed = 1.0f;

	void Update()
	{
		HandleInput();
	}

	void HandleInput()
	{
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), 0.0f);

		Move(input);
	}

	public void Move(Vector2 direction)
	{
		transform.position = (Vector2)transform.position + direction * _speed * Time.deltaTime;

		if(transform.position.x > _limitXPos)
		{
			transform.position = new Vector2(_limitXPos, transform.position.y);
		}
		else if(transform.position.x < -_limitXPos)
		{
			transform.position = new Vector2(-_limitXPos, transform.position.y);
		}
	}
}
