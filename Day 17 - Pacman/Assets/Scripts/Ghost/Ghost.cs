using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	[SerializeField] GhostBehaviour _initialBehaviour;

	[HideInInspector] public Movement movement { get; private set; }
	[HideInInspector] public GhostHome home { get; private set; }
	[HideInInspector] public GhostScatter scatter { get; private set; }
	[HideInInspector] public GhostChase chase { get; private set; }
	//[HideInInspector] public GhostFrightened frightened { get; private set; }

	[HideInInspector] public Pacman pacman { get; private set; }

	public int points = 200;

	GameManager _gameManager;

	private void Awake()
	{
		movement = GetComponent<Movement>();
		home = GetComponent<GhostHome>();
		scatter = GetComponent<GhostScatter>();
		chase = GetComponent<GhostChase>();
		//frightened = GetComponent<GhostFrightened>();

		pacman = FindObjectOfType<Pacman>();
		_gameManager = FindObjectOfType<GameManager>();
	}

	public void ResetState()
	{
		gameObject.SetActive(true);
		movement.ResetState();
		//frightened.Disable();
		chase.Disable();
		scatter.Enable();

		if(home != _initialBehaviour)
			home.Disable();

		if(_initialBehaviour != null)
			_initialBehaviour.Enable();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			//if(frightened.enabled)
			//{
			//	_gameManager.GhostEaten(this);
			//}
			//else
			//{
			_gameManager.PacmanEaten();
			//}
		}
	}
}
