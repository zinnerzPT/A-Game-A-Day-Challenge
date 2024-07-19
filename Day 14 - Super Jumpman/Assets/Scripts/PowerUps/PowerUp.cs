using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			Activate(collision.gameObject.GetComponent<Player>());
		}
	}

	protected abstract void Activate(Player player);
}
