using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
	private Movement _movement;

	private void Awake()
	{
		_movement = GetComponent<Movement>();
	}

	void Update()
	{
		HandleInput();
	}

	private void HandleInput()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		if(horizontal > 0.5f)
			_movement.SetDirection(Vector3.right);
		else if(horizontal < -0.5f)
			_movement.SetDirection(Vector3.left);
		else if(vertical > 0.5f)
			_movement.SetDirection(Vector3.forward);
		else if(vertical < -0.5f)
			_movement.SetDirection(Vector3.back);

		//transform.LookAt(transform.position + _movement.moveDirection);
	}



	public void ResetState()
	{
		_movement.ResetState();
		gameObject.SetActive(true);
	}
}
