using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	[SerializeField] private LayerMask _obstacleLayer;

	public List<Vector3> availableDirections { get; private set; }

	private void Start()
	{
		availableDirections = new List<Vector3>();

		CheckAvailableDirection(Vector3.right);
		CheckAvailableDirection(Vector3.left);
		CheckAvailableDirection(Vector3.forward);
		CheckAvailableDirection(Vector3.back);
	}

	private void CheckAvailableDirection(Vector3 direction)
	{
		if(!Physics.BoxCast(transform.position, Vector3.one * .5f, direction, Quaternion.identity, 1.5f, _obstacleLayer))
		{
			availableDirections.Add(direction);
		}
	}
}
