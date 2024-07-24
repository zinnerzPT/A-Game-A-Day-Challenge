using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private Ghost[] _ghosts;
	private Pacman _pacman;
	[SerializeField] private Transform _pellets;


	public int ghostMultiplier { get; private set; } = 1;
	public int score { get; private set; }
	public int lives { get; private set; }

	private void Awake()
	{
		_pacman = FindObjectOfType<Pacman>();
		_ghosts = FindObjectsByType<Ghost>(FindObjectsSortMode.None);
	}

	private void Start()
	{
		NewGame();
	}

	private void Update()
	{
		if(lives <= 0 && Input.anyKeyDown)
		{
			NewGame();
		}
	}

	private void NewGame()
	{
		SetScore(0);
		SetLives(3);
		NewRound();
	}

	private void SetScore(int score)
	{
		this.score = score;
	}

	private void SetLives(int lives)
	{
		this.lives = lives;
	}

	private void NewRound()
	{
		foreach(Transform pellet in _pellets)
		{
			pellet.gameObject.SetActive(true);
		}

		ResetState();
	}

	private void ResetState()
	{
		for(int i = 0; i < _ghosts.Length; ++i)
		{
			_ghosts[i].ResetState();
		}

		_pacman.ResetState();

		ResetGhostMultiplier();
	}

	private void GameOver()
	{
		for(int i = 0; i < _ghosts.Length; ++i)
		{
			_ghosts[i].gameObject.SetActive(false);
		}

		_pacman.gameObject.SetActive(false);
	}


	public void GhostEaten(Ghost ghost)
	{
		int points = ghost.points * ghostMultiplier;
		SetScore(score + points);

		ghostMultiplier++;
	}

	public void PacmanEaten()
	{
		_pacman.gameObject.SetActive(false);

		SetLives(lives - 1);

		if(lives > 0)
			Invoke(nameof(ResetState), 3.0f);
		else
			GameOver();
	}

	public void PelletEaten(Pellet pellet)
	{
		pellet.gameObject.SetActive(false);
		SetScore(score + pellet.points);

		if(!HasRemainingPellets())
		{
			_pacman.gameObject.SetActive(false);
			Invoke(nameof(NewRound), 3.0f);
		}
	}

	public void PowerPelletEaten(PowerPellet powerPellet)
	{
		for (int i = 0; i<_ghosts.Length; ++i)
		{
			//_ghosts[i].frightened.Enable(powerPellet.duration);
		}

		PelletEaten(powerPellet);

		CancelInvoke(nameof(ResetGhostMultiplier));
		Invoke(nameof(ResetGhostMultiplier), powerPellet.duration);
	}

	private bool HasRemainingPellets()
	{
		foreach(Transform pellet in _pellets)
		{
			if(pellet.gameObject.activeSelf)
			{
				return true;
			}
		}

		return false;
	}

	private void ResetGhostMultiplier()
	{
		ghostMultiplier = 1;
	}
}
