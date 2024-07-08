using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
	[SerializeField] float _limitYPos = 3.0f;
	[SerializeField] float _speed = 1.0f;

	[SerializeField] PlayerType _playerType;

	PlayerInput _playerInput;
	PlayerInputActions _playerInputActions;

	Ball _ball;

	public PlayerType PlayerType
	{
		get => _playerType;
	}

	public void SetBall(Ball ball)
	{
		_ball = ball;
	}

	private void Awake()
	{
		if(_playerType == PlayerType.CPU)
			return;

		_playerInput = GetComponent<PlayerInput>();

		_playerInputActions = new PlayerInputActions();

		if(_playerType == PlayerType.Player1)
			_playerInputActions.Player1.Enable();

		if(_playerType == PlayerType.Player2)
			_playerInputActions.Player2.Enable();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	void Update()
	{
		switch(_playerType)
		{
			case PlayerType.Player1:
				HandleInput(1);
				break;
			case PlayerType.Player2:
				HandleInput(2);
				break;
			case PlayerType.CPU:
				HandleNPCAI();
				break;
		}
	}

	void HandleInput(int player)
	{
		Vector2 input = new Vector2();
		switch(player)
		{
			case 1:
				input = _playerInputActions.Player1.Movement.ReadValue<Vector2>();
				break;
			case 2:
				input = _playerInputActions.Player2.Movement.ReadValue<Vector2>();
				break;
		}

		Move(input);
	}

	void HandleNPCAI()
	{
		if(_ball == null)
			return;

		Move(new Vector2(0.0f, _ball.transform.position.y - transform.position.y).normalized);
	}

	public void Move(Vector2 direction)
	{
		transform.position = (Vector2)transform.position + direction * _speed * Time.deltaTime;

		if(transform.position.y > _limitYPos)
		{
			transform.position = new Vector2(transform.position.x, _limitYPos);
		}
		else if(transform.position.y < -_limitYPos)
		{
			transform.position = new Vector2(transform.position.x, -_limitYPos);
		}
	}
}

public enum PlayerType
{
	CPU = 0,
	Player1 = 1,
	Player2 = 2
}