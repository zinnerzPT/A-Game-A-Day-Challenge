using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
	[SerializeField] private GameObject _completedSprite;

	private bool _completed;

	private GameManager _gameManager;

	private void Awake()
	{
		_gameManager = FindObjectOfType<GameManager>();
	}

	public void Initialize()
	{
		_completed = false;
		_completedSprite.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			_completed = true;
			_completedSprite.SetActive(true);

			Destroy(collision.gameObject);

			_gameManager.Score();
		}
	}
}
