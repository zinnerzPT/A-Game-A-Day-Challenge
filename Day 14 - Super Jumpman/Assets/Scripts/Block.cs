using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
	SpriteRenderer _spriteRenderer;
	Animator _animator;

	[SerializeField] Sprite _depletedSprite;

	protected bool _isDepleted;

	protected SoundManager _soundManager;

	protected void Awake()
	{
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		_animator = GetComponent<Animator>();

		_soundManager = FindObjectOfType<SoundManager>();
	}

	public abstract void Activate(Strength strength);

	protected void SetActivated()
	{
		if(_isDepleted)
			return;
		Deplete();
		DepleteSprite();
		PlayActivateAnimation();
	}

	protected virtual void PlayActivateAnimation()
	{
		_animator.SetTrigger("Activate");
	}

	protected virtual void DepleteSprite()
	{
		_spriteRenderer.sprite = _depletedSprite;
	}

	protected virtual void Deplete()
	{
		_isDepleted = true;
	}
}

public enum Strength
{
	Weak,
	Strong,
	Indestructible
}
