using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{

	[SerializeField] float _movementSpeed = 3.0f;

	[SerializeField] Transform _firePos;

	[SerializeField] Laser _laserPrefab;

	[SerializeField] float _xLimit = 6.0f;

	bool _isFireCooldown = false;

	Laser _currentLaser;

	SoundManager _soundManager;

	private void Awake()
	{
		_soundManager = FindObjectOfType<SoundManager>();
	}

	void Start()
	{

	}

	void Update()
	{
		HandleInput();
	}

	private void HandleInput()
	{
		float moveInput = Input.GetAxisRaw("Horizontal");

		Move(moveInput);

		if(Input.GetButtonDown("Fire"))
		{
			if(!_isFireCooldown)
			{
				Fire();
			}
		}
	}

	private void Move(float moveInput)
	{
		transform.position = transform.position + Vector3.right * moveInput * _movementSpeed * Time.deltaTime;
		// TODO Need to limit movement

		if(transform.position.x > _xLimit)
			transform.position = new Vector2(_xLimit, transform.position.y);
		if(transform.position.x < -_xLimit)
			transform.position = new Vector2(-_xLimit, transform.position.y);
	}

	private void Fire()
	{
		// Instantiate bullet
		if(_currentLaser == null)
		{
			_currentLaser = Instantiate(_laserPrefab, _firePos.position, Quaternion.identity);
			_soundManager.PlayPlayerLaserSound();
		}

	}

	private void OnDestroy()
	{
		GameManager gm = FindObjectOfType<GameManager>();
		if(gm != null)
		{
			gm.Lose();
			_soundManager.PlayPlayerDeathSound();
		}
	}
}
