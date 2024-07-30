using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
	[SerializeField] private float _minX;
	[SerializeField] private float _xIncrement;

	private float _scrollSpeed = 1.0f;

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

		if(pos.x < _minX)
			pos.x += _xIncrement;

		transform.position = pos;
	}

	public void SetSpeed(float speed)
	{
		_scrollSpeed = speed;
	}
}
