using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehaviour
{

	[SerializeField] Material _blueMat;
	[SerializeField] Material _whiteMat;

	private MeshRenderer _meshRenderer;

	private Material _regularMat;
	private Material _nextMat;

	private bool eaten;

	private void Awake()
	{
		_meshRenderer = GetComponentInChildren<MeshRenderer>();
		_regularMat = _meshRenderer.material;
	}

	public override void Enable(float duration)
	{
		base.Enable(duration);

		_meshRenderer.material = _blueMat;
		_nextMat = _whiteMat;

		InvokeRepeating(nameof(Flash), duration / 2f, 0.5f);
	}

	public override void Disable()
	{
		base.Disable();

		_meshRenderer.material = _regularMat;
	}

	private void Eaten()
	{
		CancelInvoke();
		eaten = true;
		_ghost.transform.position = _ghost.home.inside.position;
		_ghost.home.Enable(_duration);

		_meshRenderer.material = _regularMat;
	}

	private void Flash()
	{
		if(!eaten)
		{
			Material mat = _meshRenderer.material;
			_meshRenderer.material = _nextMat;
			_nextMat = mat;
		}
	}

	private void OnEnable()
	{
		_ghost.movement.speedMultiplier = 0.5f;
		eaten = false;
	}

	private void OnDisable()
	{
		_ghost.movement.speedMultiplier = 1f;
		_meshRenderer.material = _regularMat;
		eaten = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(enabled && other.gameObject.CompareTag("Node"))
		{
			Node node = other.GetComponent<Node>();

			Vector3 direction = Vector3.zero;
			float maxDistance = float.MinValue;

			// Find the available direction that moves farthest from pacman
			foreach(Vector3 availableDirection in node.availableDirections)
			{
				// If the distance in this direction is greater than the current
				// max distance then this direction becomes the new farthest
				Vector3 newPosition = transform.position + availableDirection;
				float distance = (_ghost.pacman.transform.position - newPosition).sqrMagnitude;

				if(distance > maxDistance)
				{
					direction = availableDirection;
					maxDistance = distance;
				}
			}

			_ghost.movement.SetDirection(direction);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(enabled && collision.gameObject.CompareTag("Player"))
		{
			Eaten();
		}
	}
}
