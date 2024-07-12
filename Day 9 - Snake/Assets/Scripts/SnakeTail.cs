using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTail : MonoBehaviour
{
	[SerializeField] Sprite _straightSprite;
	[SerializeField] Sprite _turningRightSprite;
	[SerializeField] Sprite _turningLeftSprite;
	[SerializeField] Sprite _tailSprite;

	TailState _state;

	SpriteRenderer _spriteRenderer;
	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void MakeState(Vector2Int from, Vector2Int through, Vector2Int to)
	{
		Vector2Int fromDirection = through - from;
		Vector2Int toDirection = to - through;

		if(fromDirection == toDirection)
		{
			_state = TailState.Straight;
			_spriteRenderer.sprite = _straightSprite;
		}
		else if(Vector2.SignedAngle(fromDirection, toDirection) > 0.0f)
		{
			_state = TailState.TurningLeft;
			_spriteRenderer.sprite = _turningLeftSprite;
		}
		else
		{
			_state = TailState.TurningRight;
			_spriteRenderer.sprite = _turningRightSprite;
		}
	}

	public void MakeTail()
	{
		_state = TailState.EndTail;
		_spriteRenderer.sprite = _tailSprite;
	}
}

public enum TailState
{
	Straight,
	TurningRight,
	TurningLeft,
	EndTail
}
