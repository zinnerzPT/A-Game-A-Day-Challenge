using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ghost))]
public abstract class GhostBehaviour : MonoBehaviour
{
	[SerializeField] protected float _duration;

	protected Ghost _ghost;

	private void Awake()
	{
		_ghost = GetComponent<Ghost>();
		enabled = false;
	}

	public void Enable()
	{
		Enable(_duration);
	}

	public virtual void Enable(float duration)
	{
		enabled = true;

		CancelInvoke(nameof(Disable));
		Invoke(nameof(Disable), duration);
	}

	public virtual void Disable()
	{
		enabled = false;

		CancelInvoke();
	}
}
