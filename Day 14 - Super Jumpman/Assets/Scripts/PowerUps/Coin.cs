using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PowerUp
{
	SoundManager _soundManager;

	private void Awake()
	{
		_soundManager = FindObjectOfType<SoundManager>();
	}

	private void Start()
	{
		_soundManager.PlayCoinSound();
		Destroy(gameObject, 0.5f);
	}

	protected override void Activate(Player player)
	{

	}
}
