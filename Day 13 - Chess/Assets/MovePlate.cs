using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
	private GameManager _gameManager;

	Vector2Int _position;

	public bool _isAttack;

	SpriteRenderer _spriteRenderer;

	Piece _piece;

	public void Init(Piece piece, Vector2Int position)
	{
		_piece = piece;
		_position = position;
		transform.position = new Vector3(_position.x, _position.y);

		_isAttack = _gameManager.GetPiece(position) != null;
	}

	private void Awake()
	{
		_gameManager = FindObjectOfType<GameManager>();

		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		if(_isAttack)
			_spriteRenderer.color = Color.red;
	}

	public void OnMouseUp()
	{
		_gameManager.MovePiece(_piece, _position);
	}
}
