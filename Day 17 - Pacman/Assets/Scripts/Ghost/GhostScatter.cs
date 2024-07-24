using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScatter : GhostBehaviour
{
	private void OnDisable()
	{
		_ghost.chase.Enable();
	}

	private void OnTriggerEnter(Collider other)
	{
		if(enabled && 
			//!_ghost.frightened.enabled && 
			other.gameObject.CompareTag("Node"))
		{
			Node node = other.GetComponent<Node>();

			int index = Random.Range(0, node.availableDirections.Count);

			if(node.availableDirections[index] == -_ghost.movement.moveDirection && node.availableDirections.Count > 1)
			{
				index++;
				if(index >= node.availableDirections.Count)
					index = 0;
			}

			_ghost.movement.SetDirection(node.availableDirections[index]);
		}
	}
}
