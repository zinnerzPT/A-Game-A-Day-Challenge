using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBlock : Block
{
	public override void Activate(Strength strength)
	{
		if(strength == Strength.Strong)
		{
			_soundManager.PlayBlockBreakSound();
			Destroy(gameObject);
		}
		else
		{
			PlayActivateAnimation();
		}
	}
}
