using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
	[SerializeField] private Sprite[] _diceSprites;

	private SpriteRenderer _spriteRenderer;

	private GameManager _gameManager;
	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_gameManager = FindObjectOfType<GameManager>();
	}

	private void OnMouseDown()
	{
		_gameManager.RollDice();
	}

	public int RollDice()
	{
		int result = Random.Range(0, _diceSprites.Length);

		_spriteRenderer.sprite = _diceSprites[result];

		// Convert range to 1-6
		return result + 1;
	}
}
