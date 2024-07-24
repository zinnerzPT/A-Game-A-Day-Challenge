using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHome : GhostBehaviour
{
	[SerializeField] public Transform inside;
	[SerializeField] private Transform _outside;

	private void OnEnable()
	{
		StopAllCoroutines();
	}

	private void OnDisable()
	{
		// Check for active self to prevent error when object is destroyed
		if(gameObject.activeInHierarchy)
		{
			StartCoroutine(ExitTransition());
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		// Reverse direction everytime the ghost hits a wall to create the
		// effect of the ghost bouncing around the home
		if(enabled && collision.gameObject.CompareTag("Obstacle"))
		{
			_ghost.movement.SetDirection(-_ghost.movement.moveDirection);
		}
	}

	private IEnumerator ExitTransition()
	{
		// Turn off movement while we manually animate the position
		_ghost.movement.SetDirection(Vector3.forward, true);
		_ghost.movement.rigidbody.isKinematic = true;
		_ghost.movement.enabled = false;

		Vector3 position = transform.position;

		float duration = 0.5f;
		float elapsed = 0f;

		// Animate to the starting point
		while(elapsed < duration)
		{
			_ghost.transform.position = Vector3.Lerp(position, inside.position, elapsed / duration);
			elapsed += Time.deltaTime;
			yield return null;
		}

		elapsed = 0f;

		// Animate exiting the ghost home
		while(elapsed < duration)
		{
			_ghost.transform.position = Vector3.Lerp(inside.position, _outside.position, elapsed / duration);
			elapsed += Time.deltaTime;
			yield return null;
		}

		// Pick a random direction left or right and re-enable movement
		_ghost.movement.SetDirection(new Vector3(Random.value < 0.5f ? -1.0f : 1.0f, 0.0f, 0.0f), true);
		_ghost.movement.rigidbody.isKinematic = false;
		_ghost.movement.enabled = true;
	}

}
