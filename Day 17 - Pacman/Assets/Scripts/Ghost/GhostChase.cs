using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChase : GhostBehaviour
{
	private void OnDisable()
	{
		_ghost.scatter.Enable();
	}

	private void OnTriggerEnter(Collider other)
	{
		if(enabled &&
			//!_ghost.frightened.enabled && 
			other.gameObject.CompareTag("Node"))
		{
			Node node = other.GetComponent<Node>();

			Vector3 direction = Vector3.zero;
			float minDistance = float.MaxValue;

			foreach(Vector3 availableDirection in node.availableDirections)
			{
				Vector3 newPosition = transform.position + availableDirection;
				float newPosDistance = (_ghost.pacman.transform.position - newPosition).sqrMagnitude;

				if(newPosDistance < minDistance)
				{
					direction = availableDirection;
					minDistance = newPosDistance;
				}
			}

			_ghost.movement.SetDirection(direction);
		}
	}
}
