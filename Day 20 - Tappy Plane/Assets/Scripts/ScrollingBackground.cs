using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
	[SerializeField] private float _scrollSpeed = 1.0f;

	private GameManager _gameManager;
	private void Awake()
	{
		_gameManager = FindObjectOfType<GameManager>();
	}

	void Update()
	{
		if(_gameManager.isGameOver)
			return;

		Vector3 pos = transform.position;

		pos.x -= _scrollSpeed * Time.deltaTime;

		if(pos.x < -8.0f)
			pos.x += 16.0f;

		transform.position = pos;
	}
}
